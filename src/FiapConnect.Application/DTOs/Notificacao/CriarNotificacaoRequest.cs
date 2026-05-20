namespace FiapConnect.Application.DTOs.Notificacao;

// Payload para criar uma notificacao. A Origem nao vem do cliente:
// o service fixa "DOTNET" para notificacoes criadas por este projeto
public class CriarNotificacaoRequest
{
    public string RmDestinatario { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public Dictionary<string, object>? DadosContexto { get; set; }
    public string Prioridade { get; set; } = "NORMAL";
}