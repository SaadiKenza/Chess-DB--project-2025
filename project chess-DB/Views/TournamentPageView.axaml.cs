using Avalonia.Controls;
using project_chess_DB.ViewModels;

namespace project_chess_DB.Views;

public partial class TournamentPageView : UserControl
{
    public TournamentPageView()
    {
        InitializeComponent();
        DataContext = new TournamentPageViewModel();
    }
}