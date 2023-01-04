using FreezyShop.Data.Entities;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class FavouritesViewModel : ClientFavourite
    {
        public IEnumerable<ClientFavourite> Favourites { get; set; }
       
     
    }
}
