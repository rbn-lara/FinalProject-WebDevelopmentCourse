using API_Proyecto2.Data;
using API_Proyecto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Proyecto2.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class proyectos_constru : ControllerBase
   {
      private readonly DbContextChild _context;

      public proyectos_constru(DbContextChild context)
      {
         _context = context;
      }

      /*
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
      {
         return await _context.Projects.ToListAsync();
      }
      */

      //PROJECTS

      [HttpGet("{id}")]
      public ActionResult<Project> GetProject(string id)
      {
         return _context.Projects.Find(int.Parse(id));
      }

      [HttpGet("cliente/{id}")]
      public ActionResult<IEnumerable<Project>> GetProjectsOf(string id)
      {
         var project = _context.Projects.Where(p => p.ClientId == id);

         if (project == null)
         {
            return NotFound();
         }

         return project.ToList();
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> PutProject(int id, Project project)
      {
         if (id != project.ProjectId)
         {
            return BadRequest();
         }

         _context.Entry(project).State = EntityState.Modified;

         try
         {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!ProjectExists(id))
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
      public async Task<ActionResult<Project>> PostProject(Project project)
      {
         _context.Projects.Add(project);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
      }

      [HttpGet]
      [Route("Initialized")]
      public bool HasInitialized()
      {
         return _context.Projects.Any();
      }

      private bool ProjectExists(int id)
      {
         return _context.Projects.Any(e => e.ProjectId == id);
      }


      //IMAGES

      [HttpGet("e/GetImages")]
      public ActionResult<List<ProjectImage>> GetImages([FromQuery] int[] idList)
      {
         List<ProjectImage> imagenes = new List<ProjectImage>();
         foreach(var i in idList)
         {
            imagenes.AddRange(_context.Images.Where(e => e.ProjectId == i).ToList());
         }
         return imagenes;
      }


      [HttpPut("img/{id}")]
      public async Task<ActionResult<Project>> PutImage(int id, ProjectImage projectImage)
      {
         _context.Entry(projectImage).State = EntityState.Modified;
         try
         {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!ProjectImageExists(id))
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


      [HttpPost("img")]
      public async Task<ActionResult<Project>> PostImage(ProjectImage projectImage)
      {
         _context.Images.Add(projectImage);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetProject", new { id = projectImage.ProjectId }, projectImage);
      }

      private bool ProjectImageExists(int id)
      {
         return _context.Images.Any(e => e.ProjectId == id);
      }

      //Metodo range utilizado para cargar objetos predeterminados por requerimiento de instrucciones

      [HttpPost("img/Range")]
      public async Task<ActionResult<Project>> PostImageRange(List<ProjectImage> projectImages)
      {
         _context.Images.AddRange(projectImages);
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
