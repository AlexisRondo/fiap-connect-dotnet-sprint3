namespace FiapConnect.Models
{
    public class UsuarioHabilidade
    {
        public int IdUsuarioHabilidade { get; set; }
        public int IdUsuario { get; set; }
        public int IdHabilidade { get; set; }

        // Navigation properties
        public virtual Usuario Usuario { get; set; }
        public virtual Habilidade Habilidade { get; set; }
    }
}