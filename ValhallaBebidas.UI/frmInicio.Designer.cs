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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            picFoto = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            lblNome = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnIniciar = new Guna.UI2.WinForms.Guna2Button();
            btnSair = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)picFoto).BeginInit();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackgroundImage = Properties.Resources.Group_3;
            guna2Panel1.CustomizableEdges = customizableEdges8;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges9;
            guna2Panel1.Size = new Size(1170, 187);
            guna2Panel1.TabIndex = 0;
            // 
            // picFoto
            // 
            picFoto.ImageRotate = 0F;
            picFoto.Location = new Point(497, 265);
            picFoto.Name = "picFoto";
            picFoto.ShadowDecoration.CustomizableEdges = customizableEdges10;
            picFoto.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            picFoto.Size = new Size(150, 150);
            picFoto.TabIndex = 1;
            picFoto.TabStop = false;
            // 
            // lblNome
            // 
            lblNome.BackColor = Color.Transparent;
            lblNome.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNome.ForeColor = Color.White;
            lblNome.Location = new Point(497, 433);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(150, 34);
            lblNome.TabIndex = 2;
            lblNome.Text = "Olá, Usuário.";
            // 
            // btnIniciar
            // 
            btnIniciar.BorderRadius = 15;
            btnIniciar.CustomizableEdges = customizableEdges11;
            btnIniciar.DisabledState.BorderColor = Color.DarkGray;
            btnIniciar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnIniciar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnIniciar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnIniciar.FillColor = Color.FromArgb(214, 189, 119);
            btnIniciar.Font = new Font("Segoe UI", 9F);
            btnIniciar.ForeColor = Color.White;
            btnIniciar.Location = new Point(449, 483);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnIniciar.Size = new Size(261, 45);
            btnIniciar.TabIndex = 3;
            btnIniciar.Text = "Iniciar";
            btnIniciar.Click += btnIniciar_Click;
            // 
            // btnSair
            // 
            btnSair.BorderRadius = 15;
            btnSair.CustomizableEdges = customizableEdges13;
            btnSair.DisabledState.BorderColor = Color.DarkGray;
            btnSair.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSair.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSair.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSair.FillColor = Color.IndianRed;
            btnSair.Font = new Font("Segoe UI", 9F);
            btnSair.ForeColor = Color.White;
            btnSair.Location = new Point(449, 550);
            btnSair.Name = "btnSair";
            btnSair.PressedColor = Color.IndianRed;
            btnSair.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnSair.Size = new Size(261, 45);
            btnSair.TabIndex = 3;
            btnSair.Text = "Sair";
            btnSair.Click += btnSair_Click;
            // 
            // frmInicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1170, 700);
            Controls.Add(btnSair);
            Controls.Add(btnIniciar);
            Controls.Add(lblNome);
            Controls.Add(picFoto);
            Controls.Add(guna2Panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmInicio";
            ((System.ComponentModel.ISupportInitialize)picFoto).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picFoto;
        private Guna.UI2.WinForms.Guna2Button btnIniciar;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNome;
        private Guna.UI2.WinForms.Guna2Button btnSair;
    }
}