using FreezyShopMobile.Prims.Helpers;
using FreezyShopMobile.Prims.Models;
using FreezyShopMobile.Prims.Sirvices;
using FreezyShopMobile.Prims.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FreezyShopMobile.Prims.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private bool _isEnabled;
        private bool _isRunning;
        private DelegateCommand _loginCommand;
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private List<UserResponse> _users;

        public LoginPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            IsEnabled = true;
            Title = "Entrar";

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error,
                        Languages.ConnectionError, Languages.Accept);
                });

                return;
            }


        }


        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));


        public string Email { get; set; }

        public string Password { get; set; }


        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public List<UserResponse> Users
        {
                get => _users;
                set => SetProperty(ref _users, value);
        }

        

        public async void Login()
        {
            int i = 0;
            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.GetListAsync<UserResponse>(url, "/api", "/userapi");


            if (!response.IsSuccess)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            List<UserResponse> users = (List<UserResponse>)response.Result;


            //if(users == null)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", "Tou vazio", "Ok");
            //}
            //else
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", response.Result.ToString(), "Ok");
            //}

            if (string.IsNullOrEmpty(Email))
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Erro", "Verifica o Email", "Ok");
                return;

            }

            if (string.IsNullOrEmpty(Password))
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Erro", "Verifica a Palavra Passe", "Ok");
                return;

            }

            

            foreach (var email in users)
            {
                if (email.Email.Equals(Email) && email.Password.Equals(Password))
                {
                    IsRunning = false;
                    await App.Current.MainPage.DisplayAlert("Bem Vindo:", email.FirstName + " " + email.LastName , "Ok");
                    //await App.Current.MainPage.Navigation.PushAsync(new ProductsPage());
                    await NavigationService.NavigateAsync($"/{nameof(FrezzyMasterDetailPage)}/NavigationPage/{nameof(ProductsPage)}");
                }
                else
                {
                    i++;
                }
                if(i >= users.Count)
                {
                    await App.Current.MainPage.DisplayAlert("Algo Correu Mal", "Email ou Palavra-Passe incorretas", "Ok");
                    IsRunning = false;
                    i = 0;
                    return;
                }
            }
        }
    }
}
