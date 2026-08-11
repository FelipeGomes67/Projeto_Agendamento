namespace AgendamentoAPI.Utils;

public class Criptografia
{
    public static string GerarHashSenha(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }
    public static bool VerificarSenha(string senhaDigitada, string senhaHashBanco)
    {
        return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaHashBanco);
    }
}
