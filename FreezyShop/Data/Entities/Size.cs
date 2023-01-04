namespace FreezyShop.Data.Entities
{
    public class Size : IEntity
    {
        public int Id { get; set; }
        public int Hip { get; set; } //woman 
        public int WaistWoman { get; set; }
        public int WaistMan { get; set; }

        public int BreastWoman { get; set; } 

        public int BreastMan{ get; set; }

        public string ResultSize { get; set; }

      

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
