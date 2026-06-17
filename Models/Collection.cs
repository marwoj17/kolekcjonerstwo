using System.Collections.Generic;

namespace CollectionSystem.Models
{
    public class Collection
    {
        // Klasa reprezentująca pojedynczą kolekcję
        // Zawiera nazwę kolekcji oraz listę elementów tej kolekcji
        public string Name { get; set; }
        // Lista elementów w kolekcji. Domyślnie inicjalizowana jako pusta lista.
        public List<CollectionItem> Items { get; set; } = new List<CollectionItem>();
    }
}
