namespace MailSender.Services
{
    public class LogService
    {
        public List<string> Logs { get; } = [];

        public void Append(string text)
        {
            Logs.Add(text);
        }
    }
}