namespace FreezyShop.Data.Entities
{
    public class PromoCode : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public int Percentagem { get; set; }
    }
}
