using Avalonia.Controls;
using project_chess_DB.Models;
using project_chess_DB.Services;
using project_chess_DB.ViewModels;

namespace project_chess_DB.Views;

public partial class TournamentPageView : UserControl
{
    private readonly TournamentRepository repo = new TournamentRepository();

    public TournamentPageView()
    {
        InitializeComponent();
       
    }

    private void TournamentGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs t)
    {
        if (t.Row.DataContext is Tournament editedTournament)
        {
            repo.UpdateTournament(editedTournament);
        }
    }
}