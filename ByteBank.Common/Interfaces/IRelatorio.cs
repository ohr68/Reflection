namespace ByteBank.Common.Interfaces;

public interface IRelatorio<T>
{
    void Processar(List<T> boletos);
}