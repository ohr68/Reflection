using System.Reflection;

namespace ByteBank.Common.Plugin;

public static class PluginHelper
{
    public static List<Type> ObterClassesDePlugin<T>()
    {
        List<Type> tiposEncontrados = new();

        var assemblyEmExecucao = Assembly.GetExecutingAssembly();
        var assemblyDosPlugins = typeof(T).Assembly;

        var tipos = assemblyDosPlugins.GetTypes();

        var tiposImplementandoT = tipos.Where(t => typeof(T).IsAssignableFrom(t) && 
                                                               t is { IsClass: true, IsAbstract: false });
        
        tiposEncontrados.AddRange(tiposImplementandoT);
        
        return tiposEncontrados;
    }
}