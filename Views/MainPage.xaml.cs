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
            InitializeComponent();
            collections = FileService.Load();

            CollectionsList.ItemsSource = collections;
        }

        private async void AddCollection(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Nowa kolekcja", "Podaj nazwę:");

            if (!string.IsNullOrEmpty(name))
            {
                collections.Add(new Collection { Name = name });

                CollectionsList.ItemsSource = null;
                CollectionsList.ItemsSource = collections;

                FileService.Save(collections);
            }
        }

        private async void OpenCollectionButton(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selected = button?.BindingContext as Collection;
            if (selected == null) return;

            await Shell.Current.Navigation.PushAsync(new Views.CollectionPage(selected, collections));
        }

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

            FileService.Save(collections);
        }

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

            FileService.Save(collections);
        }
    }
}

