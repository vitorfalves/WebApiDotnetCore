using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _Context;

        public ProAgilRepository(ProAgilContext context)
        {
            _Context = context;
        }

        //GERAIS
        public void add<T>(T entity) where T : class
        {
            _Context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _Context.Update(entity);
            
        }
        public void Delete<T>(T entity) where T : class
        {
            _Context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _Context.SaveChangesAsync()) > 0;
        }

        //EVENTOS
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _Context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSocial);

            if(includePalestrantes)
            {
                query = query
                        .Include(pe => pe.PalestranteEventos)
                        .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                        .OrderByDescending(c => c.Id);

            return await query.ToArrayAsync();
                    
        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _Context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSocial);

            if(includePalestrantes)
            {
                query = query
                        .Include(pe => pe.PalestranteEventos)
                        .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                         .OrderByDescending(c => c.DataEvento)
                         .Where(p => p.Tema.ToLower().Contains(tema.ToLower()));


            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _Context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedeSocial);

            if(includePalestrantes)
            {
                query = query
                        .Include(pe => pe.PalestranteEventos)
                        .ThenInclude(p => p.Palestrante);
            }

            query = query.AsNoTracking()
                         .OrderByDescending(c => c.Id)
                         .Where(c => c.Id == EventoId);


            return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTES
        public async Task<Palestrante> GetPalestranteByIdAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _Context.Palestrante
                .Include(c => c.RedeSociais);

            if(includeEventos)
            {
                query = query
                        .Include(pe => pe.PalestranteEventos)
                        .ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome)
                         .Where(p => p.Id == PalestranteId);


            return await query.FirstOrDefaultAsync();
        }
         public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _Context.Palestrante
                .Include(c => c.RedeSociais);

            if(includeEventos)
            {
                query = query
                        .Include(pe => pe.PalestranteEventos)
                        .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking()
                         .OrderBy(p => p.Nome)
                         .Where(p => p.Nome.ToLower().Contains(name.ToLower()));


            return await query.ToArrayAsync();
        }
    }
}