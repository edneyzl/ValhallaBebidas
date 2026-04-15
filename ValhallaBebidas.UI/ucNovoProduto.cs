using SenacBuy.UI.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucNovoProduto : UserControl
    {
        private readonly ProdutoApiService _produtoService = new();
        private readonly CategoriaApiService _categoriaService = new();
        private List<CategoriaDto> _categorias = new();
        private string? _caminhoFotoLocal;
        private readonly int? _idEdicao;


        public ucNovoProduto(int? id = null)
        {
            InitializeComponent();
            _idEdicao = id;
        }

    }
}
