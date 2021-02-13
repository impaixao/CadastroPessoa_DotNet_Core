using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiTeste.Data;
using apiTeste.Models;

namespace apiTeste.Controllers
{
    [ApiController]
    [Route("/v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] Contexto contexto)
        {
            var categories = await contexto.Categories.ToListAsync();
            return categories;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post([FromServices] Contexto contexto, [FromBody] Category model)
        {
            if (ModelState.IsValid)
            {
                contexto.Categories.Add(model);
                await contexto.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}