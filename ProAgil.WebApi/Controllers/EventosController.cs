using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebApi.Dtos;

namespace ProAgil.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private readonly IMapper _mapper;

        public EventosController(IProAgilRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet]
        [Route("GetEventoById")]
        public async Task<IActionResult> GetEventoById(int id)
        {
            try
            {
                Evento evento = await _repo.GetEventoAsyncById(id, true);

                var result = _mapper.Map<EventoDto>(evento);

                return Ok(result);
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
                Evento[] evento = await _repo.GetAllEventosAsyncByTema(tema, true);

                var result = _mapper.Map<EventoDto>(evento);

                return Ok(result);
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
                Evento[] eventos = await _repo.GetAllEventosAsync(true);

                var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);

                return Ok(results);

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _repo.add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"Eventos/GetEventoById/{model.Id}", _mapper.Map<EventoDto>(evento));
                }

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou {ex.Message}");
            }
            return BadRequest();
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(EventoDto model)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(model.Id, false);
                if (evento == null)
                    return NotFound();

                _mapper.Map(model, evento);

                _repo.Update(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"Eventos/GetEventoById/{model.Id}", _mapper.Map<EventoDto>(evento));
                }

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(id, false);
                if (evento == null)
                    return NotFound();

                _repo.Delete(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }

            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
            return BadRequest();
        }
    }
}

