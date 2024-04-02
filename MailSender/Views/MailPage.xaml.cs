using MailSender.ViewModels;

namespace MailSender.Views;

public partial class MailPage : ContentPage
{
	public MailPage(MailViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsLoaded)
        {
            webView.Reload();
        }
    }
}