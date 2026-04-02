using System;
using System.ComponentModel.DataAnnotations;

namespace FiapConnect.Models
{
    public class Solicitacao
    {
        public int IdSolicitacao { get; set; }
        public int IdGrupo { get; set; }
        public int IdUsuarioSolicitante { get; set; }

        [StringLength(500)]
        public string MensagemSolicitacao { get; set; }

        [StringLength(20)]
        public string StatusSolicitacao { get; set; } = "PENDENTE"; // PENDENTE, APROVADA, REJEITADA

        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }

        // Navigation properties
        public virtual Grupo Grupo { get; set; }
        public virtual Usuario UsuarioSolicitante { get; set; }
    }
}