using Lab11.Data.Context;
using Lab11.Data.Models;
using Lab11.Dtos.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EComAppDbContext _context;

        public ProductController(EComAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetAll()
        {
            var products = _context.Products.Include(p => p.Category)
                .Select(p => new ProductReadDto()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Count = p.Count,
                    ExpiryDate = p.ExpiryDate,
                    ImageURL = p.ImageUrl,
                    Category = p.Category.Name
                }).ToList();
            return Ok(products);

        }
        [HttpGet("{id}")]
        public ActionResult<ProductReadDto> GetById(int id)
        {

            var products = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (products == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found" });
            }
            var productReadDto = new ProductReadDto()
                {
                    Id = products.Id,
                    Title = products.Title,
                    Description = products.Description,
                    Price = products.Price,
                    Count = products.Count,
                    ExpiryDate = products.ExpiryDate,
                    ImageURL = products.ImageUrl,
                    Category = products.Category.Name
                };
            return Ok(productReadDto);

        }
        [HttpGet("search")]
        public ActionResult<ProductReadDto> GetByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Search title cannot be empty.");
            }
            var products = _context.Products.Include(p => p.Category).Where(p => p.Title == title)
                .Select(p => new ProductReadDto()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    Count = p.Count,
                    ExpiryDate = p.ExpiryDate,
                    ImageURL = p.ImageUrl,
                    Category = p.Category.Name
                }).ToList();
            return Ok(products);

        }

        [HttpPost]
        [Route("Create")]
        public ActionResult<ProductReadDto> Create(ProductCreateDto ProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Product = new Product()
            {
                Title = ProductDto.Title,
                Description = ProductDto.Description,
                Price = ProductDto.Price,
                Count = ProductDto.Count,
                CategoryId = ProductDto.CategoryId
            };

            _context.Products.Add(Product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById),new { id =Product.Id } , Product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult<ProductReadDto> Update(int id , ProductEditDto ProductDto)
        {
            if (id != ProductDto.Id)
            {
                return NotFound(new { Message = $"Product with id {id} does not match Product id in body {ProductDto.Id}" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (products == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found" });
            }



            products.Title = ProductDto.Title;
            products.Description = ProductDto.Description;
            products.Price = ProductDto.Price;
            products.Count = ProductDto.Count;
            products.CategoryId = ProductDto.CategoryId;
            
            _context.SaveChanges();

            var resultDto = new ProductReadDto
            {
                Id = products.Id,
                Title = products.Title,
                Description = products.Description,
                Price = products.Price,
                Count = products.Count,
                ExpiryDate = products.ExpiryDate,
                ImageURL = products.ImageUrl,
                Category = products.Category?.Name ?? "Unknown"
            };

            return Ok(resultDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {

            var products = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (products == null)
            {
                return NotFound(new { Message = $"Product with id {id} not found" });
            }
            _context.Products.Remove(products);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
