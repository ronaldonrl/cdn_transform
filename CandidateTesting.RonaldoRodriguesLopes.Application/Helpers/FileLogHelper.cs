namespace CandidateTesting.RonaldoRodriguesLopes.Application.Helpers
{
    internal static class FileLogHelper
    {
        public static char delimiterCharacter = '|';

        public static List<string> SplitLineMessage(string message)
        {
            return (from m in message.Split(delimiterCharacter, StringSplitOptions.None)
                    where !string.IsNullOrWhiteSpace(m)
                    select m).ToList();
        }
    }
}
