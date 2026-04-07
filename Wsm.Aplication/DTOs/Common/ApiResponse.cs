namespace Wsm.Aplication.DTOs.Common;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public T? Dados { get; set; }
    public List<string> Erros { get; set; }
    public DateTime Timestamp { get; set; }

    public ApiResponse()
    {
        Erros = new List<string>();
        Timestamp = DateTime.UtcNow;
    }

    public static ApiResponse<T> Success(T dados, string mensagem = "Operação realizada com sucesso")
    {
        return new ApiResponse<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            Dados = dados
        };
    }

    public static ApiResponse<T> Failure(string mensagem, List<string>? erros = null)
    {
        return new ApiResponse<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            Erros = erros ?? new List<string>()
        };
    }

    public static ApiResponse<T> Failure(string mensagem, string erro)
    {
        return new ApiResponse<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            Erros = new List<string> { erro }
        };
    }
}
