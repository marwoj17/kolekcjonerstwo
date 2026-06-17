using CollectionSystem.Models;
using CollectionSystem.Services;

namespace CollectionSystem.Views;
        
public partial class CollectionPage : ContentPage
{
    private Collection collection;
    private List<Collection> allCollections;

    public CollectionPage(Collection col, List<Collection> all)
    {

        InitializeComponent();

        collection = col;
        allCollections = all;

        ItemsList.ItemsSource = collection.Items;
    }

    private async void AddItem(object sender, EventArgs e)
    {
        string title = await DisplayPromptAsync("Nowy element", "Tytuł:");
        if (string.IsNullOrEmpty(title)) return;

        string desc = await DisplayPromptAsync("Opis", "Opis:");

        collection.Items.Add(new CollectionItem
        {
            Title = title,
            Description = desc
        });

        RefreshList();
    }

    private async void EditItem(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.BindingContext as CollectionItem;

        if (item == null) return;
        string newTitle = await DisplayPromptAsync("Edycja", "Nowy tytuł:", initialValue: item.Title);
        if (string.IsNullOrEmpty(newTitle)) return;

        string newDesc = await DisplayPromptAsync("Edycja", "Nowy opis:", initialValue: item.Description);

        item.Title = newTitle;
        item.Description = newDesc;

        RefreshList();
    }

    private void DeleteItem(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.BindingContext as CollectionItem;

        if (item == null) return;
        collection.Items.Remove(item);

        RefreshList();
    }

    private void RefreshList()
    {
        ItemsList.ItemsSource = null;
        ItemsList.ItemsSource = collection.Items;
        FileService.Save(allCollections);
    }
}
