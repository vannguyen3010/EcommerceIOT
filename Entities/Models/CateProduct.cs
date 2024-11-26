namespace Entities.Models
{
    public class CateProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameSlug { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public ICollection<CateProduct> CategoriesObjs { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
