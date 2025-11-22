using Avalonia.Controls;
using project_chess_DB.ViewModels;

namespace project_chess_DB.Views;

public partial class PlayerPageView : UserControl
{
    public PlayerPageView()
    {
        InitializeComponent();
        DataContext = new PlayerPageViewModel();
    }
}