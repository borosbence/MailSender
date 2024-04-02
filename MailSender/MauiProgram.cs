using CommunityToolkit.Maui;
using MailSender.Services;
using MailSender.ViewModels;
using MailSender.Views;
using Microsoft.Extensions.Logging;

namespace MailSender
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<LogService>();
            builder.Services.AddTransient(x =>
            {
                return new EmailService("FELHASZNÁLÓNÉV", "JELSZÓ", "smtp.gmail.com", 587);
            });

            builder.Services.AddTransient<MailViewModel>();
            builder.Services.AddTransient<MailPage>();

            builder.Services.AddTransient<LogViewModel>();
            builder.Services.AddTransient<LogPage>();

            return builder.Build();
        }
    }
}
