using ByteBank.Common.Attributes;

namespace ByteBank.Common;

public class BoletosPorCedente
{
    [NomeColuna("Nome")]
    public string? Nome { get; set; }
    
    [NomeColuna("Documento")]
    public string? Documento { get; set; }
    
    [NomeColuna("Agencia")]
    public string? Agencia { get; set; }
    
    [NomeColuna("Conta")]
    public string? Conta { get; set; }
    
    [NomeColuna("Total")]
    public decimal Valor { get; set; }
    
    [NomeColuna("Qtd")]
    public int Quantidade { get; set; }
}