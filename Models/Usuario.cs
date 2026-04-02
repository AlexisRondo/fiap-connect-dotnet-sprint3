using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FiapConnect.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "RM é obrigatório")]
        [StringLength(10)]
        public string RM { get; set; }

        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(100)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string EmailInstitucional { get; set; }

        [StringLength(50)]
        public string? Curso { get; set; }

        [StringLength(20)]
        public string? Periodo { get; set; }

        [StringLength(10)]
        public string? Turma { get; set; }

        public bool StatusBusca { get; set; }
        public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public virtual ICollection<GrupoUsuario> GruposUsuario { get; set; } = new List<GrupoUsuario>();

        [JsonIgnore]
        public virtual ICollection<UsuarioHabilidade> UsuarioHabilidades { get; set; } = new List<UsuarioHabilidade>();

        [JsonIgnore]
        public virtual ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();
    }
}