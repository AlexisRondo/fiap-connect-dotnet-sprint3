using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FiapConnect.Models
{
    public class Habilidade
    {
        public int IdHabilidade { get; set; }

        [Required]
        [StringLength(50)]
        public string NomeHabilidade { get; set; }

        [StringLength(20)]
        public string TipoHabilidade { get; set; } // TECNICA ou COMPORTAMENTAL

        [StringLength(20)]
        public string NivelProficiencia { get; set; } // BASICO, INTERMEDIARIO, AVANCADO

        // Navigation properties
        public virtual ICollection<UsuarioHabilidade> UsuarioHabilidades { get; set; }
    }
}