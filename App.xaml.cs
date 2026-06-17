using System.Diagnostics;
using CollectionSystem.Services;

namespace CollectionSystem
{
    public partial class App : Application
    {
        public App()
        {
            // Inicjalizacja komponentów aplikacji
            InitializeComponent();

#if DEBUG
            // W trybie debug wypisujemy ścieżkę do pliku z danymi
            // Przydatne podczas testów i sprawdzania, gdzie zapisuje się collections.txt
            Debug.WriteLine($"Data path: {FileService.GetPath()}");
#endif
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}