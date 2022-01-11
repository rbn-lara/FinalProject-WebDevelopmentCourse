using Newtonsoft.Json;
using Proyecto2_RubenLaraMarin.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Proyecto2_RubenLaraMarin.Data
{
   public class ApiCalls
   {
      private readonly HttpClient _httpClient;

      public ApiCalls(HttpClient httpClient)
      {
         _httpClient = httpClient;
         if (!ClientsInitialized().Result)
            InitializeClients();
         if (!ProjectsInitialized().Result)
            InitializeProjects();
      }

      //Initializers

      public async void InitializeClients()
      {
         await PostObject(new List<Client>() {
            new Client() { ClientId = "01-1111-1111", ClientName = "Jhon Doe Smith", Phone = "1111-1111" },
            new Client() { ClientId = "02-2222-2222", ClientName = "Moe Moe Moe", Phone = "2222-2222" },
            new Client() { ClientId = "5-555-555555", ClientName = "Sociedad Lechera S.A", Phone = "5555-5555" }
         }, "/api/Clientes/Range");
      }
      public async void InitializeProjects()
      {
         string sThisPath = AppDomain.CurrentDomain.BaseDirectory;
         string tempPath = Path.Combine(sThisPath, @"..\..\..\wwwroot\");
         string resourcesPath = Path.GetFullPath(tempPath);
         HttpResponseMessage respuesta = await PostObject(new Project()
         {
            ClientId = "01-1111-1111",
            ProjectName = "Mi casa sonada",
            Bathrooms = 2,
            HalfBathrooms = 1,
            ConstructionSize = 1,
            FloorType = 1,
            HaveSinkOutdoors = true,
            IsLivingRoomKitchen = true,
            KitchenFurnitureType = 0,
            PriceBySquaredMeter = 180000,
            Rooms = 3,
            Terrace = 0
         }, "/api/proyectos_constru");

         var a = respuesta.Content.ReadAsStringAsync();

         HttpResponseMessage respuesta2 = await PostObject(new Project()
         {
            ClientId = "02-2222-2222",
            ProjectName = "Proyecto de vida",
            Bathrooms = 2,
            HalfBathrooms = 2,
            ConstructionSize = 4,
            FloorType = 2,
            HaveSinkOutdoors = true,
            IsLivingRoomKitchen = false,
            KitchenFurnitureType = 2,
            PriceBySquaredMeter = 400000,
            Rooms = 3,
            Terrace = 1
         }, "/api/proyectos_constru");

         HttpResponseMessage respuesta3 = await PostObject(new Project()
         {
            ClientId = "5-555-555555",
            ProjectName = "Oficina mayor",
            Bathrooms = 2,
            HalfBathrooms = 1,
            ConstructionSize = 4,
            FloorType = 0,
            HaveSinkOutdoors = false,
            IsLivingRoomKitchen = false,
            KitchenFurnitureType = 0,
            PriceBySquaredMeter = 380000,
            Rooms = 4,
            Terrace = 0
         }, "/api/proyectos_constru");

         var id = respuesta.Headers.Location.Segments[3];
         var id2 = respuesta2.Headers.Location.Segments[3];
         var id3 = respuesta3.Headers.Location.Segments[3];

         await PostObject(new List<ProjectImage>() {
            new ProjectImage()
         {
            ProjectId = int.Parse(id),
            ImageData = ImgToArr(new Bitmap(resourcesPath + "house-1.png"))
         },
            new ProjectImage()
         {
            ProjectId = int.Parse(id2),
            ImageData = ImgToArr(new Bitmap(resourcesPath + "house-2.png"))
         },
            new ProjectImage()
         {
            ProjectId = int.Parse(id3),
            ImageData = ImgToArr(new Bitmap(resourcesPath + "house-3.png"))
         }

         }, "/api/proyectos_constru/img/Range");
      }

      //Gets
      public async Task<ICollection<Client>> GetClientsAsync()
      {
         HttpResponseMessage responseMessage = await _httpClient.GetAsync("/api/Clientes");
         return DeserializeClients(await responseMessage.Content.ReadAsStringAsync());
      }
      /*
      public async Task<ICollection<Project>> GetProjectsAsync()
      {
         HttpResponseMessage responseMessage = await _httpClient.GetAsync("/api/proyectos_constru");
         return DeserializeProjects(await responseMessage.Content.ReadAsStringAsync());
      }
      */
      public async Task<ICollection<Project>> GetProjectsOfAsync(string id)
      {
         HttpResponseMessage responseMessage = await _httpClient.GetAsync("/api/proyectos_constru/cliente/" + id);
         return DeserializeProjects(await responseMessage.Content.ReadAsStringAsync());
      }
      public async Task<Project> GetSingleProjectAsync(string id)
      {
         HttpResponseMessage responseMessage = await _httpClient.GetAsync("/api/proyectos_constru/" + id);
         return DeserializeProject(await responseMessage.Content.ReadAsStringAsync());
      }
      public async Task<ICollection<ProjectImage>> GetImagesAsync(int[] idList)
      {
         string parametersRoute = "/api/proyectos_constru/e/GetImages?";
         int counter = 1;
         foreach (var i in idList)
         {
            if (idList.Length == counter++)
               parametersRoute += "idList=" + i.ToString();
            else
               parametersRoute += "idList=" + i.ToString() + "&";
         }

         HttpResponseMessage responseMessage = await _httpClient.GetAsync(parametersRoute);
         return DeserializeImages(await responseMessage.Content.ReadAsStringAsync());
      }

      //Post

      public async Task<HttpResponseMessage> PostObject(object toPost, string route)
      {
         return await _httpClient.PostAsync(route, SerializeObject(toPost));
      }

      //Put
      public async Task<HttpResponseMessage> PutObject(object toPut, string route)
      {
         return await _httpClient.PutAsync(route, SerializeObject(toPut));
      }


      //Verifying methods

      public async Task<bool> CLientExist(string id)
      {
         string response = await _httpClient.GetStringAsync("/api/Clientes/Exists/" + id);
         return bool.Parse(response);
      }

      public async Task<bool> ClientsInitialized()
      {
         string response = await _httpClient.GetStringAsync("/api/Clientes/Initialized");
         return bool.Parse(response);
      }

      public async Task<bool> ProjectsInitialized()
      {
         string response = await _httpClient.GetStringAsync("/api/proyectos_constru/Initialized");
         return bool.Parse(response);
      }

      //Misc methods

      public byte[] ImgToArr(Bitmap bm)
      {
         byte[] arr;
         using (MemoryStream ms = new MemoryStream())
         {
            Bitmap bmp = bm;
            bmp.Save(ms, format: System.Drawing.Imaging.ImageFormat.Png);
            arr = ms.ToArray();
         }
         return arr;
      }

      //Serializer
      private StringContent SerializeObject(object toSerialize)
      {
         return new StringContent(System.Text.Json.JsonSerializer.Serialize(toSerialize), Encoding.UTF8, Application.Json);
      }

      //Deserializers
      private ICollection<Client> DeserializeClients(string s)
      {
         return JsonConvert.DeserializeObject<ICollection<Client>>(s);
      }
      private ICollection<Project> DeserializeProjects(string s)
      {
         return JsonConvert.DeserializeObject<ICollection<Project>>(s);
      }
      private Project DeserializeProject(string s)
      {
         return JsonConvert.DeserializeObject<Project>(s);
      }
      private ICollection<ProjectImage> DeserializeImages(string s)
      {
         return JsonConvert.DeserializeObject<ICollection<ProjectImage>>(s);
      }
   }
}
