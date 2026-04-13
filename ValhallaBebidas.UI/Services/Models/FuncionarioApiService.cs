using SenacBuy.UI.Services.Models;
using System.Net.Http.Json;
using System.Text.Json;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Serviço responsável pelas chamadas HTTP ao endpoint de Usuário da API.
    ///
    /// Endpoints utilizados:
    ///   POST   api/usuario/login   — autenticar usuário
    ///   POST   api/usuario         — cadastrar novo usuário
    ///   GET    api/usuario         — listar todos os usuários
    ///   PUT    api/usuario/{id}    — atualizar nome/email de um usuário
    /// </summary>
    public class FuncionarioApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        // ──────────────────────────────────────────────────────────────────────────────
        // LOGIN
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Envia email e senha para a API e retorna o resultado da autenticação.
        /// Retorna null e exibe mensagem ao usuário se houver erro de comunicação.
        /// </summary>
        public async Task<LoginResponseDto?> LoginAsync(string email, string senha)
        {
            try
            {
                var payload = new LoginDto { Email = email, Senha = senha };
                var response = await _http.PostAsJsonAsync("api/funcionario/login", payload);

                // A API retorna 200 OK em caso de sucesso ou 401 Unauthorized em falha
                var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                return resultado;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Não foi possível conectar à API.\nVerifique se a API está rodando em {ApiClientService.ApiBaseUrl}\n\nDetalhe: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado no login: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CADASTRAR FUNCIONARIO
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Cadastra um novo usuário na API.
        /// Retorna o UsuarioDto criado ou null em caso de erro.
        /// </summary>
        public async Task<FuncionarioDto?> CadastrarFuncionarioAsync(string nome,string dataNasc, string cpf, string telefone, string email, string senha, string? fotoPerfil = null)
        {
            try
            {
                var payload = new CriarFuncionarioDto { Nome = nome, Email = email, DataNascimento = dataNasc, Cpf = cpf, Telefone = telefone, Senha = senha, FotoPerfil = fotoPerfil };
                var response = await _http.PostAsJsonAsync("api/funcionario", payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<FuncionarioDto>();

                // 409 Conflict: email já cadastrado
                var erro = await response.Content.ReadAsStringAsync();
                var msg  = ExtrairMensagemErro(erro);
                MessageBox.Show(msg, "Erro ao Cadastrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        // LISTAR FUNCIONARIOS
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Retorna a lista de todos os usuários cadastrados (sem senhas).
        /// </summary>
        public async Task<List<FuncionarioDto>> ListarUsuariosAsync()
        {
            try
            {
                var lista = await _http.GetFromJsonAsync<List<FuncionarioDto>>("api/funcionario");
                return lista ?? new List<FuncionarioDto>();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Sem conexão com a API: {ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<FuncionarioDto>();
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // BUSCAR POR ID
        // ──────────────────────────────────────────────────────────────────────────────

        public async Task<FuncionarioDto?> GetUsuarioByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<FuncionarioDto>($"api/funcionario/{id}");
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro ao buscar funcionario: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // ATUALIZAR FUNCIONARIO
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Atualiza nome e email de um usuário existente.
        /// Retorna true em caso de sucesso.
        /// </summary>
        public async Task<bool> AtualizarUsuarioAsync(FuncionarioDto dto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/funcionario/{dto.Id}", dto);

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Atualizar",
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
        // EXCLUIR FUNCIONARIO
        // ──────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Exclui um usuário existente.
        /// Retorna true em caso de sucesso.
        /// </summary>
        public async Task<bool> ExcluirFuncionarioAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/funcionario/{id}");

                if (response.IsSuccessStatusCode)
                    return true;

                var erro = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ExtrairMensagemErro(erro), "Erro ao Excluir",
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

        /// <summary>
        /// Tenta extrair a propriedade "mensagem" de uma resposta JSON de erro.
        /// Se não conseguir, retorna o texto bruto.
        /// </summary>
        private static string ExtrairMensagemErro(string json)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("mensagem", out var m))
                    return m.GetString() ?? json;
            }
            catch { /* não é JSON — retorna o texto bruto */ }
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

                var response = await _http.PostAsync("api/upload/funcionario", form);
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
