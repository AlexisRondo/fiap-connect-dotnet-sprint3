using Microsoft.AspNetCore.Mvc;
using FiapConnect.Models;
using FiapConnect.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiapConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabilidadeController : ControllerBase
    {
        private static List<Habilidade> habilidades = new List<Habilidade>
        {
            new Habilidade { IdHabilidade = 1, NomeHabilidade = "C#", TipoHabilidade = "TECNICA", NivelProficiencia = "AVANCADO" },
            new Habilidade { IdHabilidade = 2, NomeHabilidade = "Java", TipoHabilidade = "TECNICA", NivelProficiencia = "INTERMEDIARIO" },
            new Habilidade { IdHabilidade = 3, NomeHabilidade = "React", TipoHabilidade = "TECNICA", NivelProficiencia = "BASICO" },
            new Habilidade { IdHabilidade = 4, NomeHabilidade = "Liderança", TipoHabilidade = "COMPORTAMENTAL", NivelProficiencia = "AVANCADO" },
            new Habilidade { IdHabilidade = 5, NomeHabilidade = "Trabalho em Equipe", TipoHabilidade = "COMPORTAMENTAL", NivelProficiencia = "AVANCADO" }
        };

        /// <summary>
        /// Lista todas as habilidades
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<HabilidadeDTO>> GetHabilidades()
        {
            var habilidadesDTO = habilidades.Select(h => new HabilidadeDTO
            {
                IdHabilidade = h.IdHabilidade,
                NomeHabilidade = h.NomeHabilidade,
                TipoHabilidade = h.TipoHabilidade,
                NivelProficiencia = h.NivelProficiencia,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/habilidade/{h.IdHabilidade}" }
                }
            });

            return Ok(habilidadesDTO);
        }

        /// <summary>
        /// Busca habilidades com paginação
        /// </summary>
        [HttpGet("search")]
        public ActionResult<PagedResultDTO<HabilidadeDTO>> SearchHabilidades(
            [FromQuery] string tipo = null,
            [FromQuery] string nivel = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = habilidades.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(h => h.TipoHabilidade == tipo.ToUpper());

            if (!string.IsNullOrEmpty(nivel))
                query = query.Where(h => h.NivelProficiencia == nivel.ToUpper());

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new HabilidadeDTO
                {
                    IdHabilidade = h.IdHabilidade,
                    NomeHabilidade = h.NomeHabilidade,
                    TipoHabilidade = h.TipoHabilidade,
                    NivelProficiencia = h.NivelProficiencia,
                    Links = new Dictionary<string, string>
                    {
                        { "self", $"/api/habilidade/{h.IdHabilidade}" }
                    }
                })
                .ToList();

            return Ok(new PagedResultDTO<HabilidadeDTO>
            {
                Data = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPrevious = page > 1,
                HasNext = page < totalPages
            });
        }
    }
}