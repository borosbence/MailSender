using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MailSender.Services;

namespace MailSender.ViewModels
{
    public class MailViewModel : ObservableObject
    {
        public required string From { get; set; }
        public required string To { get; set; }
        public string? Subject { get; set; }

        private string? _body;
        public string? Body
        {
            get { return _body; }
            set { 
                SetProperty(ref _body, value);
                OnPropertyChanged(nameof(HtmlBody));
            }
        }
        public string? HtmlBody => Body?.ReplaceLineEndings("<br />");

        public static string FromToolTip => "Az e-mail cím után szóközzel tudja megadni a megjelenítendő nevét.";
        public static string ToToolTip => FromToolTip + "Több címzett esetén pontos vesszővel válassza el az értékeket.";
        public static string HtmlBodyToolTip => "Az üzenetet HTML formátumban is el tudja küldeni.";

        private bool _isSending;
        public bool IsSending
        {
            get { return _isSending; }
            set { SetProperty(ref _isSending, value); }
        }

        private bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { SetProperty(ref _visible, value); }
        }

        private readonly EmailService _emailService;
        private readonly LogService _logService;

        public IAsyncRelayCommand SendMailCommandAsync { get; }

        public MailViewModel(LogService logService, EmailService emailService)
        {
            _logService = logService;
            _emailService = emailService;

            SendMailCommandAsync = new AsyncRelayCommand(SendMail);
        }

        private async Task SendMail()
        {
            IsSending = true;
            Visible = false;
            await Task.Delay(1000);
            if (!string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(To))
            {
                string[] fromArray = GetDisplayName(From);
                string[] addresses = To.Split(';');
                foreach (var address in addresses)
                {
                    string[] toArray = GetDisplayName(address);
                    try
                    {
                        await _emailService.SendEmailAsync(fromArray[0], toArray[0], Subject, Body, fromArray[1], toArray[1]);
                        _logService.Append($"Sikeres üzenet küldés: {toArray[0]} .");
                    }
                    catch (Exception ex)
                    {
                        _logService.Append($"Hiba: {ex.Message}");
                    }
                }
            }
            else 
            {                  
                _logService.Append("Hiányzik a feladó vagy a címzett e-mail címe.");
            }
            WeakReferenceMessenger.Default.Send("update");
            IsSending = false;
            Visible = true;
        }

        private string[] GetDisplayName(string inputText)
        {
            string[] result = new string[2];
            string[] inputArray = inputText.Split(' ');
            result[0] = inputArray[0];
            result[1] = inputArray.Length > 0 ? string.Join(" ", inputText.Skip(1)) : string.Empty;
            return result;
        }
    }
}
