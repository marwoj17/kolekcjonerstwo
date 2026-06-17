using CollectionSystem.Models;
using CollectionSystem.Services;

namespace CollectionSystem.Views;
        
public partial class CollectionPage : ContentPage
{
    private Collection collection;
    private List<Collection> allCollections;

    public CollectionPage(Collection col, List<Collection> all)
    {
        // Konstruktor strony szczegółów kolekcji
        // Otrzymujemy referencję do pojedynczej kolekcji oraz listy wszystkich kolekcji
        InitializeComponent();

        collection = col;
        allCollections = all;

        // Ustawiamy źródło danych dla listy elementów w tej kolekcji
        ItemsList.ItemsSource = collection.Items;
    }

    private async void AddItem(object sender, EventArgs e)
    {
        // Dodawanie nowego elementu do wybranej kolekcji
        string title = await DisplayPromptAsync("Nowy element", "Tytuł:");
        if (string.IsNullOrEmpty(title)) return;

        string desc = await DisplayPromptAsync("Opis", "Opis:");

        collection.Items.Add(new CollectionItem
        {
            Title = title,
            Description = desc
        });

        // Odświeżamy widok i zapisujemy zmiany
        RefreshList();
    }

    private async void EditItem(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.BindingContext as CollectionItem;

        if (item == null) return;
        // Edycja pojedynczego elementu kolekcji
        string newTitle = await DisplayPromptAsync("Edycja", "Nowy tytuł:", initialValue: item.Title);
        if (string.IsNullOrEmpty(newTitle)) return;

        string newDesc = await DisplayPromptAsync("Edycja", "Nowy opis:", initialValue: item.Description);

        item.Title = newTitle;
        item.Description = newDesc;

        // Zapis i odświeżenie listy
        RefreshList();
    }

    private void DeleteItem(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.BindingContext as CollectionItem;

        if (item == null) return;
        // Usuwamy pojedynczy element z kolekcji
        collection.Items.Remove(item);

        // Zapis / odświeżenie
        RefreshList();
    }

    private void RefreshList()
    {
        ItemsList.ItemsSource = null;
        ItemsList.ItemsSource = collection.Items;
        // Po każdej zmianie zapisujemy całą listę kolekcji do pliku
        FileService.Save(allCollections);
    }
}