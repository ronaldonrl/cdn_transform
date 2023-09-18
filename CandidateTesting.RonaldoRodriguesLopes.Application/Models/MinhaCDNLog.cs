using System.Globalization;
using System.Text;
using CandidateTesting.RonaldoRodriguesLopes.Application.Helpers;

namespace CandidateTesting.RonaldoRodriguesLopes.Application.Models
{
    internal class MinhaCDNLog
    {
        public static char DelimiterCharacter { get; } = ' ';
        public int ResponseSize { get; set; }
        public int StatusCode { get; set; }
        public string CacheStatus { get; set; }
        public float TimeTaken { get; set; }
        public string HttpMethodUriPath { get; set; }

        public MinhaCDNLog(int responseSize, int statusCode, string cacheStatus, string httpMethodUriPath, float timeTaken)
        {
            ResponseSize = responseSize;
            StatusCode = statusCode;
            CacheStatus = cacheStatus;
            TimeTaken = timeTaken;
            HttpMethodUriPath = httpMethodUriPath;
        }

        public static implicit operator MinhaCDNLog(string line)
        {
            var values = FileLogHelper.SplitLineMessage(line);

            return new MinhaCDNLog(
                    int.Parse(values[0].ToString()),
                    int.Parse(values[1].ToString()),
                    values[2].ToString(),
                    values[3].ToString(),
                    float.Parse(values[4], CultureInfo.InvariantCulture)
                );
        }
    }    
}
