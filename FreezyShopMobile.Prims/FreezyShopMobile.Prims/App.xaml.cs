using FreezyShopMobile.Prims.Sirvices;
using FreezyShopMobile.Prims.ViewModels;
using FreezyShopMobile.Prims.Views;
using Prism;
using Prism.Ioc;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace FreezyShopMobile.Prims
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("NzcxOTU4QDMyMzAyZTMzMmUzMEFNb1NOZVpYYVJSaDhQTE52YWpZc3RoTkNtWHFSNldIc1hlMkw4R0NHelk9");

            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
            
        }



        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductDetailPage, ProductDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<FrezzyMasterDetailPage, FrezzyMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
        }
    }
}
