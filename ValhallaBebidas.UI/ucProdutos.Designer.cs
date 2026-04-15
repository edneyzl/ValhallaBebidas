namespace ValhallaBebidas.UI
{
    partial class ucProdutos
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            txtBuscaProduto = new Guna.UI2.WinForms.Guna2TextBox();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            btnNovoProduto = new Guna.UI2.WinForms.Guna2Button();
            btnEditarProduto = new Guna.UI2.WinForms.Guna2Button();
            btnExcluirProduto = new Guna.UI2.WinForms.Guna2Button();
            dgvProdutos = new Guna.UI2.WinForms.Guna2DataGridView();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProdutos).BeginInit();
            SuspendLayout();
            // 
            // txtBuscaProduto
            // 
            txtBuscaProduto.BackColor = Color.FromArgb(33, 33, 33);
            txtBuscaProduto.BorderColor = Color.FromArgb(214, 189, 119);
            txtBuscaProduto.BorderRadius = 12;
            txtBuscaProduto.CustomizableEdges = customizableEdges1;
            txtBuscaProduto.DefaultText = "";
            txtBuscaProduto.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtBuscaProduto.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtBuscaProduto.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtBuscaProduto.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtBuscaProduto.FillColor = Color.FromArgb(33, 33, 33);
            txtBuscaProduto.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtBuscaProduto.Font = new Font("Segoe UI", 9F);
            txtBuscaProduto.ForeColor = Color.FromArgb(214, 189, 119);
            txtBuscaProduto.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtBuscaProduto.Location = new Point(277, 24);
            txtBuscaProduto.Name = "txtBuscaProduto";
            txtBuscaProduto.PlaceholderForeColor = Color.FromArgb(214, 189, 119);
            txtBuscaProduto.PlaceholderText = "🔍  Pesquisar produto pelo nome...";
            txtBuscaProduto.SelectedText = "";
            txtBuscaProduto.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtBuscaProduto.Size = new Size(263, 36);
            txtBuscaProduto.TabIndex = 4;
            txtBuscaProduto.TextChanged += txtBuscaProduto_TextChanged_1;
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.FromArgb(33, 33, 33);
            guna2Panel1.Controls.Add(lblTitulo);
            guna2Panel1.Controls.Add(lblSubtitulo);
            guna2Panel1.Controls.Add(btnNovoProduto);
            guna2Panel1.Controls.Add(txtBuscaProduto);
            guna2Panel1.Controls.Add(btnEditarProduto);
            guna2Panel1.Controls.Add(btnExcluirProduto);
            guna2Panel1.CustomizableEdges = customizableEdges9;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2Panel1.Size = new Size(969, 80);
            guna2Panel1.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Century Gothic", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(214, 189, 119);
            lblTitulo.Location = new Point(17, 13);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(225, 34);
            lblTitulo.TabIndex = 12;
            lblTitulo.Text = "📦  Gestão de Produtos";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(214, 189, 119);
            lblSubtitulo.Location = new Point(14, 45);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(245, 20);
            lblSubtitulo.TabIndex = 13;
            lblSubtitulo.Text = "Gerencie os produtos cadastrados no sistema";
            // 
            // btnNovoProduto
            // 
            btnNovoProduto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovoProduto.BorderRadius = 10;
            btnNovoProduto.Cursor = Cursors.Hand;
            btnNovoProduto.CustomizableEdges = customizableEdges3;
            btnNovoProduto.FillColor = Color.FromArgb(214, 189, 119);
            btnNovoProduto.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNovoProduto.ForeColor = Color.Black;
            btnNovoProduto.Location = new Point(562, 22);
            btnNovoProduto.Name = "btnNovoProduto";
            btnNovoProduto.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnNovoProduto.Size = new Size(155, 40);
            btnNovoProduto.TabIndex = 9;
            btnNovoProduto.Text = "+ Novo Produto";
            btnNovoProduto.Click += btnNovoProduto_Click;
            // 
            // btnEditarProduto
            // 
            btnEditarProduto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditarProduto.BorderRadius = 10;
            btnEditarProduto.Cursor = Cursors.Hand;
            btnEditarProduto.CustomizableEdges = customizableEdges5;
            btnEditarProduto.FillColor = Color.FromArgb(64, 64, 64);
            btnEditarProduto.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEditarProduto.ForeColor = Color.White;
            btnEditarProduto.Location = new Point(723, 22);
            btnEditarProduto.Name = "btnEditarProduto";
            btnEditarProduto.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditarProduto.Size = new Size(110, 40);
            btnEditarProduto.TabIndex = 10;
            btnEditarProduto.Text = "✏️ Editar";
            btnEditarProduto.Click += btnEditarProduto_Click_1;
            // 
            // btnExcluirProduto
            // 
            btnExcluirProduto.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcluirProduto.BorderRadius = 10;
            btnExcluirProduto.Cursor = Cursors.Hand;
            btnExcluirProduto.CustomizableEdges = customizableEdges7;
            btnExcluirProduto.FillColor = Color.FromArgb(220, 60, 60);
            btnExcluirProduto.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnExcluirProduto.ForeColor = Color.White;
            btnExcluirProduto.Location = new Point(839, 22);
            btnExcluirProduto.Name = "btnExcluirProduto";
            btnExcluirProduto.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnExcluirProduto.Size = new Size(110, 40);
            btnExcluirProduto.TabIndex = 11;
            btnExcluirProduto.Text = "🗑️ Excluir";
            btnExcluirProduto.Click += btnExcluirProduto_Click_1;
            // 
            // dgvProdutos
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvProdutos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvProdutos.BackgroundColor = Color.Black;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvProdutos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvProdutos.ColumnHeadersHeight = 4;
            dgvProdutos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvProdutos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvProdutos.GridColor = Color.FromArgb(231, 229, 255);
            dgvProdutos.Location = new Point(19, 86);
            dgvProdutos.Name = "dgvProdutos";
            dgvProdutos.RowHeadersVisible = false;
            dgvProdutos.Size = new Size(930, 450);
            dgvProdutos.TabIndex = 1;
            dgvProdutos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvProdutos.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvProdutos.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvProdutos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvProdutos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvProdutos.ThemeStyle.BackColor = Color.Black;
            dgvProdutos.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvProdutos.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvProdutos.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProdutos.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvProdutos.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvProdutos.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvProdutos.ThemeStyle.HeaderStyle.Height = 4;
            dgvProdutos.ThemeStyle.ReadOnly = false;
            dgvProdutos.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvProdutos.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProdutos.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvProdutos.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvProdutos.ThemeStyle.RowsStyle.Height = 25;
            dgvProdutos.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvProdutos.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // ucProdutos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            Controls.Add(dgvProdutos);
            Controls.Add(guna2Panel1);
            Name = "ucProdutos";
            Size = new Size(969, 549);
            guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProdutos).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2TextBox txtBuscaProduto;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnNovoProduto;
        private Guna.UI2.WinForms.Guna2Button btnEditarProduto;
        private Guna.UI2.WinForms.Guna2Button btnExcluirProduto;
        private Guna.UI2.WinForms.Guna2DataGridView dgvProdutos;
        private Label lblTitulo;
        private Label lblSubtitulo;
    }
}
