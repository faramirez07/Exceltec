using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Exceltec.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(Masco mascotaPar)
        {
            EntidadMascota mascota = new EntidadMascota();
            List<EntidadMascota> ListMascota = new List<EntidadMascota>();
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/");

            var request = clientHttp.PostAsync("pet", mascotaPar, new JsonMediaTypeFormatter()).Result;

            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                var resutado = resulstring.Count();
                if (resutado>0)
                {
                    return RedirectToAction("About");
                }
                return View(mascotaPar);

            }

            return View(mascotaPar);
        }

        public ActionResult About(string filtro)
        {
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/pet/" + filtro);

            var request = clientHttp.GetAsync("").Result;
            EntidadMascota mascota = new EntidadMascota();
            List<EntidadMascota> ListMascota = new List<EntidadMascota>();
            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                //var listado = JsonConvert.DeserializeObject(resulstring);
                // List<string> Customer = new JavaScriptSerializer().Deserialize<List<string>>(resulstring);
                //var numero = listado[0]["name"].ToString();
                var personaIntermedioObject = JObject.Parse(resulstring);
                mascota.Nombre = personaIntermedioObject["name"].ToString();
                mascota.Id = Convert.ToInt32(personaIntermedioObject["id"]);
                mascota.Disponible = personaIntermedioObject["status"].ToString();
                foreach (var item in personaIntermedioObject.First)
                {
                    ListMascota.Add(mascota);
                }
               

                return View(ListMascota);
            }
                return View(new List<EntidadMascota>());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Actualizar(int id)
        {
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/pet/" + id);

            var request = clientHttp.GetAsync("").Result;
            EntidadMascota mascota = new EntidadMascota();
            List<EntidadMascota> ListMascota = new List<EntidadMascota>();
            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                //var listado = JsonConvert.DeserializeObject(resulstring);
                // List<string> Customer = new JavaScriptSerializer().Deserialize<List<string>>(resulstring);
                //var numero = listado[0]["name"].ToString();
                var personaIntermedioObject = JObject.Parse(resulstring);
                mascota.Nombre = personaIntermedioObject["name"].ToString();
                mascota.Id = Convert.ToInt32(personaIntermedioObject["id"]);
                mascota.Disponible = personaIntermedioObject["status"].ToString();
                //foreach (var item in personaIntermedioObject.First)
                //{
                //    ListMascota.Add(mascota);
                //}


                return View(mascota);
            }
            return View(new EntidadMascota());
        }

        [HttpPost]
        public ActionResult Actualizar(EntidadMascota mascotaPar)
        {
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/");
            Masco par = new Masco();
            par.id = mascotaPar.Id;
            par.name = mascotaPar.Nombre;
            par.status = mascotaPar.Disponible;
            var request = clientHttp.PostAsync("pet", par, new JsonMediaTypeFormatter()).Result;

            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                var resutado = resulstring.Count();
                if (resutado > 0)
                {
                    return RedirectToAction("About");
                }
                return View(mascotaPar);
            }
            return View(mascotaPar);
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/");

            var request = clientHttp.DeleteAsync("pet/" + id).Result;

            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                var resutado = resulstring.Count();
                if (resutado > 0)
                {
                    return RedirectToAction("About");
                }

            }

            return View();
        }
    }
}