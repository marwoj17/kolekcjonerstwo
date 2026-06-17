using System;
using System.Collections.Generic;
using CollectionSystem.Models;
using CollectionSystem.Services;

namespace CollectionSystem
{
    public partial class MainPage : ContentPage
    {
        private List<Collection> collections = new List<Collection>();

        public MainPage()
        {
            // Konstruktor strony głównej
            // Inicjalizujemy komponenty XAML, ładujemy zapisane kolekcje z pliku
            InitializeComponent();
            collections = FileService.Load();

            // Ustawiamy źródło danych dla kontrolki CollectionView
            CollectionsList.ItemsSource = collections;
        }

        private async void AddCollection(object sender, EventArgs e)
        {
            // Dodawanie nowej kolekcji - proste okno dialogowe z polem tekstowym
            string name = await DisplayPromptAsync("Nowa kolekcja", "Podaj nazwę:");

            if (!string.IsNullOrEmpty(name))
            {
                // Tworzymy nowy obiekt Collection i dodajemy do listy
                collections.Add(new Collection { Name = name });

                // Odświeżamy widok (prostą metodą: przypisanie ItemsSource na nowo)
                CollectionsList.ItemsSource = null;
                CollectionsList.ItemsSource = collections;

                // Zapisujemy zmiany do pliku
                FileService.Save(collections);
            }
        }

        // Handler dla przycisku "Otwórz" przy konkretnej kolekcji
        // Pobiera powiązany obiekt Collection z BindingContext przycisku i otwiera stronę szczegółów
        private async void OpenCollectionButton(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selected = button?.BindingContext as Collection;
            if (selected == null) return;

            await Shell.Current.Navigation.PushAsync(new Views.CollectionPage(selected, collections));
        }

        // Edycja nazwy kolekcji bez wchodzenia w jej szczegóły
        // Używamy DisplayPromptAsync do szybkiej zmiany nazwy
        private async void EditCollectionButton(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selected = button?.BindingContext as Collection;
            if (selected == null) return;

            string newName = await DisplayPromptAsync("Edycja kolekcji", "Nowa nazwa:", initialValue: selected.Name);
            if (string.IsNullOrEmpty(newName)) return;

            selected.Name = newName;
            CollectionsList.ItemsSource = null;
            CollectionsList.ItemsSource = collections;

            // Zapisujemy zmienioną listę kolekcji
            FileService.Save(collections);
        }

        // Usuwanie kolekcji z listy (z potwierdzeniem)
        private async void DeleteCollectionButton(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selected = button?.BindingContext as Collection;
            if (selected == null) return;

            bool ok = await DisplayAlert("Usuń kolekcję", $"Czy na pewno usunąć '{selected.Name}'?", "Tak", "Nie");
            if (!ok) return;

            collections.Remove(selected);
            CollectionsList.ItemsSource = null;
            CollectionsList.ItemsSource = collections;

            // Zapis po usunięciu
            FileService.Save(collections);
        }
    }
}

