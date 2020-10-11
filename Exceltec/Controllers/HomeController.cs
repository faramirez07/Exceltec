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

        /// <summary>
        /// Método para ingresar un nuevo registro de mascota
        /// </summary>
        /// <param name="mascotaPar"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Nuevo(Masco mascotaPar)
        {
            EntidadMascota mascota = new EntidadMascota();
            List<EntidadMascota> ListMascota = new List<EntidadMascota>();
            //Referencia al WEBAPI proporcionado de swagger
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/");

            var request = clientHttp.PostAsync("pet", mascotaPar, new JsonMediaTypeFormatter()).Result;
            //Al ser positivo la conexión entra a ejecutar la consulta.
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

        /// <summary>
        /// Méteodo para obtener una máscota en especifico y mostrarla
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public ActionResult About(string filtro)
        {
            //Referencia al WEBAPI proporcionado de swagger
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/pet/" + filtro);

            var request = clientHttp.GetAsync("").Result;
            EntidadMascota mascota = new EntidadMascota();
            List<EntidadMascota> ListMascota = new List<EntidadMascota>();
            //Al ser positivo la conexión entra a ejecutar la consulta.
            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                var personaIntermedioObject = JObject.Parse(resulstring);
                //Se cambia a otra entidad para que tenga todas las variables del json
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
        /// <summary>
        /// Método para la pagina de contacto esta desabilitada
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Método para obtener el valor a actualizar de un registro de mascota
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Actualizar(int id)
        {
            //Referencia al WEBAPI proporcionado de swagger
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/pet/" + id);

            var request = clientHttp.GetAsync("").Result;
            EntidadMascota mascota = new EntidadMascota();
            List<EntidadMascota> ListMascota = new List<EntidadMascota>();
            //Al ser positivo la conexión entra a ejecutar la consulta.
            if (request.IsSuccessStatusCode)
            {
                var resulstring = request.Content.ReadAsStringAsync().Result;
                var personaIntermedioObject = JObject.Parse(resulstring);
                //Se cambia a otra entidad para que tenga todas las variables del json
                mascota.Nombre = personaIntermedioObject["name"].ToString();
                mascota.Id = Convert.ToInt32(personaIntermedioObject["id"]);
                mascota.Disponible = personaIntermedioObject["status"].ToString();


                return View(mascota);
            }
            return View(new EntidadMascota());
        }

        /// <summary>
        /// Método para actualizar un valor especifico de mascota
        /// </summary>
        /// <param name="mascotaPar"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Actualizar(EntidadMascota mascotaPar)
        {
            //Referencia al WEBAPI proporcionado de swagger
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/");
            Masco par = new Masco();
            par.id = mascotaPar.Id;
            par.name = mascotaPar.Nombre;
            par.status = mascotaPar.Disponible;
            var request = clientHttp.PostAsync("pet", par, new JsonMediaTypeFormatter()).Result;
            //Al ser positivo la conexión entra a ejecutar la consulta.
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

        /// <summary>
        /// Método para eliminar un registro especifico de mascota
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            //Referencia al WEBAPI proporcionado de swagger
            HttpClient clientHttp = new HttpClient();
            clientHttp.BaseAddress = new Uri("https://petstore.swagger.io/v2/");

            var request = clientHttp.DeleteAsync("pet/" + id).Result;
            //Al ser positivo la conexión entra a ejecutar la consulta.
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