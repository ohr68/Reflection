using System.Reflection;
using ByteBank.Common;
using ByteBank.Common.Interfaces;
using ByteBank.Common.Plugin;

MostrarBanner();

while (true)
{
    MostrarMenu();

    if (int.TryParse(Console.ReadLine(), out int escolha))
    {
        ExecutarEscolha(escolha);
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}

static void MostrarBanner()
{
    Console.WriteLine(@"


    ____        __       ____              __      
   / __ )__  __/ /____  / __ )____ _____  / /__    
  / __  / / / / __/ _ \/ __  / __ `/ __ \/ //_/    
 / /_/ / /_/ / /_/  __/ /_/ / /_/ / / / / ,<       
/_____/\__, /\__/\___/_____/\__,_/_/ /_/_/|_|      
      /____/                                       
                                
        ");
}

static void MostrarMenu()
{
    Console.WriteLine("\nEscolha uma opção:");
    Console.WriteLine();
    Console.WriteLine("1. Ler arquivo de boletos");
    Console.WriteLine();
    Console.Write("Digite o número da opção desejada: ");
}

static void ExecutarEscolha(int escolha)
{
    switch (escolha)
    {
        case 1:
            LerArquivoBoletos();
            break;
        case 2:
            GravarGrupoBoleto();
            break;
        case 3:
            ExecutarPlugins();
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

static void LerArquivoBoletos()
{
    Console.WriteLine("Lendo arquivo de boletos...");

    var leitorDeBoleto = new LeitorDeBoleto();
    List<Boleto> boletos = leitorDeBoleto.LerBoletos("Boletos.csv");

    foreach (var boleto in boletos)
    {
        Console.WriteLine(
            $"Cedente: {boleto.CedenteNome}, Valor: {boleto.Valor:#0.00}, Vencimento: {boleto.DataVencimento}");
    }
}

static void GravarGrupoBoleto()
{
    Console.WriteLine("Gravando grupo boletos...");

    LeitorDeBoleto leitorDeBoleto = new();
    var boletos = leitorDeBoleto.LerBoletos("Boletos.csv");

    // var geradorDeCsv = new Relatorio("BoletosPorCedente.csv", DateTime.Now);
    // geradorDeCsv.Processar(boletos);

    var nomeParametroConstrutor = "nomeArquivoSaida";
    var parametroConstrutor = "BoletosPorCedente.csv";
    var nomeMetodo = "Processar";
    var parametroMetodo = boletos;

    ProcessarDinamicamente(nomeParametroConstrutor, parametroConstrutor,
        nomeMetodo, parametroMetodo);
}

static void ProcessarDinamicamente(string nomeParametroConstrutor, string parametroConstrutor, string nomeMetodo,
    List<Boleto> parametroMetodo)
{
    var tipoClasseRelatorio = typeof(Relatorio);
    var construtores = tipoClasseRelatorio.GetConstructors();

    var construtor = construtores.Single(c => c.GetParameters().Any(p => p.Name == nomeParametroConstrutor));

    var instanciaClasse = construtor.Invoke(new object[] { parametroConstrutor });
    var metodoProcessar = tipoClasseRelatorio.GetMethod(nomeMetodo);

    metodoProcessar?.Invoke(instanciaClasse, new object[] { parametroMetodo });
}

static void ExecutarPlugins()
{
    LeitorDeBoleto leitorCsv = new();
    var boletos = leitorCsv.LerBoletos("Boletos.csv");

    var classesDePlugin = PluginHelper.ObterClassesDePlugin<IRelatorio<Boleto>>();

    foreach (var classe in classesDePlugin)
    {
        var plugin = Activator.CreateInstance(classe, new object[] { "BoletosPorCedente.csv" });

        var metodoProcessar = classe.GetMethod("Processar");
        metodoProcessar?.Invoke(plugin, new object[] { boletos });
    }
}