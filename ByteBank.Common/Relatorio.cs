using System.Reflection;
using ByteBank.Common.Attributes;
using ByteBank.Common.Interfaces;

namespace ByteBank.Common;

public class Relatorio : IRelatorio<Boleto>
{
    private readonly string _nomeArquivoSaida;
    private readonly DateTime _dataEmissaoRelatorio;

    public Relatorio(string nomeArquivoSaida)
    {
        _nomeArquivoSaida = nomeArquivoSaida;
        _dataEmissaoRelatorio = DateTime.Now;
    }
    
    public void Processar(List<Boleto> boletos)
    {
        var boletosPorCedente = PegaBoletosAgrupados(boletos);

        GravarArquivo(boletosPorCedente);
    }

    private void GravarArquivo(List<BoletosPorCedente> grupos)
    {
        var tipo = typeof(BoletosPorCedente);
        var propriedades = tipo.GetProperties();

        using (var sw = new StreamWriter(_nomeArquivoSaida))
        {
            var cabecalho = propriedades
                .Select(p => p.GetCustomAttribute<NomeColunaAttribute>()?.Cabecalho
                ?? p.Name);
            
            sw.WriteLine(string.Join(',', cabecalho));

            foreach (var grupo in grupos)
            {
                var valores = propriedades.Select(p => p.GetValue(grupo));
                sw.WriteLine(string.Join(',', valores));
            }
        }
        
        Console.Write($"Arquivo '{_nomeArquivoSaida}' criado com sucesso!");
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
            BoletosPorCedente boletoPorCedente = new()
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