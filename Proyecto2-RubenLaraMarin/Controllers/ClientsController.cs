using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto2_RubenLaraMarin.Data;
using Proyecto2_RubenLaraMarin.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Proyecto2_RubenLaraMarin.Controllers
{
   public class ClientsController : Controller
   {
      private readonly IHttpClientFactory _httpClientFactory;
      private readonly HttpClient _httpClient;
      private static ApiCalls call;

      public ClientsController( IHttpClientFactory httpClientFactory)
      {
         _httpClientFactory = httpClientFactory;
         _httpClient = _httpClientFactory.CreateClient("Clientes");
         _httpClient.BaseAddress = new Uri("http://localhost:35968");
         if (call == null)
            call = new ApiCalls(_httpClient);
      }

      
      public ActionResult Index()
      {
         return View();
      }

      //CREATE METHODS

      public ActionResult CreateClient()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> CreateClient(Client client)
      {
         try
         {
            await call.PostObject(client, "/api/Clientes");
            return RedirectToAction(nameof(List));
         }
         catch (Exception e)
         {
            ViewBag.Message = e.Message;
            return View();
         }
      }

      //SHOW METHOD

      public ActionResult List()
      {
         return View(call.GetClientsAsync().Result);
      }


      //EDITION METHODS

      public async Task<ActionResult> EditClient()
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
      public async Task<ActionResult> EditClient(Client client)
      {
         try
         {
            await call.PutObject(client, "/api/Clientes/" + client.ClientId);
            return RedirectToAction(nameof(List));
         }
         catch (Exception e)
         {
            ViewBag.Message = e.Message;
            return View(nameof(Index));
         }
      }

   }
}
