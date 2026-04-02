using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FiapConnect.Models
{
    public class Grupo
    {
        public int IdGrupo { get; set; }

        [Required(ErrorMessage = "Nome do grupo é obrigatório")]
        [StringLength(100)]
        public string NomeGrupo { get; set; }

        [StringLength(500)]
        public string DescricaoProjeto { get; set; }

        [StringLength(50)]
        public string DisciplinaTema { get; set; }

        public int MaxIntegrantes { get; set; } = 3;

        [StringLength(20)]
        public string StatusGrupo { get; set; } = "ABERTO";

        public DateTime DataCriacao { get; set; }

        public DateTime? DataFechamento { get; set; }

        // Navigation properties
        public virtual ICollection<GrupoUsuario> GruposUsuario { get; set; }
        public virtual ICollection<Solicitacao> Solicitacoes { get; set; }
    }
}