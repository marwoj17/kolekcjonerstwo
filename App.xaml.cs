using System.Diagnostics;
using CollectionSystem.Services;

namespace CollectionSystem
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
#if DEBUG
            Debug.WriteLine($"Data path: {FileService.GetPath()}");
#endif
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
