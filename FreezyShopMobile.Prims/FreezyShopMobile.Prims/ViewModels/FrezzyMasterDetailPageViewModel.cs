using FreezyShopMobile.Prims.Helpers;
using FreezyShopMobile.Prims.ItemViewModels;
using FreezyShopMobile.Prims.Models;
using FreezyShopMobile.Prims.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FreezyShopMobile.Prims.ViewModels
{
    public class FrezzyMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public FrezzyMasterDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_ficon",
                    PageName = $"{nameof(ProductsPage)}",
                    Title = "Produtos Freezy"
                },
                new Menu
                {
                    Icon = "ic_ficon",
                    PageName = $"{nameof(AboutPage)}",
                    Title = "Contate-Nos/Sobre"
                },
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    Title = m.Title,
                    PageName = m.PageName
                }).ToList());
        }
    }
}
