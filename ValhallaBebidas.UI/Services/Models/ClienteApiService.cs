using SenacBuy.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Serviço responsável pelas chamadas HTTP ao endpoint de Cliente da API.
    ///
    /// Endpoints utilizados:
    ///   GET    api/cliente         — listar todos
    ///   GET    api/cliente/{id}    — buscar por id
    ///   POST   api/cliente         — criar novo
    ///   PUT    api/cliente/{id}    — atualizar
    ///   DELETE api/cliente/{id}    — remover
    /// </summary>
    public class ClienteApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        // ──────────────────────────────────────────────────────────────────────────────
        // LISTAR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<List<ClienteDto>> GetClientesAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<ClienteDto>>("api/cliente");
                return lista ?? new List<ClienteDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar os clientes.\nVerifique se a API está rodando em {ApiClientService.ApiBaseUrl}\n\nDetalhe: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<ClienteDto>();
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // BUSCAR POR ID
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<ClienteDto?> GetClienteByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<ClienteDto>($"api/cliente/{id}");
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro ao buscar cliente: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CRIAR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<ClienteDto?> CreateClienteAsync(string nome, string cpf)
        {
            try
            {
                var payload  = new CriarClienteDto { Nome = nome, CPF = cpf };
                var response = await _http.PostAsJsonAsync("api/cliente", payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ClienteDto>();

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Criar Cliente",
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

        public async Task<bool> UpdateClienteAsync(int id, string nome, string cpf)
        {
            try
            {
                var payload  = new AtualizarClienteDto { Nome = nome, CPF = cpf };
                var response = await _http.PutAsJsonAsync($"api/cliente/{id}", payload);

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Atualizar Cliente",
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

        public async Task<bool> DeleteClienteAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/cliente/{id}");

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Excluir Cliente",
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
    }
}
