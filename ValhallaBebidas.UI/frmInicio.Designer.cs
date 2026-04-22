namespace ValhallaBebidas.UI
{
    partial class frmInicio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            picFoto = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            lblNome = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnIniciar = new Guna.UI2.WinForms.Guna2Button();
            btnSair = new Guna.UI2.WinForms.Guna2Button();
            panelBack2 = new Guna.UI2.WinForms.Guna2Panel();
            panelBack1 = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)picFoto).BeginInit();
            panelBack2.SuspendLayout();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // picFoto
            // 
            picFoto.BackColor = Color.Transparent;
            picFoto.FillColor = Color.FromArgb(64, 64, 64);
            picFoto.Image = Properties.Resources.user1;
            picFoto.ImageRotate = 0F;
            picFoto.InitialImage = null;
            picFoto.Location = new Point(502, 53);
            picFoto.Name = "picFoto";
            picFoto.ShadowDecoration.CustomizableEdges = customizableEdges3;
            picFoto.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            picFoto.Size = new Size(150, 150);
            picFoto.SizeMode = PictureBoxSizeMode.Zoom;
            picFoto.TabIndex = 1;
            picFoto.TabStop = false;
            // 
            // lblNome
            // 
            lblNome.BackColor = Color.Transparent;
            lblNome.Font = new Font("Sora ExtraBold", 21.7499962F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.ForeColor = Color.White;
            lblNome.Location = new Point(480, 211);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(192, 38);
            lblNome.TabIndex = 2;
            lblNome.Text = "Olá, Usuário.";
            lblNome.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnIniciar
            // 
            btnIniciar.BackColor = Color.Transparent;
            btnIniciar.BorderRadius = 15;
            btnIniciar.Cursor = Cursors.Hand;
            btnIniciar.CustomizableEdges = customizableEdges6;
            btnIniciar.DisabledState.BorderColor = Color.DarkGray;
            btnIniciar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnIniciar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnIniciar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnIniciar.FillColor = Color.FromArgb(214, 189, 119);
            btnIniciar.Font = new Font("Sora", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnIniciar.ForeColor = Color.Black;
            btnIniciar.HoverState.FillColor = Color.FromArgb(242, 215, 134);
            btnIniciar.Location = new Point(432, 275);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.ShadowDecoration.CustomizableEdges = customizableEdges7;
            btnIniciar.Size = new Size(290, 60);
            btnIniciar.TabIndex = 3;
            btnIniciar.Text = "Iniciar Sistema";
            btnIniciar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // btnSair
            // 
            btnSair.BackColor = Color.Transparent;
            btnSair.BorderColor = Color.FromArgb(64, 64, 64);
            btnSair.BorderRadius = 15;
            btnSair.BorderThickness = 2;
            btnSair.Cursor = Cursors.Hand;
            btnSair.CustomizableEdges = customizableEdges4;
            btnSair.DisabledState.BorderColor = Color.DarkGray;
            btnSair.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSair.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSair.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSair.FillColor = Color.Transparent;
            btnSair.Font = new Font("Sora", 15.75F, FontStyle.Bold);
            btnSair.ForeColor = Color.White;
            btnSair.HoverState.ForeColor = Color.FromArgb(255, 47, 75);
            btnSair.Location = new Point(432, 353);
            btnSair.Name = "btnSair";
            btnSair.PressedColor = Color.Transparent;
            btnSair.ShadowDecoration.CustomizableEdges = customizableEdges5;
            btnSair.Size = new Size(290, 60);
            btnSair.TabIndex = 3;
            btnSair.Text = "Sair";
            btnSair.Click += btnSair_Click;
            // 
            // panelBack2
            // 
            panelBack2.BackgroundImage = Properties.Resources.BackGround2;
            panelBack2.BackgroundImageLayout = ImageLayout.Zoom;
            panelBack2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            panelBack2.Controls.Add(picFoto);
            panelBack2.Controls.Add(btnSair);
            panelBack2.Controls.Add(lblNome);
            panelBack2.Controls.Add(btnIniciar);
            panelBack2.CustomizableEdges = customizableEdges8;
            panelBack2.Location = new Point(2, 184);
            panelBack2.Name = "panelBack2";
            panelBack2.ShadowDecoration.CustomizableEdges = customizableEdges9;
            panelBack2.Size = new Size(1168, 519);
            panelBack2.TabIndex = 4;
            // 
            // panelBack1
            // 
            panelBack1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panelBack1.BackgroundImage = Properties.Resources.BackGround1;
            panelBack1.BackgroundImageLayout = ImageLayout.Zoom;
            panelBack1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            panelBack1.CustomizableEdges = customizableEdges1;
            panelBack1.Location = new Point(-10, 0);
            panelBack1.Name = "panelBack1";
            panelBack1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            panelBack1.Size = new Size(1180, 188);
            panelBack1.TabIndex = 5;
            // 
            // frmInicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1170, 700);
            Controls.Add(panelBack1);
            Controls.Add(panelBack2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmInicio";
            ((System.ComponentModel.ISupportInitialize)picFoto).EndInit();
            panelBack2.ResumeLayout(false);
            panelBack2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picFoto;
        private Guna.UI2.WinForms.Guna2Button btnIniciar;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNome;
        private Guna.UI2.WinForms.Guna2Button btnSair;
        private Guna.UI2.WinForms.Guna2Panel panelBack2;
        private Guna.UI2.WinForms.Guna2Panel panelBack1;
    }
}