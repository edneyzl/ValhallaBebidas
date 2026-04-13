using SenacBuy.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Serviço responsável pelas chamadas HTTP ao endpoint de Pedido da API.
    ///
    /// Endpoints utilizados:
    ///   GET    api/pedido          — listar todos
    ///   GET    api/pedido/{id}     — buscar por id
    ///   POST   api/pedido          — criar novo pedido
    ///   DELETE api/pedido/{id}     — remover pedido
    /// </summary>
    public class PedidoApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        // ──────────────────────────────────────────────────────────────────────────────
        // LISTAR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<List<PedidoDto>> GetPedidosAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<PedidoDto>>("api/pedido");
                return lista ?? new List<PedidoDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar os pedidos.\nVerifique se a API está rodando em {ApiClientService.ApiBaseUrl}\n\nDetalhe: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<PedidoDto>();
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // BUSCAR POR ID
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<PedidoDto?> GetPedidoByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<PedidoDto>($"api/pedido/{id}");
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro ao buscar pedido: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CRIAR
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Cria um novo pedido na API com os itens indicados.
        /// </summary>
        public async Task<PedidoDto?> CreatePedidoAsync(int clienteId, List<CriarItemPedidoDto> itens, string status = "Pendente")
        {
            try
            {
                var payload  = new CriarPedidoDto { ClienteId = clienteId, Itens = itens, Status = status };
                var response = await _http.PostAsJsonAsync("api/pedido", payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<PedidoDto>();

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Criar Pedido",
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
        // EXCLUIR
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<bool> DeletePedidoAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/pedido/{id}");

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Excluir Pedido",
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
        // ATUALIZAR (PUT /api/pedido/{id})
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Atualiza um pedido existente via PUT /api/pedido/{id}.
        /// Retorna o PedidoDto atualizado ou null em caso de falha.
        /// </summary>
        public async Task<PedidoDto?> UpdatePedidoAsync(int id, int clienteId, List<CriarItemPedidoDto> itens, string status = "Pendente")
        {
            try
            {
                var payload  = new AtualizarPedidoDto { ClienteId = clienteId, Itens = itens, Status = status };
                var response = await _http.PutAsJsonAsync($"api/pedido/{id}", payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<PedidoDto>();

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Atualizar Pedido",
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
