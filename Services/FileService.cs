using CollectionSystem.Models;
using System.Collections.Generic;

namespace CollectionSystem.Services
{
    public static class FileService
    {
        // Pełna ścieżka do pliku tekstowego, w którym przechowujemy wszystkie kolekcje.
        // Plik znajduje się w katalogu aplikacji (AppDataDirectory).
        private static string filePath = Path.Combine(
            FileSystem.AppDataDirectory, "collections.txt");

        public static void Save(List<Collection> collections)
        {
            List<string> lines = new List<string>();

            foreach (var col in collections)
            {
                // Oznaczamy początek nowej kolekcji linią zaczynającą się od #COLLECTION:
                // następne linie z ITEM: zawierają elementy kolekcji.
                lines.Add($"#COLLECTION:{col.Name}");

                foreach (var item in col.Items)
                {
                    // Element zapisujemy jako "ITEM:Tytuł|Opis". Separator '|' oddziela tytuł od opisu.
                    lines.Add($"ITEM:{item.Title}|{item.Description}");
                }
            }

            File.WriteAllLines(filePath, lines);
        }

        public static List<Collection> Load()
        {
            List<Collection> collections = new List<Collection>();

            if (!File.Exists(filePath))
                return collections;

            var lines = File.ReadAllLines(filePath);

            Collection current = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("#COLLECTION:"))
                {
                    // Nowa kolekcja - tworzymy obiekt i dodajemy do listy
                    current = new Collection
                    {
                        Name = line.Replace("#COLLECTION:", "")
                    };
                    collections.Add(current);
                }
                else if (line.StartsWith("ITEM:") && current != null)
                {
                    // Parsujemy linię ITEM:Tytuł|Opis
                    var data = line.Replace("ITEM:", "").Split('|');

                    current.Items.Add(new CollectionItem
                    {
                        Title = data[0],
                        Description = data.Length > 1 ? data[1] : ""
                    });
                }
            }

            return collections;
        }

        public static string GetPath()
        {
            // Zwracamy ścieżkę do pliku, używane w debugowaniu / informowaniu użytkownika
            return filePath;
        }
    }
}
