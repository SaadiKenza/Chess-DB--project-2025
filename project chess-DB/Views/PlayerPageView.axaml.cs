using Avalonia.Controls;
using Avalonia.Metadata;
using project_chess_DB.ViewModels;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Linq;
using project_chess_DB.Models;
using project_chess_DB.Services;

namespace project_chess_DB.Views;

public partial class PlayerPageView : UserControl
{
    private readonly PlayerRepository repo = new PlayerRepository();

    public PlayerPageView()
    {
        InitializeComponent();
        var txtAge = this.FindControl<TextBox>("TxtAge");
        var txtElo = this.FindControl<TextBox>("TxtElo");
        var txtPhonenumber = this.FindControl<TextBox>("TxtPhonenumber");
        var txtLastname = this.FindControl<TextBox>("TxtLastname");
        var txtFirstname = this.FindControl<TextBox>("TxtFirstname");
        var txtCountry = this.FindControl<TextBox>("TxtCountry");
        txtAge?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
        txtElo?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
        txtPhonenumber?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
        txtLastname?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
        txtFirstname?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
        txtCountry?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);

    }
    private void DataGrid_PreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
    {
        if (e.EditingElement is TextBox textBox)
        {
            var header = e.Column.Header?.ToString();

            if (header == "Age" || header == "Elo")
            {
                textBox.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
            }
            else if (header == "Last Name" || header == "First Name" || header == "Country")
            {
                textBox.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
            }
        }
    }

    private void DataGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        if (e.Row.DataContext is Player editedPlayer)
        {
            // Mise à jour directe dans la DB
            repo.UpdatePlayer(editedPlayer);
        }

    }
    private void Numeric_TextInput(object? sender, TextInputEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Text) && !e.Text.All(char.IsDigit))
        {
            e.Handled = true;
        }
    }
    private void Letters_TextInput(object? sender, TextInputEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Text))
        {
            bool isValid = e.Text.All(c => char.IsLetter(c) || c == ' ' || c == '-');

            if (!isValid)
            {
                e.Handled = true;
            }
        }
    }
}