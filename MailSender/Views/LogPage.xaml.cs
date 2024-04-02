using MailSender.ViewModels;

namespace MailSender.Views;

public partial class LogPage : ContentPage
{
	public LogPage(LogViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}