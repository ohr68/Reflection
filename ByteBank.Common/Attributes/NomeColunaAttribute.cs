namespace ByteBank.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NomeColunaAttribute : Attribute
{
    public string? Cabecalho { get; }

    public NomeColunaAttribute(string cabecalho)
    {
        Cabecalho = cabecalho;
    }
}