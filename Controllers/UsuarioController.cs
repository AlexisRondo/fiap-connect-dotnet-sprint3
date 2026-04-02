using Microsoft.AspNetCore.Mvc;
using FiapConnect.Models;
using FiapConnect.DTOs;

namespace FiapConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        // Simulando banco de dados em memória
        private static List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario
            {
                IdUsuario = 1,
                RM = "RM12345",
                NomeCompleto = "João Silva",
                EmailInstitucional = "joao.silva@fiap.com.br",
                Curso = "Engenharia de Software",
                Periodo = "Noturno",
                Turma = "3SIS",
                StatusBusca = true,
                DataCadastro = DateTime.Now.AddDays(-30)
            },
            new Usuario
            {
                IdUsuario = 2,
                RM = "RM54321",
                NomeCompleto = "Maria Santos",
                EmailInstitucional = "maria.santos@fiap.com.br",
                Curso = "Análise de Sistemas",
                Periodo = "Matutino",
                Turma = "3SIA",
                StatusBusca = true,
                DataCadastro = DateTime.Now.AddDays(-15)
            }
        };

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os usuários
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<UsuarioDTO>> GetUsuarios()
        {
            _logger.LogInformation("Listando todos os usuários. Total: {Total}", usuarios.Count);

            var usuariosDTO = usuarios.Select(u => new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                RM = u.RM,
                NomeCompleto = u.NomeCompleto,
                EmailInstitucional = u.EmailInstitucional,
                Curso = u.Curso,
                Periodo = u.Periodo,
                Turma = u.Turma,
                StatusBusca = u.StatusBusca,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/usuario/{u.IdUsuario}" },
                    { "habilidades", $"/api/usuario/{u.IdUsuario}/habilidades" },
                    { "grupos", $"/api/usuario/{u.IdUsuario}/grupos" }
                }
            });

            return Ok(usuariosDTO);
        }

        /// <summary>
        /// Obtém usuário por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<UsuarioDTO> GetUsuario(int id)
        {
            _logger.LogInformation("Buscando usuário ID: {Id}", id);

            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário ID {Id} não encontrado.", id);
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            }

            var usuarioDTO = new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                RM = usuario.RM,
                NomeCompleto = usuario.NomeCompleto,
                EmailInstitucional = usuario.EmailInstitucional,
                Curso = usuario.Curso,
                Periodo = usuario.Periodo,
                Turma = usuario.Turma,
                StatusBusca = usuario.StatusBusca,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/usuario/{usuario.IdUsuario}" },
                    { "update", $"/api/usuario/{usuario.IdUsuario}" },
                    { "delete", $"/api/usuario/{usuario.IdUsuario}" },
                    { "habilidades", $"/api/usuario/{usuario.IdUsuario}/habilidades" },
                    { "grupos", $"/api/usuario/{usuario.IdUsuario}/grupos" }
                }
            };

            return Ok(usuarioDTO);
        }

        /// <summary>
        /// Cria novo usuário
        /// </summary>
        [HttpPost]
        public ActionResult<UsuarioDTO> CreateUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos ao criar usuário.");
                return BadRequest(ModelState);
            }

            if (!usuario.EmailInstitucional.EndsWith("@fiap.com.br"))
            {
                _logger.LogWarning("Tentativa de cadastro com email inválido: {Email}", usuario.EmailInstitucional);
                return BadRequest(new { message = "Email deve ser do domínio FIAP (@fiap.com.br)" });
            }

            if (usuarios.Any(u => u.RM == usuario.RM))
            {
                _logger.LogWarning("Tentativa de cadastro com RM duplicado: {RM}", usuario.RM);
                return Conflict(new { message = "RM já cadastrado" });
            }

            usuario.IdUsuario = usuarios.Count > 0 ? usuarios.Max(u => u.IdUsuario) + 1 : 1;
            usuario.DataCadastro = DateTime.Now;
            usuarios.Add(usuario);

            _logger.LogInformation("Usuário criado com sucesso. ID: {Id}, RM: {RM}", usuario.IdUsuario, usuario.RM);

            var usuarioDTO = new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                RM = usuario.RM,
                NomeCompleto = usuario.NomeCompleto,
                EmailInstitucional = usuario.EmailInstitucional,
                Curso = usuario.Curso,
                Periodo = usuario.Periodo,
                Turma = usuario.Turma,
                StatusBusca = usuario.StatusBusca,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/usuario/{usuario.IdUsuario}" },
                    { "update", $"/api/usuario/{usuario.IdUsuario}" },
                    { "delete", $"/api/usuario/{usuario.IdUsuario}" }
                }
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuarioDTO);
        }

        /// <summary>
        /// Busca usuários com paginação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<PagedResultDTO<UsuarioDTO>> SearchUsuarios(
            [FromQuery] string nome = null,
            [FromQuery] string curso = null,
            [FromQuery] bool? statusBusca = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string orderBy = "nome")
        {
            _logger.LogInformation("Buscando usuários. Filtros: nome={Nome}, curso={Curso}, page={Page}", nome, curso, page);

            var query = usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(u => u.NomeCompleto.Contains(nome, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(curso))
                query = query.Where(u => u.Curso.Contains(curso, StringComparison.OrdinalIgnoreCase));
            if (statusBusca.HasValue)
                query = query.Where(u => u.StatusBusca == statusBusca.Value);

            query = orderBy.ToLower() switch
            {
                "rm" => query.OrderBy(u => u.RM),
                "curso" => query.OrderBy(u => u.Curso),
                "data" => query.OrderBy(u => u.DataCadastro),
                _ => query.OrderBy(u => u.NomeCompleto)
            };

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UsuarioDTO
                {
                    IdUsuario = u.IdUsuario,
                    RM = u.RM,
                    NomeCompleto = u.NomeCompleto,
                    EmailInstitucional = u.EmailInstitucional,
                    Curso = u.Curso,
                    Periodo = u.Periodo,
                    Turma = u.Turma,
                    StatusBusca = u.StatusBusca,
                    Links = new Dictionary<string, string>
                    {
                        { "self", $"/api/usuario/{u.IdUsuario}" },
                        { "details", $"/api/usuario/{u.IdUsuario}" }
                    }
                })
                .ToList();

            var result = new PagedResultDTO<UsuarioDTO>
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
                    { "self", $"/api/usuario/search?page={page}&pageSize={pageSize}" },
                    { "first", $"/api/usuario/search?page=1&pageSize={pageSize}" },
                    { "last", $"/api/usuario/search?page={totalPages}&pageSize={pageSize}" }
                }
            };

            if (result.HasPrevious)
                result.Links.Add("previous", $"/api/usuario/search?page={page - 1}&pageSize={pageSize}");
            if (result.HasNext)
                result.Links.Add("next", $"/api/usuario/search?page={page + 1}&pageSize={pageSize}");

            return Ok(result);
        }

        /// <summary>
        /// Atualiza status de busca do usuário
        /// </summary>
        [HttpPatch("{id}/status")]
        public ActionResult UpdateStatusBusca(int id, [FromBody] bool statusBusca)
        {
            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário ID {Id} não encontrado para atualização de status.", id);
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            }

            usuario.StatusBusca = statusBusca;
            _logger.LogInformation("Status de busca do usuário ID {Id} atualizado para {Status}.", id, statusBusca);
            return Ok(new { message = "Status atualizado com sucesso", statusBusca });
        }

        /// <summary>
        /// Remove usuário
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteUsuario(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
            {
                _logger.LogWarning("Tentativa de deletar usuário ID {Id} inexistente.", id);
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            }

            usuarios.Remove(usuario);
            _logger.LogInformation("Usuário ID {Id} removido com sucesso.", id);
            return NoContent();
        }
    }
}
