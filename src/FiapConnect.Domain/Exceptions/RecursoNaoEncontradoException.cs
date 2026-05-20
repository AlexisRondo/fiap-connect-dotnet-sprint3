namespace FiapConnect.Domain.Exceptions;

// Lancada quando um recurso solicitado nao existe (RM inexistente,
// conversa inexistente, notificacao inexistente, historico inexistente)
// O middleware global mapeia para HTTP 404 Not Found
public class RecursoNaoEncontradoException : Exception
{
    public RecursoNaoEncontradoException(string mensagem) : base(mensagem)
    {
    }
}