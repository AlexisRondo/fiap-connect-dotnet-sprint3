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
    public class SolicitacaoController : ControllerBase
    {
        private static List<Solicitacao> solicitacoes = new List<Solicitacao>();

        /// <summary>
        /// Cria nova solicitação para entrar em grupo
        /// </summary>
        [HttpPost]
        public ActionResult<SolicitacaoDTO> CreateSolicitacao([FromBody] Solicitacao solicitacao)
        {
            solicitacao.IdSolicitacao = solicitacoes.Count > 0 ? solicitacoes.Max(s => s.IdSolicitacao) + 1 : 1;
            solicitacao.DataSolicitacao = DateTime.Now;
            solicitacao.StatusSolicitacao = "PENDENTE";
            solicitacoes.Add(solicitacao);

            var dto = new SolicitacaoDTO
            {
                IdSolicitacao = solicitacao.IdSolicitacao,
                IdGrupo = solicitacao.IdGrupo,
                IdUsuarioSolicitante = solicitacao.IdUsuarioSolicitante,
                MensagemSolicitacao = solicitacao.MensagemSolicitacao,
                StatusSolicitacao = solicitacao.StatusSolicitacao,
                DataSolicitacao = solicitacao.DataSolicitacao,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/solicitacao/{solicitacao.IdSolicitacao}" },
                    { "aprovar", $"/api/solicitacao/{solicitacao.IdSolicitacao}/aprovar" },
                    { "rejeitar", $"/api/solicitacao/{solicitacao.IdSolicitacao}/rejeitar" }
                }
            };

            return CreatedAtAction(nameof(GetSolicitacao), new { id = solicitacao.IdSolicitacao }, dto);
        }

        /// <summary>
        /// Obtém solicitação por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<SolicitacaoDTO> GetSolicitacao(int id)
        {
            var solicitacao = solicitacoes.FirstOrDefault(s => s.IdSolicitacao == id);

            if (solicitacao == null)
                return NotFound(new { message = $"Solicitação {id} não encontrada" });

            return Ok(new SolicitacaoDTO
            {
                IdSolicitacao = solicitacao.IdSolicitacao,
                IdGrupo = solicitacao.IdGrupo,
                IdUsuarioSolicitante = solicitacao.IdUsuarioSolicitante,
                MensagemSolicitacao = solicitacao.MensagemSolicitacao,
                StatusSolicitacao = solicitacao.StatusSolicitacao,
                DataSolicitacao = solicitacao.DataSolicitacao,
                Links = new Dictionary<string, string>
                {
                    { "self", $"/api/solicitacao/{solicitacao.IdSolicitacao}" }
                }
            });
        }

        /// <summary>
        /// Busca solicitações com paginação
        /// </summary>
        [HttpGet("search")]
        public ActionResult<PagedResultDTO<SolicitacaoDTO>> SearchSolicitacoes(
            [FromQuery] string status = null,
            [FromQuery] int? idGrupo = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = solicitacoes.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(s => s.StatusSolicitacao == status.ToUpper());

            if (idGrupo.HasValue)
                query = query.Where(s => s.IdGrupo == idGrupo.Value);

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SolicitacaoDTO
                {
                    IdSolicitacao = s.IdSolicitacao,
                    IdGrupo = s.IdGrupo,
                    IdUsuarioSolicitante = s.IdUsuarioSolicitante,
                    MensagemSolicitacao = s.MensagemSolicitacao,
                    StatusSolicitacao = s.StatusSolicitacao,
                    DataSolicitacao = s.DataSolicitacao,
                    Links = new Dictionary<string, string>
                    {
                        { "self", $"/api/solicitacao/{s.IdSolicitacao}" }
                    }
                })
                .ToList();

            return Ok(new PagedResultDTO<SolicitacaoDTO>
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