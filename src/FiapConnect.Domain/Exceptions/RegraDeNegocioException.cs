namespace FiapConnect.Domain.Exceptions;

// Lancada quando uma regra de negocio eh violada (enviar mensagem em conversa
// encerrada, remetente nao eh participante da conversa, criar conversa com
// apenas 1 participante, etc)
// O middleware global mapeia para HTTP 400 Bad Request
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}