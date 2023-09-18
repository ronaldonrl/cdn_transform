using CandidateTesting.RonaldoRodriguesLopes.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CandidateTesting.RonaldoRodriguesLopes.Test
{
    public class LogFileTest
    {
        [Fact]
        public async void TestDownloadLogFileSuccess()
        {
            //Arrange
            var pathFile = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";
            var logFileServiceMock = new Mock<ILogger<LogFileService>>();

            var logFileService = new LogFileService(logFileServiceMock.Object);

            //ACT
            var result = await logFileService.DownloadLogFile(pathFile);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void TestDownloadLogFileError()
        {
            //Arrange
            var pathFile = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/testeFileNotExist.txt";
            var logFileServiceMock = new Mock<ILogger<LogFileService>>();

            var logFileService = new LogFileService(logFileServiceMock.Object);

            //ACT
            var result = await logFileService.DownloadLogFile(pathFile);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void TestReadLogFileToProcessSuccess()
        {
            //Arrange
            var pathFile = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";
            var logFileServiceMock = new Mock<ILogger<LogFileService>>();

            var logFileService = new LogFileService(logFileServiceMock.Object);
            await logFileService.DownloadLogFile(pathFile);            

            //ACT
            var result = await logFileService.ReadLogFileToProcess();

            //Assert
            Assert.True(!string.IsNullOrEmpty(result));
        }

        [Fact]
        public async void TestReadLogFileToProcessError()
        {
            //Arrange            
            var logFileServiceMock = new Mock<ILogger<LogFileService>>();
            var logFileService = new LogFileService(logFileServiceMock.Object);
            var pathToFile = Directory.GetCurrentDirectory() + "\\minhacdn.txt";
            File.Delete(pathToFile);

            //ACT
            var result = await logFileService.ReadLogFileToProcess();

            //Assert
            Assert.True(string.IsNullOrEmpty(result));
        }

        [Fact]
        public async void TestProcessLogFileSuccess()
        {
            //Arrange            
            var pathFile = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";
            var logFileServiceMock = new Mock<ILogger<LogFileService>>();
            var logFileService = new LogFileService(logFileServiceMock.Object);
            
            await logFileService.DownloadLogFile(pathFile);
            var fileLog = await logFileService.ReadLogFileToProcess();
            var pathFileOut = "./output/minhaCdn1.txt";            

            //ACT
            var result = await logFileService.ProcessLogFile(fileLog, pathFileOut);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void TestProcessLogFileError()
        {
            //Arrange
            var logFileServiceMock = new Mock<ILogger<LogFileService>>();
            var logFileService = new LogFileService(logFileServiceMock.Object);
            var fileLog = string.Empty;
            var pathFileOut = "./output/minhaCdn1.txt";            

            //ACT
            var result = await logFileService.ProcessLogFile(fileLog, pathFileOut);

            //Assert
            Assert.False(result);
        }
    }
}