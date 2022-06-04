namespace DAL.Entities
{
    public class Review: BaseEntity
    {
        public string Text { get; set; }
        public int Stars { get; set; }
        public DateTime Date { get; set; }
        public int ShopId { get; set; }
        public int CustomerId { get; set; }
        public Shop Shop { get; set; }
        public User Customer { get; set; }
    }
}
