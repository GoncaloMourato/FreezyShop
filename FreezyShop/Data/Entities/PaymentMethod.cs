using System;

namespace FreezyShop.Data.Entities
{
    public class PaymentMethod : IEntity
    {
        public int Id { get; set; }
        public string NameOnCard { get; set; }

        public string CardNumber { get; set; }

        public string ExperyDate { get; set; }

        public int CVV { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
