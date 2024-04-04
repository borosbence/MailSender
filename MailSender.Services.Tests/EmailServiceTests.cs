namespace MailSender.Services.Tests
{
    [TestClass()]
    public class EmailServiceTests
    {
        private const string USERNAME = "FELHASZNÁLÓNÉV";
        private const string PASSWORD = "JELSZÓ";
        private const string HOST = "smtp.gmail.com";
        private const int PORT = 587;

        [TestMethod()]
        public async Task SendMailAsyncTest()
        {

            var emailService = new EmailService(USERNAME, PASSWORD, HOST, PORT);
            try
            {
                string to = "CÍMZETT";
                string subject = "TÁRGY";
                string body = "<h1>Hello Világ</h1>";
                string fromName = "KÜLDŐ NEVE";
                await emailService.SendEmailAsync(to, subject, body, fromName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                return;
            }
            Assert.IsTrue(true);
        }
    }
}