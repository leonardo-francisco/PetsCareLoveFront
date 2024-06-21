﻿namespace PetsCareLove.Web.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Photo { get; set; }

        public Guid CategoryId { get; set; }
        public Guid PetId { get; set; }
    }
}
