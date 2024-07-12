﻿namespace GeekShopping.CartAPI.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
