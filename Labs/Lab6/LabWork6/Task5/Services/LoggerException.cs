namespace Task5.Services
{
    public class LoggerException
    {
        public static void LogExceptionToFile(string message)
        {
            try
            {
                using(var streamWriter = new StreamWriter("logException.log", true))
                {
                    streamWriter.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy")}] - {message}");
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"Не удалось записать в файл логов {ex.Message}");
            }
        }
    }
}
