using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Common
{
    public class LeitorDeBoleto
    {
        public List<Boleto> LerBoletos(string caminhoArquivo)
        {
            var boletos = new List<Boleto>();
            
            using (var reader = new StreamReader(caminhoArquivo))
            {
                // ler cabeçalho do arquivo CSV
                var linha = reader.ReadLine();
                var cabecalho = linha?.Split(',');

                while (!reader.EndOfStream)
                {
                    linha = reader.ReadLine();
                    var dados = linha?.Split(',');
                    
                    boletos.Add(MapearTextoParaObjeto<Boleto>(cabecalho!, dados!));
                }
            }

            return boletos;
        }

        private T MapearTextoParaObjeto<T>(string[] nomesPropriedades, string[] valoresPropriedades)
        {
            T instancia = Activator.CreateInstance<T>();
            
            for (int i = 0; i < nomesPropriedades.Length; i++)
            {
                var nomePropriedade = nomesPropriedades[i];
                var propertyInfo = instancia!.GetType().GetProperty(nomePropriedade);

                if (propertyInfo != null)
                {
                    var propertyType = propertyInfo.PropertyType;
                    var valor = valoresPropriedades[i];
                    
                    var valorConvertido = Convert.ChangeType(valor, propertyType);
                    
                    propertyInfo.SetValue(instancia, valorConvertido);
                }
            }
            
            return instancia;
        }
    }
}
