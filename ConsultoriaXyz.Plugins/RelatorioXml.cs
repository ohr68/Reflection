using System.Xml;
using ByteBank.Common;
using ByteBank.Common.Interfaces;

namespace ConsultoriaXyz.Plugins;

public class RelatorioXml : IRelatorio<Boleto>
{
    private const string PastaDestino = @"C:\Plugins";

    public void Processar(List<Boleto> boletos)
    {
        var boletosPorCedenteList = PegaBoletosAgrupados(boletos);

        GravarArquivo(boletosPorCedenteList);
    }

    private void GravarArquivo(List<BoletosPorCedente> boletosPorCedenteList)
    {
        var caminhoArquivo = Path.Combine(PastaDestino, $"{nameof(BoletosPorCedente)}.xml");

        var propriedadesObjeto = typeof(BoletosPorCedente).GetProperties();

        var configXml = new XmlWriterSettings { Indent = true };
        using (var xmlWriter = XmlWriter.Create(caminhoArquivo, configXml))
        {
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("BoletosPorCedente");

            foreach (var boletoPorCedente in boletosPorCedenteList)
            {
                xmlWriter.WriteStartElement("BoletoPorCedente");

                foreach (var propriedade in propriedadesObjeto)
                {
                    xmlWriter.WriteStartElement(propriedade.Name);
                    xmlWriter.WriteString(propriedade.GetValue(boletoPorCedente)?.ToString() ?? string.Empty);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
        }

        Console.WriteLine($"Arquivo '{caminhoArquivo}' criado com sucesso!");
    }

    private List<BoletosPorCedente> PegaBoletosAgrupados(List<Boleto> boletos)
    {
        var boletosAgrupados = boletos.GroupBy(b => new
        {
            b.CedenteNome,
            b.CedenteCpfCnpj,
            b.CedenteAgencia,
            b.CedenteConta
        });

        List<BoletosPorCedente> boletosPorCedente = new();

        foreach (var grupo in boletosAgrupados)
        {
            BoletosPorCedente boletoPorCedente = new BoletosPorCedente
            {
                Nome = grupo.Key.CedenteNome,
                Documento = grupo.Key.CedenteCpfCnpj,
                Agencia = grupo.Key.CedenteAgencia,
                Conta = grupo.Key.CedenteConta,
                Valor = grupo.Sum(b => b.Valor),
                Quantidade = grupo.Count()
            };

            boletosPorCedente.Add(boletoPorCedente);
        }

        return boletosPorCedente;
    }
}