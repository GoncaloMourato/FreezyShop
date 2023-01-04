using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace FreezyShopMobile.Prims.ViewModels
{
    
    public class AboutPageViewModel : ViewModelBase
    {
        
        public AboutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Contate-Nos/Sobre";



        }

    }


}
