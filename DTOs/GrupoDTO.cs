using System.Collections.Generic;

namespace FiapConnect.DTOs
{
    public class GrupoDTO
    {
        public int IdGrupo { get; set; }
        public string NomeGrupo { get; set; }
        public string DescricaoProjeto { get; set; }
        public string DisciplinaTema { get; set; }
        public int MaxIntegrantes { get; set; }
        public int IntegrantesAtuais { get; set; }
        public string StatusGrupo { get; set; }

        // HATEOAS Links
        public Dictionary<string, string> Links { get; set; }
    }
}