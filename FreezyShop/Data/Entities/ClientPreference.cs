namespace FreezyShop.Data.Entities
{
    public class ClientPreference : IEntity
    {
        public int Id { get; set; }

        public string Preferences { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
