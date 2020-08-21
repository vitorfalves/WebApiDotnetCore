using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public EventosController(IProAgilRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        [Route("GetEventoById")]
        public async Task<IActionResult> GetEventoById(int id)
        {
            try
            {
                Evento result = await _repo.GetEventoAsyncById(id, true);
                return  Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet]
        [Route("GetEventosByTema")]
        public async Task<IActionResult> GetEventosByTema(string tema)
        {
            try
            {
                Evento[] result = await _repo.GetAllEventosAsyncByTema(tema, true);
                return  Ok(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet]
        [Route("GetEventosList")]
        public async Task<IActionResult> GetEventosList()
        {
            try
            {
                Evento[] results = await _repo.GetAllEventosAsync(true);

                return Ok(results);
        
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                _repo.add(model);
                if(await _repo.SaveChangesAsync())
                {
                    return  Created($"Eventos/GetEventoById/{model.Id}", model);
                }
                
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return  BadRequest();
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(Evento model)
        {
            try
            {
                var evento = _repo.GetEventoAsyncById(model.Id, false);
                if(evento == null)
                    return NotFound();

                _repo.Update(model);

                if(await _repo.SaveChangesAsync())
                {
                    return  Created($"Eventos/GetEventoById/{model.Id}", model);
                }
                
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return  BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(id, false);
                if(evento == null)
                    return NotFound();

                _repo.Delete(evento);
                
                if(await _repo.SaveChangesAsync())
                {
                    return  Ok();
                }
                
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return  BadRequest();
        }
    }
}

