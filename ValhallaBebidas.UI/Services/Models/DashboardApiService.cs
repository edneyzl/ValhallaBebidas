using SenacBuy.UI.Services.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Serviço de comunicação com GET /api/dashboard.
    /// Retorna dados agregados (KPIs) para exibição no Dashboard.
    /// </summary>
    public class DashboardApiService
    {
        private readonly HttpClient _http = ApiClientService.Cliente;

        /// <summary>
        /// Busca os dados do dashboard na API.
        /// Retorna null em caso de falha de conexão.
        /// </summary>
        public async Task<DashboardDto?> GetDashboardAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<DashboardDto>("api/dashboard");
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar o dashboard.\n{ex.Message}",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }
    }
}
