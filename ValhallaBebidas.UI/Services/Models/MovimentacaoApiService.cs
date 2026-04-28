using SenacBuy.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI.Services
{
    /// <summary>
    /// Serviço responsável pelas chamadas HTTP ao endpoint de Movimentação da API.
    ///
    /// Endpoints utilizados:
    ///   GET    api/movimentacao
    ///   GET    api/movimentacao/{id}
    ///   GET    api/movimentacao/produto/{produtoId}
    ///   POST   api/movimentacao/entrada
    ///   POST   api/movimentacao/saida
    ///   POST   api/movimentacao/{id}/estornar
    ///   DELETE api/movimentacao/{id}
    /// </summary>
    public class MovimentacaoApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        // ─────────────────────────────────────────
        // LISTAR TODAS
        // ─────────────────────────────────────────
        public async Task<List<MovimentacaoDto>> GetMovimentacoesAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<MovimentacaoDto>>("api/movimentacao");
                return lista ?? new List<MovimentacaoDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar movimentações.\nAPI: {ApiClientService.ApiBaseUrl}\n\n{ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new List<MovimentacaoDto>();
            }
        }

        // ─────────────────────────────────────────
        // LISTAR POR PRODUTO
        // ─────────────────────────────────────────
        public async Task<List<MovimentacaoDto>> GetPorProdutoAsync(int produtoId)
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<MovimentacaoDto>>($"api/movimentacao/produto/{produtoId}");
                return lista ?? new List<MovimentacaoDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro ao carregar movimentações: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new List<MovimentacaoDto>();
            }
        }

        // ─────────────────────────────────────────
        // BUSCAR POR ID
        // ─────────────────────────────────────────
        public async Task<MovimentacaoDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<MovimentacaoDto>($"api/movimentacao/{id}");
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro ao buscar movimentação: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }

        // ─────────────────────────────────────────
        // ENTRADA
        // ─────────────────────────────────────────
        public async Task<MovimentacaoDto?> RegistrarEntradaAsync(CriarMovimentacaoDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/movimentacao/entrada", dto);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<MovimentacaoDto>();

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro),
                    "Erro ao Registrar Entrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return null;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }

        // ─────────────────────────────────────────
        // SAÍDA
        // ─────────────────────────────────────────
        public async Task<MovimentacaoDto?> RegistrarSaidaAsync(CriarMovimentacaoDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/movimentacao/saida", dto);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<MovimentacaoDto>();

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro),
                    "Erro ao Registrar Saída",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return null;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
        }

        // ─────────────────────────────────────────
        // ESTORNAR
        // ─────────────────────────────────────────
        public async Task<bool> EstornarAsync(int id)
        {
            try
            {
                var response = await _http.PostAsync($"api/movimentacao/{id}/estornar", null);

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro),
                    "Erro ao Estornar",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        // ─────────────────────────────────────────
        // REMOVER
        // ─────────────────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/movimentacao/{id}");

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro),
                    "Erro ao Remover",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        // ─────────────────────────────────────────
        // AUXILIAR
        // ─────────────────────────────────────────
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
    }
}