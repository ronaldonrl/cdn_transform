using System.Text;

namespace CandidateTesting.RonaldoRodriguesLopes.Application.Models
{
    internal class AgoraLog
    {
        public AgoraLog(int responseSize, int statusCode, string httpMethod, string uriPath, int timeTaken, string cacheStatus)
        {
            ResponseSize = responseSize;
            StatusCode = statusCode;
            HttpMethod = httpMethod;
            UriPath = uriPath;
            TimeTaken = timeTaken;
            CacheStatus = cacheStatus;
        }

        public static char DelimiterCharacter { get; } = ' ';
        public int ResponseSize { get; set; }
        public int StatusCode { get; set; }
        public string HttpMethod { get; set; }
        public string UriPath { get; set; }
        public int TimeTaken { get; set; }
        public string CacheStatus { get; set; }
        public string Provider { get; set; } = "\"MINHA CDN\"";

        public static implicit operator AgoraLog(MinhaCDNLog minhaCDNLog)
        {
            return new AgoraLog(
                minhaCDNLog.ResponseSize,
                minhaCDNLog.StatusCode,
                minhaCDNLog.HttpMethodUriPath.Split(DelimiterCharacter)[0].Replace("\"", ""),
                minhaCDNLog.HttpMethodUriPath.Split(DelimiterCharacter)[1],
                (int)Math.Round(minhaCDNLog.TimeTaken),
                minhaCDNLog.CacheStatus
                );
        }

        public static implicit operator string(AgoraLog agoraLog)
        {
            var line = new StringBuilder();
            line
                .Append(agoraLog.Provider)
                .Append(DelimiterCharacter)
                .Append(agoraLog.HttpMethod)
                .Append(DelimiterCharacter)
                .Append(agoraLog.StatusCode)
                .Append(DelimiterCharacter)
                .Append(agoraLog.UriPath)
                .Append(DelimiterCharacter)
                .Append(agoraLog.TimeTaken)
                .Append(DelimiterCharacter)
                .Append(agoraLog.ResponseSize)
                .Append(DelimiterCharacter)
                .Append(agoraLog.CacheStatus);

            return line.ToString();
        }
    }
}
