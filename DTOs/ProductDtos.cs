using System.ComponentModel.DataAnnotations;

namespace MultiTenantSaas.DTOs
{
    public class CreateProductRequest
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }

    public class UpdateProductRequest : CreateProductRequest
    {
    }

    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
