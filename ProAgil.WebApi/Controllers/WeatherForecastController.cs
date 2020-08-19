using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<Evento> Get(int id)
        {
            return new Evento []{
                new Evento(){
                    Id = 1,
                    Tema = "Angular e dotnet",
                    Local = "BH",
                    QtdPessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2)
                },
                new Evento(){
                    Id = 2,
                    Tema = "Angular e EF",
                    Local = "SP",
                    QtdPessoas = 350,
                    DataEvento = DateTime.Now.AddDays(2)
                }
            }.FirstOrDefault(x => x.Id == id);
        }
    }
}
