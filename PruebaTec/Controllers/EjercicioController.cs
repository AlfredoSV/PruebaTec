using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PruebaTec.AccesoDatos;
using PruebaTec.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTec.Controllers
{
    public class EjercicioController : Controller
    {
        private readonly ILogger<EjercicioController> _logger;
        private readonly DbDiasLaborales _dbDiasLaborales;

        public EjercicioController(ILogger<EjercicioController> logger,IConfiguration configuration)
        {
            _dbDiasLaborales = new DbDiasLaborales(configuration.GetSection("db").Value);
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Ejercicio1() => View();

        [HttpPost]
        public IActionResult Ejercicio1Res(DateTime fechaAdqui)
        {
            var respuestaModelo = new ResultadoEjercicio1();
            respuestaModelo.Mensaje1 = $"Fecha Seleccionada: {fechaAdqui.ToString("dd/MM/yyyy")}";

            string mensaje = "Esta fecha no es valida";

            if (!(fechaAdqui == DateTime.MinValue || fechaAdqui == DateTime.MaxValue))
            {
                var dia = (int)fechaAdqui.DayOfWeek;
                if (dia == 0 || dia == 6)
                {
                    mensaje = "Esta fecha corresponde a un día sábado o domingo";
                }
                else
                {
                    if (_dbDiasLaborales.ConsultarDiaLaboral(fechaAdqui.Day,fechaAdqui.Month))
                    {
                        mensaje = "Es un día feriado, favor de seleccionar otro";

                    }
                    else
                    {
                        var ulimoLunesDelMes = new DateTime(fechaAdqui.Year, fechaAdqui.Month, DateTime.DaysInMonth(fechaAdqui.Year, fechaAdqui.Month));

                        while (ulimoLunesDelMes.DayOfWeek != DayOfWeek.Monday)
                            ulimoLunesDelMes = ulimoLunesDelMes.AddDays(-1);

                        var lunes = ulimoLunesDelMes.Day;

                        if (fechaAdqui.Day < lunes)
                        {
                            mensaje = $"Fecha de cobro: {fechaAdqui.ToString("dd/MM/yyyy")}";
                        }
                        else
                        {
                            var fechaSiguienteMes = new DateTime(fechaAdqui.Year, fechaAdqui.Month, 1).AddMonths(1);
                            while ((int)fechaSiguienteMes.DayOfWeek == 0 || (int)fechaSiguienteMes.DayOfWeek == 6 && _dbDiasLaborales.ConsultarDiaLaboral(fechaSiguienteMes.Day, fechaSiguienteMes.Month))
                            {
                                fechaSiguienteMes = fechaSiguienteMes.AddDays(1);
                            }

                            mensaje = $"Fecha de cobro: {fechaSiguienteMes.ToString("dd/MM/yyyy")}";
                        }

                    }
                                       
                }
            }

            respuestaModelo.Mensaje2 = mensaje;
            return View("Ejercicio1", respuestaModelo);
        }

        [HttpGet]
        public IActionResult Ejercicio2()
        {
            StreamReader streader = new StreamReader("Recursos/ubicaciones.json", Encoding.UTF8);
            string jsonString = streader.ReadToEnd();
            var ubicaciones = JsonConvert.DeserializeObject<UbicacionesGenerales>(jsonString);

            return View(ubicaciones.Ubicaciones.ToList().OrderBy(ubicacion => ubicacion.City));
        }

        [HttpGet]
        public IActionResult Ejercicio3() => View();


        [HttpPost]
        public async Task<IActionResult> Ejercicio3Res(string nombre)
        {
            var paises = string.Empty;
            var mensaje = "No se encontrarón coincidencias";
            var resultado = string.Empty;
            var objetoRes = new NombreComun();

            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync($"https://api.nationalize.io/?name={nombre}");

            if (respuesta.IsSuccessStatusCode)
            {
                resultado = respuesta.Content.ReadAsStringAsync().Result.ToString();
                objetoRes = JsonConvert.DeserializeObject<NombreComun>(resultado);
            }
            
            

            if (!resultado.Equals("") && objetoRes.Country.Any())
            {
                objetoRes.Country.ToList().ForEach(c => paises += c.Country_id + ",");

                mensaje = $"El nombre ingresado es más común en: {paises.Trim(',')}";

            }

            return View("Ejercicio3", mensaje);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
