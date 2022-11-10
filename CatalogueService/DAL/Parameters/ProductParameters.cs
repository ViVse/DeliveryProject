namespace DAL.Parameters
{
    public class ProductParameters: PaginationParameters
    {
        public int MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; }

        public int MinProductionTime { get; set; } = 0;
        public int? MaxProductionTime { get; set; }

        public string Name { get; set; } = "";

        public int? ShopId { get; set; }

        public bool ValidPriceRange => MaxPrice == null || MinPrice <= MaxPrice;
        public bool ValidProductionTimeRange => MaxProductionTime == null || MinProductionTime <= MaxProductionTime;
    }
}
