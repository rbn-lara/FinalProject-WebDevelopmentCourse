using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto2_RubenLaraMarin.Data;
using Proyecto2_RubenLaraMarin.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Proyecto2_RubenLaraMarin.Controllers
{
   public class ProjectsController : Controller
   {
      private readonly IHttpClientFactory _httpClientFactory;
      private readonly HttpClient _httpClient;
      private static ApiCalls call;

      public ProjectsController(IHttpClientFactory httpClientFactory)
      {
         _httpClientFactory = httpClientFactory;
         _httpClient = _httpClientFactory.CreateClient("Proyectos");
         _httpClient.BaseAddress = new Uri("http://localhost:35968");

         if (call == null)
            call = new ApiCalls(_httpClient);
      }

      public ActionResult Index()
      {
         return View();
      }

      // CREATE METHODS

      public async Task<ActionResult> CreateProject()
      {
         try
         {
            TempData["ClientCollection"] = JsonConvert.SerializeObject(await call.GetClientsAsync());
            return View();
         }
         catch (Exception e)
         {
            ViewBag.Message = e.Message;
            return View(nameof(Index));
         }
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> CreateProject(IFormCollection collection)
      {
         try
         {
            Project proyecto = collectionToProject(collection);
            byte[] buffer;
            if(proyecto.ProjectImage != null)
            {
               using (var stream = proyecto.ProjectImage.OpenReadStream())
               {
                  buffer = new byte[stream.Length];
                  stream.Read(buffer, 0, (int)stream.Length);
               }
            }
            else
            {
               buffer = null;
            }
            proyecto.ProjectImage = null;
            HttpResponseMessage respuesta = await call.PostObject(proyecto, "/api/proyectos_constru");
            string id = respuesta.Headers.Location.Segments[3];

            ProjectImage imagen = new ProjectImage() { ProjectId = int.Parse(id), ImageData = buffer };

            await call.PostObject(imagen, "api/proyectos_constru/img");
            return RedirectToAction(nameof(Index));

         }
         catch
         {
            return RedirectToAction(nameof(Index));
         }
      }

      //LIST PROJECTS OF

      [HttpPost("{id}")]
      public async Task<ActionResult> ListProjects(string id)
      {
         if (id is null)
            id = "0";
         var e = await call.GetProjectsOfAsync(id);
         int[] ids = new int[e.Count];
         int counter = 0;
         Dictionary<int, string> dic = new Dictionary<int, string>();
         foreach(var s in e)
         {
            ids[counter++] = s.ProjectId;
         }
         var imagernes = await call.GetImagesAsync(ids);
         if (imagernes is null)
            imagernes = new List<ProjectImage>();
         else
         {
            foreach (var img in imagernes)
            {
               if(img.ImageData != null)
                  dic.Add(img.ProjectId, Convert.ToBase64String(img.ImageData));
               else
                  dic.Add(img.ProjectId, string.Empty);
            }           
         }

         return View(new CustomModel() { Images = dic, Projects = e.ToList()  });
      }

      //EDIT PROJECT
      public async Task<ActionResult> Edit(string id)
      {
         return View(nameof(EditProject), await call.GetSingleProjectAsync(id));
      }

      [HttpPost("EditProject/{id}")]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> EditProject(string id, IFormCollection collection)
      {
         try
         {
            byte[] buffer;
            Project proyecto = collectionToProject(collection);
            if (id != proyecto.ProjectId.ToString())
               return BadRequest();
            if (proyecto.ProjectImage != null)
            {
               using (var stream = proyecto.ProjectImage.OpenReadStream())
               {
                  buffer = new byte[stream.Length];
                  stream.Read(buffer, 0, (int)stream.Length);
               }
            }
            else
            {
               buffer = null;
            }
            proyecto.ProjectImage = null;

            HttpResponseMessage respuesta = await call.PutObject(proyecto, "/api/proyectos_constru/" + proyecto.ProjectId.ToString());

            ProjectImage imagen = new ProjectImage() { ProjectId = proyecto.ProjectId, ImageData = buffer };
            if(imagen.ImageData != null)
               await call.PutObject(imagen, "api/proyectos_constru/img/" + imagen.ProjectId);

            return RedirectToAction(nameof(Index));
         }
         catch(Exception e)
         {
            ViewBag.Message = e.Message;
            return RedirectToAction(nameof(Index));
         }
      }


      //PRICE FORMULA

      private int CalculatePrice(Project proyecto)
      {
         int firstFactor, secondFactor;

         firstFactor = proyecto.Rooms + proyecto.Bathrooms + proyecto.HalfBathrooms + proyecto.Terrace + proyecto.FloorType + proyecto.KitchenFurnitureType;
         secondFactor = (proyecto.HaveSinkOutdoors ? 2 : 3) * proyecto.ConstructionSize;
         return (firstFactor + secondFactor) * 20000;
      }

      //FORM TO CUSTOM PROJECT OBJECT

      private Project collectionToProject(IFormCollection col)
      {
         Project temp = new Project()
         {
            ProjectName = col["ProjectName"],
            ClientId = col["cliente"],
            Bathrooms = int.Parse( col["Bathrooms"]),
            HalfBathrooms = int.Parse(col["HalfBathrooms"]),
            ConstructionSize = int.Parse(col["construction-size-radio"]),
            FloorType = int.Parse(col["floor-type-radio"]),
            KitchenFurnitureType = int.Parse(col["furniture-type-radio"]),
            Terrace = int.Parse(col["terrace-size-radio"]),
            Rooms = int.Parse(col["Rooms"]),
            ProjectImage = col.Files.GetFile("ProjectImage")
         };
         try
         {
            temp.IsLivingRoomKitchen = bool.Parse(col["IsLivingRoomKitchen"]);            
         }
         catch (ArgumentNullException)
         {
            temp.IsLivingRoomKitchen = false;
         }
         catch (FormatException)
         {
            temp.IsLivingRoomKitchen = true;
         }
         try
         {
            temp.HaveSinkOutdoors = bool.Parse(col["HaveSinkOutdoors"]);
         }
         catch (ArgumentNullException)
         {
            temp.HaveSinkOutdoors = false;
         }
         catch (FormatException)
         {
            temp.HaveSinkOutdoors = true;
         }
         try
         {
            temp.ProjectId = int.Parse(col["ProjectId"]);
         }
         catch (ArgumentNullException)
         {
            temp.ProjectId = 0;
         }



         temp.PriceBySquaredMeter = CalculatePrice(temp);

         return temp;         
      }

   }
}
