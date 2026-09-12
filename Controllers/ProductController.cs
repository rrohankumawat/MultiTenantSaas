using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantSaas.Data;
using MultiTenantSaas.DTOs;
using MultiTenantSaas.Models;
using MultiTenantSaas.Multitenancy;

namespace MultiTenantSaas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext _context, ITenantProvider tenantProvider) : ControllerBase
    {
        [Authorize]
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var data = await _context.Products.ToListAsync();
            return Ok(data);
        }

        [Authorize]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductRequest dto)
        {
            if(tenantProvider.TenantId is null)
            {
                return BadRequest("Tenant id is null");
            }

            Product product = new();
            product.TenantId = tenantProvider.TenantId.Value;
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok("Product Created Successfully");

        }
    }
}
