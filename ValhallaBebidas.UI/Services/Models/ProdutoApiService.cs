using SenacBuy.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Serviço responsável pelas chamadas HTTP ao endpoint de Produto da API.
    ///
    /// Endpoints utilizados:
    ///   GET    api/produto         — listar todos
    ///   GET    api/produto/{id}    — buscar por id
    ///   POST   api/produto         — criar novo
    ///   PUT    api/produto/{id}    — atualizar
    ///   DELETE api/produto/{id}    — remover
    /// </summary>
    public class ProdutoApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        // ──────────────────────────────────────────────────────────────────────────────
        // LISTAR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<List<ProdutoDto>> GetProdutosAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<ProdutoDto>>("api/produto");
                return lista ?? new List<ProdutoDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar os produtos.\nVerifique se a API está rodando em {ApiClientService.ApiBaseUrl}\n\nDetalhe: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<ProdutoDto>();
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // BUSCAR POR ID
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<ProdutoDto?> GetProdutoByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<ProdutoDto>($"api/produto/{id}");
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro ao buscar produto: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CRIAR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<ProdutoDto?> CreateProdutoAsync(string nome, decimal preco, string? fotoProduto = null, int? categoriaId = null)
        {
            try
            {
                var payload  = new CriarProdutoDto { Nome = nome, Preco = preco, FotoProduto = fotoProduto, CategoriaId = categoriaId };
                var response = await _http.PostAsJsonAsync("api/produto", payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ProdutoDto>();

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Criar Produto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // ATUALIZAR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<bool> UpdateProdutoAsync(int id, string nome, decimal preco, string? fotoProduto = null, int? categoriaId = null)
        {
            try
            {
                var payload  = new AtualizarProdutoDto { Nome = nome, Preco = preco, FotoProduto = fotoProduto, CategoriaId = categoriaId };
                var response = await _http.PutAsJsonAsync($"api/produto/{id}", payload);

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Atualizar Produto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // EXCLUIR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<bool> DeleteProdutoAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/produto/{id}");

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Excluir Produto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // AUXILIAR
        // ──────────────────────────────────────────────────────────────────────────────

        private static string ExtrairMensagemErro(string json)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("mensagem", out var m))
                    return m.GetString() ?? json;
            }
            catch { }
            return json;
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // UPLOAD DE FOTO
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<string?> UploadFotoAsync(string filePath)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var streamContent = new StreamContent(fileStream);
                var contentType = Path.GetExtension(filePath).ToLowerInvariant() == ".png" ? "image/png" : "image/jpeg";
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                form.Add(streamContent, "file", Path.GetFileName(filePath));

                var response = await _http.PostAsync("api/upload/produto", form);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UploadResponseDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result?.Caminho;
                }

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro no Upload", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
