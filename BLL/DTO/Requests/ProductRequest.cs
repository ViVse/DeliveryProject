﻿namespace BLL.DTO.Requests
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public int ProductionTime { get; set; }
        public int ShopId { get; set; }
    }
}
