using System;
using Xamarin.Forms;

namespace FreezyShopMobile.Prims.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
           
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Atenção", "Queres mesmo sair da aplicação?", "Sim", "Não").ConfigureAwait(false);
                if (result)
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            });
            return true;
        }

    }
}
