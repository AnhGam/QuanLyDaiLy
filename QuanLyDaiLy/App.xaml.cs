using QuanLyDaiLy.Data;
using QuanLyDaiLy.Views.AgencyList;

namespace QuanLyDaiLy
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Resolve từ DI
            var agencyPage = _serviceProvider.GetRequiredService<Agency>();
            return new Window(new NavigationPage(agencyPage));
        }
    }
}
