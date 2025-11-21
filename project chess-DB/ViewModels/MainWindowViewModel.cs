using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace project_chess_DB.ViewModels;

//quand j'ai des {} c'est que j'ai un ensemble d'instructions à réaliser ensemble, si y a que une instruction peut ne pas le mettre
// déclaration pour créer une variable, propriété, constante (;) : stocker en mémoire
//bloc pour exécuter un ensemble d'instructions ({}) : exécuter
// pas d'ordre spécial

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] /*propriété qui permet de générer automatiquement des proppriété publique, des événement PropertyChanged*/
    private bool _isPaneOpen = true; /*stocke si le menu est ouvert*/

    [ObservableProperty] // c'est une déclaration de champs privé (fini par ; pour signaler la fin de la déclaration) pas un if, une boucle ou méthode,du coup pas de {}
    private ViewModelBase _CurrentPage = new DashboardPageViewModel(); /*la page active dans l'UI*/

    [ObservableProperty]
    private ListItemTemplate? _SelectedListItem; /*l'item du menu que je sélectionne*/
    partial void OnSelectedListItemChanged(ListItemTemplate? value) /*changement automatique de page*/
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType); //active la page sélectionner
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance; //affiche la nouvelle page
    }
    public ObservableCollection<ListItemTemplate> Items { get; } = new() //les items du menu
    {
        new ListItemTemplate(typeof(DashboardPageViewModel),"home_regular"), //nom et icon dans le menu hamburger
        new ListItemTemplate(typeof(PlayerPageViewModel),"calendar_person_regular"),
        new ListItemTemplate(typeof(TournamentPageViewModel),"book_database_regular"),
    };
    [RelayCommand] //gère les boutons
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen; //gère l'ouverture et la fermeture de menu hamburger

    }
}

public class ListItemTemplate //objet qui représente une entrée du menu (contient : ViewModel à ouvrir, son nom (le texte) et l'icône)
{
    public ListItemTemplate(Type type, string iconKey)
    {
        ModelType = type;
        Label = type.Name.Replace("PageViewModel", ""); //texte affcihé 
        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!; //icône du meneu
    }
    public string Label { get; }
    public Type ModelType { get; } //la page à ouvrir
    public StreamGeometry ListItemIcon { get; }
}
