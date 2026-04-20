using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SenacBuy.UI.Services.Models;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucFuncionarios : UserControl
    {
        private readonly FuncionarioApiService _usuarioService = new();
        private List<FuncionarioDto> _usuarios = new();

        public ucFuncionarios()
        {
            InitializeComponent();
            ConfigurarInterface();
            Load += async (s, e) => await CarregarUsuariosAsync();
        }

        private void ConfigurarInterface()
        {
            dgvFuncionarios.RowTemplate.Height = 50; // Aumentar altura da linha para caber a foto

            dgvFuncionarios.Columns.Add(new DataGridViewImageColumn
            {
                Name = "colFoto",
                HeaderText = "Foto",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                FillWeight = 40
            });
            dgvFuncionarios.Columns.Add("colId", "ID");
            dgvFuncionarios.Columns.Add("colNome", "Nome");
            dgvFuncionarios.Columns.Add("colEmail", "E-mail");

            dgvFuncionarios.Columns["colId"]!.FillWeight = 30;
            dgvFuncionarios.Columns["colNome"]!.FillWeight = 200;
            dgvFuncionarios.Columns["colEmail"]!.FillWeight = 200;
        }

        public async Task CarregarUsuariosAsync(string filtro = "")
        {
            if (_usuarios.Count == 0 || string.IsNullOrEmpty(filtro))
                _usuarios = await _usuarioService.ListarUsuariosAsync();

            AtualizarGrid(_usuarios, filtro);
        }

        private void AtualizarGrid(List<FuncionarioDto> lista, string filtro = "")
        {
            dgvFuncionarios.Rows.Clear();

            var exibidos = string.IsNullOrWhiteSpace(filtro)
                ? lista
                : lista.Where(u =>
                    u.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var u in exibidos)
            {
                int rowIndex = dgvFuncionarios.Rows.Add(null, u.Id, u.Nome, u.Email);
                _ = CarregarImagemAsync(rowIndex, u.FotoPerfil);
            }
        }

        private async Task CarregarImagemAsync(int rowIndex, string? caminhoRelativo)
        {
            if (string.IsNullOrEmpty(caminhoRelativo)) return;
            try
            {
                // Constrói URL: http://localhost:5086/api/imagens/usuarios/nome.jpg
                var url = $"{ApiClientService.ApiBaseUrl.TrimEnd('/')}/api/imagens/{caminhoRelativo}";
                using var stream = await ApiClientService.Cliente.GetStreamAsync(url);
                var img = System.Drawing.Image.FromStream(stream);

                if (dgvFuncionarios.Rows.Count > rowIndex)
                    dgvFuncionarios.Rows[rowIndex].Cells["colFoto"].Value = img;
            }
            catch { /* Ignora erro de carregamento (ex: 404, sem rede) */ }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid(_usuarios, txtBuscaUsuario.Text);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoFuncionario());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (dgvFuncionarios.CurrentRow == null)
            {
                MessageBox.Show("Selecione um usuário para editar.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvFuncionarios.CurrentRow.Cells["colId"].Value);
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoFuncionario(id));
        }

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dgvFuncionarios.CurrentRow == null)
            {
                MessageBox.Show("Selecione um usuário para excluir.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvFuncionarios.CurrentRow.Cells["colId"].Value);
            string nome = dgvFuncionarios.CurrentRow.Cells["colNome"].Value?.ToString() ?? "";

            if (MessageBox.Show(
                    $"Excluir o usuário \"{nome}\"?\nEsta ação não pode ser desfeita.",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            bool ok = await _usuarioService.ExcluirFuncionarioAsync(id);
            if (ok)
            {
                MessageBox.Show("Usuário excluído com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _usuarios.Clear();
                await CarregarUsuariosAsync();
            }
        }
    }
}
