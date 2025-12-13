using Avalonia.Controls;
using Avalonia.Metadata;
using project_chess_DB.ViewModels;
using project_chess_DB.Models;
using project_chess_DB.Services;

namespace project_chess_DB.Views;

public partial class PlayerPageView : UserControl
{
     private readonly PlayerRepository repo = new PlayerRepository();

    public PlayerPageView()
    {
        
        InitializeComponent();
       
    }

    private void DataGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        if (e.Row.DataContext is Player editedPlayer)
        {
            // Mise à jour directe dans la DB
            repo.UpdatePlayer(editedPlayer);
        }

    }
}