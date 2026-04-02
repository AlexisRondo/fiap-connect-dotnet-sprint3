using Microsoft.AspNetCore.Mvc;
using FiapConnect.Models;
using FiapConnect.DTOs;

namespace FiapConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : ControllerBase
    {
        private readonly ILogger<GrupoController> _logger;

        private static List<Grupo> grupos = new List<Grupo>
        {
            new Grupo
            {
                IdGrupo = 1,
                NomeGrupo = "Tech Innovators",
                DescricaoProjeto = "Desenvolvimento de app mobile para gestão acadêmica",
                DisciplinaTema = "Mobile Development",
                MaxIntegrantes = 3,
                StatusGrupo = "ABERTO",
                DataCriacao = DateTime.Now.AddDays(-10)
            },
            new Grupo
            {
                IdGrupo = 2,
                NomeGrupo = "Data Masters",
                DescricaoProjeto = "Análise preditiva usando Machine Learning",
                DisciplinaTema = "Data Science",
                MaxIntegrantes = 3,
                StatusGrupo = "ABERTO",
                DataCriacao = DateTime.Now.AddDays(-5)
            }
        };

        private static List<GrupoUsuario> grupoUsuarios = new List<GrupoUsuario>();

        public GrupoController(ILogger<GrupoController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os grupos
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<GrupoDTO>> GetGrupos()
        {
            _logger.LogInformation("Listando todos os grupos. Total: {Total}", grupos.Count);

            var gruposDTO = grupos.Select(g => new GrupoDTO
            {
                IdGrupo = g.IdGrupo,
                NomeGrupo = g.NomeGrupo,
                DescricaoProjeto = g.DescricaoProjeto,
                DisciplinaTema = g.DisciplinaTema,
                MaxIntegrantes = g.MaxIntegrantes,
                IntegrantesAtuais = grupoUsuarios.Count(gu => gu.IdGrupo == g.IdGrupo),
                StatusGrupo = g.StatusGrupo,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/grupo/{g.IdGrupo}" },
                    { "membros", $"/api/grupo/{g.IdGrupo}/membros" },
                    { "solicitacoes", $"/api/grupo/{g.IdGrupo}/solicitacoes" }
                }
            });

            return Ok(gruposDTO);
        }

        /// <summary>
        /// Obtém grupo por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<GrupoDTO> GetGrupo(int id)
        {
            _logger.LogInformation("Buscando grupo ID: {Id}", id);

            var grupo = grupos.FirstOrDefault(g => g.IdGrupo == id);
            if (grupo == null)
            {
                _logger.LogWarning("Grupo ID {Id} não encontrado.", id);
                return NotFound(new { message = $"Grupo com ID {id} não encontrado" });
            }

            var grupoDTO = new GrupoDTO
            {
                IdGrupo = grupo.IdGrupo,
                NomeGrupo = grupo.NomeGrupo,
                DescricaoProjeto = grupo.DescricaoProjeto,
                DisciplinaTema = grupo.DisciplinaTema,
                MaxIntegrantes = grupo.MaxIntegrantes,
                IntegrantesAtuais = grupoUsuarios.Count(gu => gu.IdGrupo == grupo.IdGrupo),
                StatusGrupo = grupo.StatusGrupo,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/grupo/{grupo.IdGrupo}" },
                    { "update", $"/api/grupo/{grupo.IdGrupo}" },
                    { "delete", $"/api/grupo/{grupo.IdGrupo}" },
                    { "membros", $"/api/grupo/{grupo.IdGrupo}/membros" }
                }
            };

            return Ok(grupoDTO);
        }

        /// <summary>
        /// Cria novo grupo
        /// </summary>
        [HttpPost]
        public ActionResult<GrupoDTO> CreateGrupo([FromBody] Grupo grupo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos ao criar grupo.");
                return BadRequest(ModelState);
            }

            if (grupo.MaxIntegrantes < 2 || grupo.MaxIntegrantes > 3)
            {
                _logger.LogWarning("Número de integrantes inválido: {Max}", grupo.MaxIntegrantes);
                return BadRequest(new { message = "Número de integrantes deve ser entre 2 e 3." });
            }

            grupo.IdGrupo = grupos.Count > 0 ? grupos.Max(g => g.IdGrupo) + 1 : 1;
            grupo.DataCriacao = DateTime.Now;
            grupo.StatusGrupo = "ABERTO";
            grupos.Add(grupo);

            _logger.LogInformation("Grupo criado com sucesso. ID: {Id}, Nome: {Nome}", grupo.IdGrupo, grupo.NomeGrupo);

            var grupoDTO = new GrupoDTO
            {
                IdGrupo = grupo.IdGrupo,
                NomeGrupo = grupo.NomeGrupo,
                DescricaoProjeto = grupo.DescricaoProjeto,
                DisciplinaTema = grupo.DisciplinaTema,
                MaxIntegrantes = grupo.MaxIntegrantes,
                IntegrantesAtuais = 0,
                StatusGrupo = grupo.StatusGrupo,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/grupo/{grupo.IdGrupo}" },
                    { "update", $"/api/grupo/{grupo.IdGrupo}" },
                    { "delete", $"/api/grupo/{grupo.IdGrupo}" }
                }
            };

            return CreatedAtAction(nameof(GetGrupo), new { id = grupo.IdGrupo }, grupoDTO);
        }

        /// <summary>
        /// Atualiza grupo
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<GrupoDTO> UpdateGrupo(int id, [FromBody] Grupo grupoAtualizado)
        {
            var grupo = grupos.FirstOrDefault(g => g.IdGrupo == id);
            if (grupo == null)
            {
                _logger.LogWarning("Grupo ID {Id} não encontrado para atualização.", id);
                return NotFound(new { message = $"Grupo com ID {id} não encontrado" });
            }

            grupo.NomeGrupo = grupoAtualizado.NomeGrupo ?? grupo.NomeGrupo;
            grupo.DescricaoProjeto = grupoAtualizado.DescricaoProjeto ?? grupo.DescricaoProjeto;
            grupo.StatusGrupo = grupoAtualizado.StatusGrupo ?? grupo.StatusGrupo;

            _logger.LogInformation("Grupo ID {Id} atualizado com sucesso.", id);
            return Ok(new GrupoDTO
            {
                IdGrupo = grupo.IdGrupo,
                NomeGrupo = grupo.NomeGrupo,
                DescricaoProjeto = grupo.DescricaoProjeto,
                DisciplinaTema = grupo.DisciplinaTema,
                MaxIntegrantes = grupo.MaxIntegrantes,
                StatusGrupo = grupo.StatusGrupo,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/grupo/{grupo.IdGrupo}" }
                }
            });
        }

        /// <summary>
        /// Remove grupo
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteGrupo(int id)
        {
            var grupo = grupos.FirstOrDefault(g => g.IdGrupo == id);
            if (grupo == null)
            {
                _logger.LogWarning("Tentativa de deletar grupo ID {Id} inexistente.", id);
                return NotFound(new { message = $"Grupo com ID {id} não encontrado" });
            }

            grupos.Remove(grupo);
            _logger.LogInformation("Grupo ID {Id} removido com sucesso.", id);
            return NoContent();
        }

        /// <summary>
        /// Lista grupos abertos (com vagas)
        /// </summary>
        [HttpGet("abertos")]
        public ActionResult<IEnumerable<GrupoDTO>> GetGruposAbertos()
        {
            _logger.LogInformation("Listando grupos com vagas disponíveis.");

            var gruposAbertos = grupos
                .Where(g => g.StatusGrupo == "ABERTO" &&
                            grupoUsuarios.Count(gu => gu.IdGrupo == g.IdGrupo) < g.MaxIntegrantes)
                .Select(g => new GrupoDTO
                {
                    IdGrupo = g.IdGrupo,
                    NomeGrupo = g.NomeGrupo,
                    DescricaoProjeto = g.DescricaoProjeto,
                    DisciplinaTema = g.DisciplinaTema,
                    MaxIntegrantes = g.MaxIntegrantes,
                    IntegrantesAtuais = grupoUsuarios.Count(gu => gu.IdGrupo == g.IdGrupo),
                    StatusGrupo = g.StatusGrupo,
                    Links = new Dictionary<string, string>
                    {
                        { "self", $"/api/grupo/{g.IdGrupo}" },
                        { "solicitar", $"/api/solicitacao" }
                    }
                });

            return Ok(gruposAbertos);
        }

        /// <summary>
        /// Busca grupos com paginação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<PagedResultDTO<GrupoDTO>> SearchGrupos(
            [FromQuery] string nome = null,
            [FromQuery] string status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Buscando grupos. Filtros: nome={Nome}, status={Status}, page={Page}", nome, status, page);

            var query = grupos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(g => g.NomeGrupo.Contains(nome, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(status))
                query = query.Where(g => g.StatusGrupo == status.ToUpper());

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GrupoDTO
                {
                    IdGrupo = g.IdGrupo,
                    NomeGrupo = g.NomeGrupo,
                    DescricaoProjeto = g.DescricaoProjeto,
                    DisciplinaTema = g.DisciplinaTema,
                    MaxIntegrantes = g.MaxIntegrantes,
                    IntegrantesAtuais = grupoUsuarios.Count(gu => gu.IdGrupo == g.IdGrupo),
                    StatusGrupo = g.StatusGrupo,
                    Links = new Dictionary<string, string>
                    {
                        { "self", $"/api/grupo/{g.IdGrupo}" }
                    }
                })
                .ToList();

            return Ok(new PagedResultDTO<GrupoDTO>
            {
                Data = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPrevious = page > 1,
                HasNext = page < totalPages,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/grupo/search?page={page}&pageSize={pageSize}" },
                    { "first", $"/api/grupo/search?page=1&pageSize={pageSize}" },
                    { "last", $"/api/grupo/search?page={totalPages}&pageSize={pageSize}" }
                }
            });
        }
    }
}
