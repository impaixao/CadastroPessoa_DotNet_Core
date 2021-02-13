using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiTeste.Data;
using apiTeste.Models;

namespace apiTeste.Controllers
{
    [ApiController]
    [Route("v1/products")]

    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] Contexto contexto)
        {
            var products = await contexto.Products
            .Include(x=>x.Category)
            .AsNoTracking()
            .ToListAsync();
            
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] Contexto contexto, int id)
        {
            var product = await contexto.Products.Include(x=>x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == id);
            return product;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] Contexto contexto, int id)
        {
            var products = await contexto.Products
            .Include(x=>x.Category)
            .Where(x=>x.CategoryId == id)
            .AsNoTracking()
            .ToListAsync();

            if (!products.Any())
            {
                return NotFound();
            }

            return products;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromServices] Contexto contexto, [FromBody] Product model)
        {
            if (ModelState.IsValid)
            {               
                if (!contexto.Categories.Any(x=>x.Id == model.CategoryId))
                {
                    return NotFound("Categoria não existe. Verifique!");
                }

                contexto.Products.Add(model);
                await contexto.SaveChangesAsync();

                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> Put([FromServices] Contexto contexto, [FromBody] Product model, int id)
        {
            var product = await contexto.Products.FirstOrDefaultAsync(x=>x.Id == id);

            if (product == null)
                return BadRequest("Produto não existe.");

            if (ModelState.IsValid)
            {                
                product.Title = model.Title;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                await contexto.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Nenhum produto enviado para atualizar.");
            }

            return model;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> Delete([FromServices] Contexto contexto, int? id)
        {
            if (id != null)
            {
                var product = await contexto.Products.FirstOrDefaultAsync(x=>x.Id == id);
                contexto.Products.Remove(product);
                await contexto.SaveChangesAsync();

                return StatusCode(200, "Registro excluido com sucesso!");
            }
            else
            {
                return NotFound("Produto não localizado.");
            }
        }

    }
}