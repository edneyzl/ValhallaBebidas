namespace ValhallaBebidas.UI
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

      
        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCadastroFuncionario form = new();//chama a tela de cadastro
            form.ShowDialog();//impede que clicar em outra tela enquanto a de cadastro estiver aberta
        }

        

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
