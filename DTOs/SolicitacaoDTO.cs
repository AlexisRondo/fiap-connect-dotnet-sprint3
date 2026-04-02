using System;
using System.Collections.Generic;

namespace FiapConnect.DTOs
{
    public class SolicitacaoDTO
    {
        public int IdSolicitacao { get; set; }
        public int IdGrupo { get; set; }
        public string NomeGrupo { get; set; }
        public int IdUsuarioSolicitante { get; set; }
        public string NomeSolicitante { get; set; }
        public string MensagemSolicitacao { get; set; }
        public string StatusSolicitacao { get; set; }
        public DateTime DataSolicitacao { get; set; }

        // HATEOAS Links
        public Dictionary<string, string> Links { get; set; }
    }
}