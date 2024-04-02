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
                string[] addresses = To.Split(';');
                foreach (var address in addresses)
                {
                    string[] toTextArray = address.Split(' ');
                    string toAddress = toTextArray[0];
                    string? toName = toTextArray.Length > 0 ? string.Join(" ", toTextArray.Skip(1)) : null;
                    try
                    {
                        await _emailService.SendEmailAsync(toAddress, Subject, Body, From, toName);
                        _logService.Append($"Sikeres üzenet küldés: {toAddress} .");
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

    }
}
