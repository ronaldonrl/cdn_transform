using CandidateTesting.RonaldoRodriguesLopes.Application.Models;
using Microsoft.Extensions.Logging;
using System.IO;

namespace CandidateTesting.RonaldoRodriguesLopes.Application.Services
{
    public class LogFileService : ILogFileService
    {
        private readonly ILogger<LogFileService> _logger;
        private const string FileNameMinhaCdn = "minhacdn.txt";
        private string PathToFile { get; set; } = string.Empty;

        public LogFileService(ILogger<LogFileService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> DownloadLogFile(string pathHttpFile)
        {
            using var client = new HttpClient();

            try
            {
                HttpResponseMessage st = await client.GetAsync(pathHttpFile);

                if(!st.IsSuccessStatusCode)
                    throw new AccessViolationException("File not find.");

                string fileContent = await st.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(fileContent))
                    throw new ArgumentNullException("File not content.");

                File.WriteAllText(FileNameMinhaCdn, fileContent);

                _logger.LogInformation("Download file success...");
                
                return true;

            } 
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error - [DownloadLogFile][HttpRequestException]: {ex.Message}");
                return false;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"Error - [DownloadLogFile][ArgumentNullException]: {ex.Message}");
                return false;
            }
            catch (AccessViolationException ex)
            {
                _logger.LogError($"Error - [DownloadLogFile][HttpRequestException]: {ex.Message}");
                return false;
            }
        }

        public async Task<string> ReadLogFileToProcess()
        {

            PathToFile = Directory.GetCurrentDirectory() + "\\" + FileNameMinhaCdn;

            try
            {
                if(!File.Exists(PathToFile))
                    throw new FileNotFoundException("File does not exist.");

                var fs = new FileStream(PathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var sr = new StreamReader(fs);
                var fileString = await sr.ReadToEndAsync();
                sr.Close();

                return fileString;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Erro - [ReadFileLogToProcess][FileNotFoundException]: {ex.Message}");
                return string.Empty;
            }
            catch (IOException ex)
            {
                _logger.LogError($"Erro - [ReadFileLogToProcess][IOException]: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task<bool> ProcessLogFile(string logFile, string pathFileOut)
        {
            _logger.LogInformation("Process started...");

            if (!ValidateLogFile(logFile))
                return false;

            _logger.LogInformation("File log validated...");

            //Transform log file MinhaCDN to Properties
            IEnumerable<string> file = File.ReadLines(PathToFile);
            IEnumerable<MinhaCDNLog> arrayLogMinhaCdn = file.Select(linha => (MinhaCDNLog)linha).ToList();            
            IEnumerable<AgoraLog> arrayLogAgora = arrayLogMinhaCdn.Select(minhaCdnLog => (AgoraLog)minhaCdnLog).ToList();

            //Transform log file MinhaCDN to log Agora
            IEnumerable<string> arrayStringLogAgora = arrayLogAgora.Select(agoraLog => (string)agoraLog);

            return CreateLogFileAgora(arrayStringLogAgora, pathFileOut);
        }

        private bool ValidateLogFile(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                    throw new Exception("No Log found.");

                if (file.Length < 10)
                    throw new Exception("Log length too short.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private bool CreateLogFileAgora(IEnumerable<string> logsAgora, string pathFileOut)
        {
            try
            {
                string[] arryFields = new string[7] {                    
                    "provider",
                    "http-method",
                    "status-code",
                    "uri-path",
                    "time-taken",
                    "response-size",
                    "cache-status"
                };

                var newLogAgora = logsAgora.ToList();

                newLogAgora.Insert(0, "#Version: 1.0");
                newLogAgora.Insert(1, "#Date: " + DateTime.Now);
                newLogAgora.Insert(2, "#Fields: " + string.Join(" ", arryFields));

                if (string.IsNullOrEmpty(pathFileOut))
                    throw new IOException("Path file out null.");

                CreateDirectoryPath(pathFileOut);
                File.WriteAllLines(pathFileOut, newLogAgora);

                _logger.LogInformation("File log Agora created success.");

                return true;

            }
            catch (IOException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private void CreateDirectoryPath(string pathFileOut)
        {
            try
            {
                string directory = Path.GetDirectoryName(pathFileOut);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogError($"Erro - [VerifyDirectoryPath][DirectoryNotFoundException]: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro - [VerifyDirectoryPath][Exception]: {ex.Message}");
            }
        }
    }
}
