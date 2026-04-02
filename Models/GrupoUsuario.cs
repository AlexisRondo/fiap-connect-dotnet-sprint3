using System;
using System.ComponentModel.DataAnnotations;

namespace FiapConnect.Models
{
    public class GrupoUsuario
    {
        public int IdGrupoUsuario { get; set; }
        public int IdGrupo { get; set; }
        public int IdUsuario { get; set; }

        [StringLength(20)]
        public string PapelMembro { get; set; } // LIDER ou MEMBRO

        public DateTime DataEntrada { get; set; }

        // Navigation properties
        public virtual Grupo Grupo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}