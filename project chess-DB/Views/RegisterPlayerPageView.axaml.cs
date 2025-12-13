using Avalonia.Controls;
using Avalonia.Interactivity;

namespace project_chess_DB.Views;

public partial class RegisterPlayerPageView : Window
{
    public RegisterPlayerPageView()
    {
        InitializeComponent();
    }
    public void OnCancelClick(object sender, RoutedEventArgs e)
    {
        Close(null);
    }
    public void OnConfirmClick(object sender, RoutedEventArgs e)
    {
        var nameBox = this.FindControl<TextBox>("Matricule");
        Close(nameBox?.Text);
    }
}
