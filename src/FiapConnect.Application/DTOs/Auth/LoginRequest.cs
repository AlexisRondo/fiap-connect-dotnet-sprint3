namespace FiapConnect.Application.DTOs.Auth;

// Payload de login. O cliente envia o idToken obtido do Firebase Auth
// O AuthService valida esse token via IFirebaseAuthClient, extrai o RM
// do email retornado, e emite o JWT proprio do .NET
public class LoginRequest
{
    public string IdToken { get; set; } = string.Empty;
}