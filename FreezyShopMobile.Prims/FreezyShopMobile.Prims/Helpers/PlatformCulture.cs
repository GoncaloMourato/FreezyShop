using System;
using System.Collections.Generic;
using System.Text;

namespace FreezyShopMobile.Prims.Helpers
{
    public class PlatformCulture
    {
        public string PlatformString { get; private set; }

        public string LaguageCode { get; private set; }

        public string LocateCode { get; private set; }

        public PlatformCulture(string platformCultureString)
        {
            if(string.IsNullOrEmpty(platformCultureString))
            {
                throw new ArgumentNullException("Expected culture identifier",
                    "platformCultureString");
            }

            PlatformString = platformCultureString.Replace("_", "-");
            int dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if(dashIndex < 0)
            {
                string[] parts = PlatformString.Split('-');
                LaguageCode = parts[0];
                LocateCode = parts[1];
            }
            else
            {
                LaguageCode = PlatformString;
                LocateCode = "";
            }
        }

        public override string ToString()
        {
            return PlatformString;
        }
    }
}
