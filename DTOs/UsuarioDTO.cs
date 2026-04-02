using System.Collections.Generic;

namespace FiapConnect.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string RM { get; set; }
        public string NomeCompleto { get; set; }
        public string EmailInstitucional { get; set; }
        public string Curso { get; set; }
        public string Periodo { get; set; }
        public string Turma { get; set; }
        public bool StatusBusca { get; set; }

        // HATEOAS Links
        public Dictionary<string, string> Links { get; set; }
    }
}