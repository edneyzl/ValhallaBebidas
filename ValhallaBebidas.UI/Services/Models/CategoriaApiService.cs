using System.Net.Http.Json;
using ValhallaBebidas.UI.Services.Models;

namespace SenacBuy.UI.Services.Models
{
    /// <summary>
    /// Serviço responsável pela chamada HTTP ao endpoint de Categorias da API.
    ///
    /// Endpoint utilizado:
    ///   GET  /api/categorias   — listar todas as categorias
    /// </summary>
    public class CategoriaApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        /// <summary>
        /// Busca a lista de categorias da API.
        /// Retorna lista vazia em caso de erro de conexão.
        /// </summary>
        public async Task<List<CategoriaDto>> GetCategoriasAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias");
                return lista ?? new List<CategoriaDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar as categorias.\n" +
                    $"Verifique se a API está rodando em {ApiClientService.ApiBaseUrl}\n\n" +
                    $"Detalhe: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new List<CategoriaDto>();
            }
        }
    }
}
