namespace ValhallaBebidas.UI
{
    partial class ucClientes
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panelHeader = new Guna.UI2.WinForms.Guna2Panel();
            lblSubtitulo = new Label();
            txtBuscaCliente = new Guna.UI2.WinForms.Guna2TextBox();
            btnNovoCliente = new Guna.UI2.WinForms.Guna2Button();
            lblTitulo = new Label();
            btnEditarCliente = new Guna.UI2.WinForms.Guna2Button();
            btnExcluirCliente = new Guna.UI2.WinForms.Guna2Button();
            panelConteudo = new Guna.UI2.WinForms.Guna2Panel();
            dgvClientes = new Guna.UI2.WinForms.Guna2DataGridView();
            panelHeader.SuspendLayout();
            panelConteudo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(33, 33, 33);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(txtBuscaCliente);
            panelHeader.Controls.Add(btnNovoCliente);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(btnEditarCliente);
            panelHeader.Controls.Add(btnExcluirCliente);
            panelHeader.CustomizableEdges = customizableEdges9;
            panelHeader.ForeColor = SystemColors.ControlText;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.ShadowDecoration.CustomizableEdges = customizableEdges10;
            panelHeader.Size = new Size(1165, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.Font = new Font("Sora Light", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = Color.FromArgb(214, 189, 119);
            lblSubtitulo.Location = new Point(12, 42);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(276, 20);
            lblSubtitulo.TabIndex = 3;
            lblSubtitulo.Text = "Gerencie os clientes cadastrados no sistema";
            // 
            // txtBuscaCliente
            // 
            txtBuscaCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtBuscaCliente.BorderColor = Color.FromArgb(214, 189, 119);
            txtBuscaCliente.BorderRadius = 8;
            txtBuscaCliente.CustomizableEdges = customizableEdges1;
            txtBuscaCliente.DefaultText = "";
            txtBuscaCliente.FillColor = Color.FromArgb(33, 33, 33);
            txtBuscaCliente.FocusedState.BorderColor = Color.FromArgb(0, 123, 204);
            txtBuscaCliente.Font = new Font("Sora", 9.749999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscaCliente.ForeColor = Color.FromArgb(214, 189, 119);
            txtBuscaCliente.Location = new Point(408, 15);
            txtBuscaCliente.Name = "txtBuscaCliente";
            txtBuscaCliente.PlaceholderForeColor = Color.FromArgb(214, 189, 119);
            txtBuscaCliente.PlaceholderText = "🔍  Pesquisar por E-mail ou CPF...";
            txtBuscaCliente.SelectedText = "";
            txtBuscaCliente.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtBuscaCliente.Size = new Size(248, 45);
            txtBuscaCliente.TabIndex = 9;
            txtBuscaCliente.TextChanged += txtBuscaCliente_TextChanged;
            // 
            // btnNovoCliente
            // 
            btnNovoCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovoCliente.BorderRadius = 10;
            btnNovoCliente.Cursor = Cursors.Hand;
            btnNovoCliente.CustomizableEdges = customizableEdges3;
            btnNovoCliente.FillColor = Color.FromArgb(214, 189, 119);
            btnNovoCliente.Font = new Font("Sora", 9.749999F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNovoCliente.ForeColor = Color.Black;
            btnNovoCliente.Location = new Point(746, 19);
            btnNovoCliente.Name = "btnNovoCliente";
            btnNovoCliente.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnNovoCliente.Size = new Size(155, 40);
            btnNovoCliente.TabIndex = 6;
            btnNovoCliente.Text = "+ Novo Cliente";
            btnNovoCliente.Click += btnNovoCliente_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Sora", 14.2499981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(214, 189, 119);
            lblTitulo.Location = new Point(8, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(238, 34);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "👥  Gestão de Clientes";
            // 
            // btnEditarCliente
            // 
            btnEditarCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditarCliente.BorderRadius = 10;
            btnEditarCliente.Cursor = Cursors.Hand;
            btnEditarCliente.CustomizableEdges = customizableEdges5;
            btnEditarCliente.FillColor = Color.FromArgb(64, 64, 64);
            btnEditarCliente.Font = new Font("Sora", 9.749999F, FontStyle.Bold);
            btnEditarCliente.ForeColor = Color.White;
            btnEditarCliente.Location = new Point(908, 20);
            btnEditarCliente.Name = "btnEditarCliente";
            btnEditarCliente.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditarCliente.Size = new Size(110, 40);
            btnEditarCliente.TabIndex = 7;
            btnEditarCliente.Text = "✏️ Editar";
            btnEditarCliente.Click += btnEditarCliente_Click;
            // 
            // btnExcluirCliente
            // 
            btnExcluirCliente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcluirCliente.BorderRadius = 10;
            btnExcluirCliente.Cursor = Cursors.Hand;
            btnExcluirCliente.CustomizableEdges = customizableEdges7;
            btnExcluirCliente.FillColor = Color.FromArgb(220, 60, 60);
            btnExcluirCliente.Font = new Font("Sora", 9.749999F, FontStyle.Bold);
            btnExcluirCliente.ForeColor = Color.White;
            btnExcluirCliente.Location = new Point(1025, 20);
            btnExcluirCliente.Name = "btnExcluirCliente";
            btnExcluirCliente.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnExcluirCliente.Size = new Size(110, 40);
            btnExcluirCliente.TabIndex = 8;
            btnExcluirCliente.Text = "🗑️ Excluir";
            btnExcluirCliente.Click += btnExcluirCliente_Click;
            // 
            // panelConteudo
            // 
            panelConteudo.BackColor = Color.FromArgb(64, 64, 64);
            panelConteudo.Controls.Add(dgvClientes);
            panelConteudo.CustomizableEdges = customizableEdges11;
            panelConteudo.Location = new Point(0, 79);
            panelConteudo.Name = "panelConteudo";
            panelConteudo.ShadowDecoration.CustomizableEdges = customizableEdges12;
            panelConteudo.Size = new Size(1165, 471);
            panelConteudo.TabIndex = 1;
            // 
            // dgvClientes
            // 
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvClientes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvClientes.BackgroundColor = Color.Black;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvClientes.ColumnHeadersHeight = 25;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvClientes.DefaultCellStyle = dataGridViewCellStyle3;
            dgvClientes.GridColor = Color.FromArgb(231, 229, 255);
            dgvClientes.Location = new Point(0, 0);
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.RowHeadersVisible = false;
            dgvClientes.Size = new Size(1165, 471);
            dgvClientes.TabIndex = 0;
            dgvClientes.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvClientes.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvClientes.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvClientes.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvClientes.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvClientes.ThemeStyle.BackColor = Color.Black;
            dgvClientes.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvClientes.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvClientes.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvClientes.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvClientes.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvClientes.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvClientes.ThemeStyle.HeaderStyle.Height = 25;
            dgvClientes.ThemeStyle.ReadOnly = true;
            dgvClientes.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvClientes.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvClientes.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvClientes.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvClientes.ThemeStyle.RowsStyle.Height = 25;
            dgvClientes.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvClientes.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // ucClientes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelConteudo);
            Controls.Add(panelHeader);
            Name = "ucClientes";
            Size = new Size(1165, 550);
            panelHeader.ResumeLayout(false);
            panelConteudo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClientes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelHeader;
        private Guna.UI2.WinForms.Guna2Button btnNovoCliente;
        private Label lblTitulo;
        private Guna.UI2.WinForms.Guna2Button btnEditarCliente;
        private Guna.UI2.WinForms.Guna2Button btnExcluirCliente;
        private Label lblSubtitulo;
        private Guna.UI2.WinForms.Guna2Panel panelConteudo;
        private Guna.UI2.WinForms.Guna2DataGridView dgvClientes;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscaCliente;
    }
}
