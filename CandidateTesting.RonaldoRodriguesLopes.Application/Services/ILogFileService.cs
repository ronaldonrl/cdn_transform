namespace CandidateTesting.RonaldoRodriguesLopes.Application.Services
{
    public interface ILogFileService
    {
        Task<bool> DownloadLogFile(string pathHttpFile);
        Task<string> ReadLogFileToProcess();
        bool ProcessLogFile(string logFile, string pathFileOut);        
    }
}
