using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Product
{
    public class CreateProductDto
    {
        [Required]
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public IFormFile ImageFile { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public bool IsHot { get; set; }

    }
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameSlug { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageFilePath { get; set; }
        public bool IsHot { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CateProductDto ParentCategory { get; set; }
    }
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        [Range(1, 5)]
        public int Rating { get; set; }
        public bool IsHot { get; set; }

        public IFormFile? File { get; set; }
    }

}
