using CandidateTesting.RonaldoRodriguesLopes.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {

        ConfigurationBuilder();

        IHost host = CreateHostBuilder(args)
            .UseSerilog()
            .Build();

        try
        {
            //sourceUrl
            //var pathFile = args[0];
            var pathFile = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";

            //targetPath
            //var pathFileOut = args[1];
            var pathFileOut = "./output/agora.txt";

            //Call Service to Download file
            ILogFileService _logService = host.Services.GetRequiredService<ILogFileService>();
            var isDownloaded = await _logService.DownloadLogFile(pathFile);

            if (isDownloaded)
            {
                var fileString = await _logService.ReadLogFileToProcess();
                if(string.IsNullOrEmpty(fileString))
                    throw new Exception($"Could not read file in {pathFile}");

                _logService.ProcessLogFile(fileString, pathFileOut);
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

    private static void ConfigurationBuilder()
    {        
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
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((opt, services) =>
            {
                services.AddTransient<ILogFileService, LogFileService>();
            });
}