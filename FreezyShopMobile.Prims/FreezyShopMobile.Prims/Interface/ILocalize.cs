using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FreezyShopMobile.Prims.Interface
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo ci);
    }
}
