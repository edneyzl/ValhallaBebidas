using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucDashboard : UserControl
    {
        private readonly DashboardApiService _dashService = new();
        private DashboardDto? _dados;

        public ucDashboard()
        {
            InitializeComponent();
            lblData.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy", new CultureInfo("pt-BR"));

            // Associa Paint do painel preenchível para desenhar o gráfico
            panelFill.Paint += PanelFill_Paint;

            Load += async (s, e) => await CarregarDashboardAsync();
        }

        private async Task CarregarDashboardAsync()
        {
            _dados = await _dashService.GetDashboardAsync();
            if (_dados == null) return;

            // Atualiza cards KPI
            lblValClientes.Text = _dados.TotalClientes.ToString();
            lblValProdutos.Text = _dados.TotalProdutos.ToString();
            lblValPedidos.Text = _dados.TotalPedidos.ToString();
            lblValFatur.Text = _dados.TotalVendas.ToString("C0", new CultureInfo("pt-BR"));

            // Atualiza rótulo de atividade com status de pedidos
            lblAtividade.Text = $"Pendentes: {_dados.PedidosPendentes}  |  " +
                                $"Finalizados: {_dados.PedidosConcluidos}  |  " +
                                $"Cancelados: {_dados.PedidosCancelados}";

            // Redesenha o gráfico
            panelFill.Invalidate();
        }

        private void PanelFill_Paint(object? sender, PaintEventArgs e)
        {
            if (_dados == null || _dados.PedidosPorMes.Count == 0) return;

            var g = e.Graphics;
            var dados = _dados.PedidosPorMes;
            int max = dados.Max(d => d.Total);
            if (max == 0) return;

            int barW = 40;
            int barGap = 30;
            int startX = 50;
            int baseY = panelFill.Height - 50;
            int maxH = panelFill.Height - 100;

            var corBarra = Color.FromArgb(214, 189, 119);
            var brushBarra = new SolidBrush(corBarra);
            var brushText = new SolidBrush(Color.FromArgb(214, 189, 119));
            var font = new Font("Segoe UI", 8F);

            // Título do gráfico
            g.DrawString("Pedidos por Mês (últimos 6 meses)",
                new Font("Segoe UI", 10F, FontStyle.Bold),
                new SolidBrush(Color.FromArgb(214, 189, 119)),
                new PointF(startX, 10));

            var meses = new[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun",
                                "Jul", "Ago", "Set", "Out", "Nov", "Dez" };

            for (int i = 0; i < dados.Count; i++)
            {
                int x = startX + i * (barW + barGap);
                int altura = (int)((double)dados[i].Total / max * maxH);
                int y = baseY - altura;

                // Barra
                g.FillRectangle(brushBarra, x, y, barW, altura);

                // Valor acima da barra
                g.DrawString(dados[i].Total.ToString(), font, brushText,
                    new PointF(x + 2, y - 16));

                // Rótulo do mês abaixo
                string mesLabel = meses[dados[i].Mes - 1];
                g.DrawString(mesLabel, font, brushText,
                    new PointF(x + 5, baseY + 5));
            }
        }

        private void ConfigurarInterface() { /* utilizado pelo Designer */ }

    }
}
