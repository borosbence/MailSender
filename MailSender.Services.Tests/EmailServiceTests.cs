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
        public async Task SendEmailAsyncTest()
        {
            var emailService = new EmailService(USERNAME, PASSWORD, HOST, PORT);
            try
            {
                string from = "KÜLDŐ EMAIL";
                string to = "CÍMZETT EMAIL";
                string subject = "TÁRGY";
                string body = "<h1>Hello Világ</h1>";
                await emailService.SendEmailAsync(from, to, subject, body);
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