using System.Collections.Generic;

namespace FiapConnect.DTOs
{
    public class HabilidadeDTO
    {
        public int IdHabilidade { get; set; }
        public string NomeHabilidade { get; set; }
        public string TipoHabilidade { get; set; }
        public string NivelProficiencia { get; set; }

        // HATEOAS Links
        public Dictionary<string, string> Links { get; set; }
    }
}