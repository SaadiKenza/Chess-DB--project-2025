using Avalonia.Controls;
using Avalonia.Metadata;
using project_chess_DB.ViewModels;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Linq;
using project_chess_DB.Models;
using project_chess_DB.Services;

namespace project_chess_DB.Views;

public partial class TournamentPageView : UserControl
{
    private readonly TournamentRepository repo = new TournamentRepository();

    public TournamentPageView()
    {
        InitializeComponent();
        var txtName_of_the_tournament = this.FindControl<TextBox>("TxtName_of_the_tournament");
        var txtCountry = this.FindControl<TextBox>("TxtCountry");
        var txtCity = this.FindControl<TextBox>("TxtCity");
        var txtCompetition_number = this.FindControl<TextBox>("TxtCompetition_number");
        var txtRegistration_number1 = this.FindControl<TextBox>("TxtRegistration_number1");
        var txtMoves1 = this.FindControl<TextBox>("TxtMoves1");
        var txtRegistration_number2 = this.FindControl<TextBox>("TxtRegistration_number2");
        var txtMoves2 = this.FindControl<TextBox>("TxtMoves2");
        txtName_of_the_tournament?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
        txtCountry?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
        txtCity?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
        txtCompetition_number?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
        txtRegistration_number1?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
        txtMoves1?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
        txtRegistration_number2?.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
        txtMoves2?.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
    }
    private void DataGrid_PreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
    {
        if (e.EditingElement is TextBox textBox)
        {
            var header = e.Column.Header?.ToString();

            if (header == "No." || header == "P1 Registration" || header == "P2 Registration")
            {
                textBox.AddHandler(TextInputEvent, Numeric_TextInput, RoutingStrategies.Tunnel);
            }
            else if (header == "Name of the tournament" || header == "Country" || header == "City" || header == "P1 Moves" || header == "P2 Moves")
            {
                textBox.AddHandler(TextInputEvent, Letters_TextInput, RoutingStrategies.Tunnel);
            }
        }
    }

    private void TournamentGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs t)
    {
        if (t.Row.DataContext is Tournament editedTournament)
        {
            repo.UpdateTournament(editedTournament);
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