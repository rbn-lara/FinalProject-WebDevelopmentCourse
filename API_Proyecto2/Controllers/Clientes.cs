using API_Proyecto2.Data;
using API_Proyecto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Proyecto2.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class Clientes : ControllerBase
   {
      private readonly DbContextChild _context;

      public Clientes(DbContextChild context)
      {
         _context = context;
      }

      [HttpGet]
      public async Task<ActionResult<IEnumerable<Client>>> GetClients()
      {
         return await _context.Clients.ToListAsync();
      }

      [HttpGet("{id}")]
      public async Task<ActionResult<Client>> GetClient(string id)
      {
         var client = await _context.Clients.FindAsync(id);

         if (client == null)
         {
            return NotFound();
         }

         return client;
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> PutClient(string id, Client client)
      {
         if (id != client.ClientId)
         {
            return BadRequest();
         }

         _context.Entry(client).State = EntityState.Modified;

         try
         {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!ClientExists(id))
            {
               return NotFound();
            }
            else
            {
               throw;
            }
         }

         return NoContent();
      }

      [HttpPost]
      public async Task<ActionResult<Client>> PostClient(Client client)
      {

         _context.Clients.Add(client);

         try
         {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateException)
         {
            if (ClientExists(client.ClientId))
            {
               return Conflict();
            }
            else
            {
               throw;
            }
         }

         return CreatedAtAction("GetClient", new { id = client.ClientId }, client);
      }

      //Verification

      [HttpGet]
      [Route("Initialized")]
      public bool HasInitialized()
      {
         return _context.Clients.Any(e => e.ClientId == "01-1111-1111");
      }

      [HttpGet]
      [Route("Exists/{id}")]
      public bool ClientExists(string id)
      {
         return _context.Clients.Any(e => e.ClientId == id);
      }

      //Metodo range utilizado para cargar objetos predeterminados por requerimiento de instrucciones

      [HttpPost("Range")]
      public async Task<ActionResult> PostClientRange(List<Client> clientes)
      {
         _context.Clients.AddRange(clientes);
            try
         {
            await _context.SaveChangesAsync();

         }
         catch (DbUpdateException)
         {
            throw;
         }

         return StatusCode(201);
      }
   }
}
