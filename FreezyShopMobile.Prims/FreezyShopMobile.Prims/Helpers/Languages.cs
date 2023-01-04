using FreezyShopMobile.Prims.Interface;
using FreezyShopMobile.Prims.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FreezyShopMobile.Prims.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Accept => Resource.Accept;

        public static string ConnectionError => Resource.ConnectionError;

        public static string Error => Resource.Error;

        public static string AddToCart => Resource.AddToCart;

        public static string Name => Resource.Name;

        public static string Description => Resource.Description;

        public static string GettingInformation => Resource.GettingInformation;

        public static string Price => Resource.Price;
        public static string SearchProduct => Resource.SearchProduct;

        public static string Products => Resource.Products;
        public static string Stock => Resource.Stock;
        public static string Login => Resource.Login;

    }
}


