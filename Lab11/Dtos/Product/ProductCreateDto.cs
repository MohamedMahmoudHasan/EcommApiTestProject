using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lab11.Dtos.Product
{
    public class ProductCreateDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int Count { get; set; }
        public int CategoryId { get; set; }
    }
}
