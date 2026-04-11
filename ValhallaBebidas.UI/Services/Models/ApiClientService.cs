using System.Net.Http;
using System.Net.Http.Headers;

namespace SenacBuy.UI.Services.Models
{
    /// <summary>
    /// Classe central que configura e reutiliza o HttpClient para comunicação com a SenacBuy.API.
    ///
    /// Por que static + singleton?
    ///   HttpClient não deve ser instanciado e descartado a cada requisição — isso causa
    ///   esgotamento de sockets (socket exhaustion). Usar um único cliente por toda a aplicação
    ///   é a prática correta em WinForms.
    ///
    /// Base URL padrão: http://localhost:5000/api/
    /// Altere ApiBaseUrl caso a API rode em porta diferente.
    /// </summary>
    public static class ApiClientService
    {
        // ──────────────────────────────────────────────────────────────────────────────
        // CONFIGURAÇÃO: ajuste esta URL se a API estiver em porta diferente
        // ──────────────────────────────────────────────────────────────────────────────
        public const string ApiBaseUrl = "http://localhost:5156/";

        // Cliente HTTP compartilhado por toda a aplicação (padrão recomendado pelo .NET)
        private static readonly HttpClient _httpClient = CriarHttpClient();

        /// <summary>
        /// Retorna o HttpClient configurado e pronto para uso.
        /// </summary>
        public static HttpClient Cliente => _httpClient;

        /// <summary>
        /// Cria e configura o HttpClient com BaseAddress e headers JSON.
        /// </summary>
        private static HttpClient CriarHttpClient()
        {
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(ApiBaseUrl);

            // Informamos à API que esperamos resposta em JSON
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Timeout de 30 segundos para evitar travamentos na UI
            cliente.Timeout = TimeSpan.FromSeconds(30);

            return cliente;
        }
    }
}
