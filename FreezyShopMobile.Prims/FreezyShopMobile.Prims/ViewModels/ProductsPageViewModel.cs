using FreezyShopMobile.Prims.Helpers;
using FreezyShopMobile.Prims.ItemViewModels;
using FreezyShopMobile.Prims.Models;
using FreezyShopMobile.Prims.Sirvices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FreezyShopMobile.Prims.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ObservableCollection<ProductItemViewModel> _products;
        private bool _isRunning;
        private string _search;
        private List<ProductResponse> _searchlist;
        private DelegateCommand _searchCommand;

        public ProductsPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            Title = "Freezy";

            LoadProductsAsync();
        }


        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowProducts));

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowProducts();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private async void LoadProductsAsync()
        {

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert(Languages.Error, 
                        Languages.ConnectionError, Languages.Accept);
                });

                return;
            }

            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            Response response = await _apiService.GetListAsync<ProductResponse>(url, "/api", "/productsapi");

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _searchlist = (List<ProductResponse>)response.Result;
            ShowProducts();
        }

        private void ShowProducts()
        {
           

            if(string.IsNullOrEmpty(Search))
            {
                Products = new ObservableCollection<ProductItemViewModel>(_searchlist.Select(p=>
                new ProductItemViewModel(_navigationService)
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl1 = p.ImageUrl1,
                    ImageUrl2 = p.ImageUrl2,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    User = p.User,
                    Accessed = p.Accessed,
                    ImageFullPath1 = p.ImageFullPath1,
                    ImageFullPath2 = p.ImageFullPath2
                }).ToList());

            }
            else
            {
                Products = new ObservableCollection<ProductItemViewModel>(_searchlist.Select
                    (p=>
                    new ProductItemViewModel(_navigationService)
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl1 = p.ImageUrl1,
                        ImageUrl2 = p.ImageUrl2,
                        Quantity = p.Quantity,
                        Description = p.Description,
                        User = p.User,
                        Accessed= p.Accessed,
                        ImageFullPath1 = p.ImageFullPath1,
                        ImageFullPath2 = p.ImageFullPath2
                    })
                    .Where(p => p.Name.ToLower().Contains(
                        Search.ToLower())).ToList());
            }
        }
    }
}
