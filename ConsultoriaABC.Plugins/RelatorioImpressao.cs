using ByteBank.Common;
using ByteBank.Common.Interfaces;

namespace ConsultoriaABC.Plugins;

public class RelatorioImpressao : IRelatorio<Boleto>
{
    private const string PastaDestino = @"C:\Plugins\Impressao";

    public void Processar(List<Boleto> boletos)
        => GerarDocumentos(boletos);

    private void GerarDocumentos(List<Boleto> boletos)
    {
        if (!Directory.Exists(PastaDestino))
            Directory.CreateDirectory(PastaDestino);

        foreach (var boleto in boletos)
        {
            var nomeArquivo = Path.Combine(PastaDestino, $"{boleto.NumeroDocumento}.txt");
            var documento = GerarDocumento(boleto);

            File.WriteAllText(nomeArquivo, documento);

            Console.WriteLine($"Boleto para impressão gerado: {nomeArquivo}");
        }
    }

    private string GerarDocumento(Boleto boleto)
    {
        string textoBoleto = $@"
------------------------------------------------------------------------------
                          BANCO Bytebank S.A.
                          Agência: {boleto.CedenteAgencia}       Conta: {boleto.CedenteConta}
------------------------------------------------------------------------------
CEDENTE: {boleto.CedenteNome}
CNPJ/CPF: {boleto.CedenteCpfCnpj}
------------------------------------------------------------------------------
SACADO: {boleto.SacadoNome}
CNPJ/CPF: {boleto.SacadoCpfCnpj}
Endereço: {boleto.SacadoEndereco}
------------------------------------------------------------------------------
Nº Documento: {boleto.NumeroDocumento}           Vencimento: {boleto.DataVencimento:dd/MM/yyyy}
Nosso Número: {boleto.NossoNumero}
------------------------------------------------------------------------------
Descrição                          Valor (R$)      Valor Documento (R$)
------------------------------------------------------------------------------
Pagamento de Serviços              {boleto.Valor,-14:N2}          {boleto.Valor,-19:N2}
------------------------------------------------------------------------------
Código de Barras: {boleto.CodigoBarras}
Linha Digitável: {boleto.LinhaDigitavel}
------------------------------------------------------------------------------
";

        return textoBoleto;
    }
}