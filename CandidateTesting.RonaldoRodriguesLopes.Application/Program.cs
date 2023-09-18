using CandidateTesting.RonaldoRodriguesLopes.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {

        //Configuration Builder Appsettings
        var configuration = new ConfigurationBuilder();
        configuration.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        //Configuration Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        //Configurarion DI
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((opt, services) =>
            {
                services.AddTransient<ILogFileService, LogFileService>();
            })
            .UseSerilog()
            .Build();       

        try
        {
            var pathFile = args[0];
            var pathFileOut = args[1];

            //Call Service to Download file
            ILogFileService _logService = host.Services.GetRequiredService<ILogFileService>();
            var isDownloaded = await _logService.DownloadLogFile(pathFile);

            if (isDownloaded)
            {
                var _file = await _logService.ReadLogFileToProcess();
                _logService.ProcessLogFile(_file, pathFileOut);
            }
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Please provide URL (sourceUrl) and a destination file (targetPath) as arguments.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error - [Main]: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press key to close.");
            Console.ReadLine();
        }
    }
}