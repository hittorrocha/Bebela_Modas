using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bebela_Modas
{
    public partial class CadastroCliente : Form
    {
        public CadastroCliente()
        {
            InitializeComponent();
        }
        private void PopularDgvCliente()
        {
            ServicoPLA servicoPLA = new ServicoPLA();
            var retorno = servicoPLA.ListarCliente();
            dgvCliente.DataSource = retorno.Item2;

        }
        private void CadastroUsuario_Load(object sender, EventArgs e)
        {
            PopularDgvCliente();
        }
    }
}
