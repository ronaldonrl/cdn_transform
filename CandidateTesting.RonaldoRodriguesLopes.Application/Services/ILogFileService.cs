namespace CandidateTesting.RonaldoRodriguesLopes.Application.Services
{
    public interface ILogFileService
    {
        Task<bool> DownloadLogFile(string pathHttpFile);
        Task<string> ReadLogFileToProcess();
        Task<bool> ProcessLogFile(string logFile, string pathFileOut);        
    }
}
