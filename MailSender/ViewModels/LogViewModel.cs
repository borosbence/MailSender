using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MailSender.Services;

namespace MailSender.ViewModels
{
    public class LogViewModel : ObservableObject
    {
        private readonly LogService _logService;

        private string? _logText;
        public string? LogText
        {
            get { return _logText; }
            set { SetProperty(ref _logText, value); }
        }

        public LogViewModel(LogService logService)
        {
            _logService = logService;
            UpdateView();
            WeakReferenceMessenger.Default.Register<string>(this, (r, m) => UpdateView());
        }

        private void UpdateView()
        {
            LogText = string.Join(Environment.NewLine, _logService.Logs);
        }
    }
}
