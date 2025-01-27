using System.Reflection;

namespace ByteBank.Common.Plugin;

public static class PluginHelper
{
    public static IEnumerable<Type> ObterClassesDePlugin<T>()
    {
        List<Type> tiposEncontrados = new();

        var assemblyEmExecucao = Assembly.GetExecutingAssembly();
        var assemblyDosPlugins = typeof(T).Assembly;

        var tipos = assemblyDosPlugins.GetTypes();

        var tiposImplementandoT = ObterTiposDoAssembly<T>(assemblyDosPlugins);

        tiposEncontrados.AddRange(tiposImplementandoT);

        return tiposEncontrados;
    }

    private static IEnumerable<Type> ObterTiposDoAssembly<T>(Assembly assemblyDosPlugins)
    {
        var tipos = assemblyDosPlugins.GetTypes();

        foreach (var tipo in tipos)
        {
            Console.WriteLine($"Nome: {tipo.Name}");
            Console.WriteLine($"Nome completo: {tipo.FullName}");
            Console.WriteLine($"É classe: {tipo.IsClass}");
            Console.WriteLine($"É interface: {tipo.IsInterface}");
            Console.WriteLine($"É abstrato: {tipo.IsAbstract}");

            Console.WriteLine("Inferfaces implementadas:");
            foreach (var interfaceType in tipo.GetInterfaces())
            {
                Console.WriteLine($" - {interfaceType.Name}");
            }

            Console.WriteLine();
        }

        var tiposImplementandoT = tipos.Where(t => typeof(T).IsAssignableFrom(t)
                                                   && t is { IsClass: true, IsAbstract: false });

        return tiposImplementandoT;
    }

    private static List<Assembly> ObterAssembliesDePlugins()
    {
        const string diretorio = @"C:\Plugins";

        var assemblies = new List<Assembly>();

        // Obter todos os arquivos .dll na pasta
        string[] arquivosDll = Directory.GetFiles(diretorio, "*.dll");

        foreach (var arquivoDll in arquivosDll)
        {
            // Carregar o assembly a partir do arquivo DLL
            var assembly = Assembly.LoadFrom(arquivoDll);
            assemblies.Add(assembly);
        }

        return assemblies;
    }
}