using Avalonia.Controls;
using project_chess_DB.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Input;
using System.Linq;
using project_chess_DB.Models;
using project_chess_DB.Services;
using System;

namespace project_chess_DB.Views;

public partial class RegisterPlayerPageView : Window
{
    public RegisterPlayerPageView()
    {
        InitializeComponent();
        var txtMatricule = this.FindControl<TextBox>("TxtMatricule");
        txtMatricule?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
    }
    private void Numeric_TextInput(object? sender, TextInputEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Text) && !e.Text.All(char.IsDigit))
        {
            e.Handled = true;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is RegisterPlayerViewModel vm)
        {
            vm.CloseAction = () => Close();
        }
    }
}
