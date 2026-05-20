namespace FiapConnect.Application.DTOs.Notificacao;

public class NotificacaoResponse
{
    public string? Id { get; set; }
    public string RmDestinatario { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataEnvio { get; set; }
    public bool Lida { get; set; }
    public DateTime? DataLeitura { get; set; }
    public Dictionary<string, object>? DadosContexto { get; set; }
    public string Prioridade { get; set; } = string.Empty;
    public string Origem { get; set; } = string.Empty;
}