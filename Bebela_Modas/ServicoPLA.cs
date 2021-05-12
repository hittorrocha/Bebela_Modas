using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bebela_Modas
{
    class ServicoPLA
    {
        ServicoSCT servicoSCT = new ServicoSCT();
        public EstruturaErro CadastrarCompra(int referencial, string produto, int quantidade, double valor, string situacao)
        {

            Compra compra = new Compra();
            ServicoSCT servicoSCT = new ServicoSCT();

            compra.Referencial = referencial;
            compra.Produtos = produto;
            compra.Quantidade = quantidade;
            compra.Valor = valor;
            compra.Situacao = situacao;
            compra.Data = DateTime.Now;

            var retorno = servicoSCT.CadastrarCompra(compra);

            return retorno;



        }

        public EstruturaErro CadastrarCliente(int referencial, string nome, string telefone, string email)
        {

            Cliente cliente = new Cliente();
            ServicoSCT servicoSCT = new ServicoSCT();

            cliente.Referencial = referencial;
            cliente.Nome = nome;
            cliente.Telefone = telefone;
            cliente.Email = email;
            cliente.Data = DateTime.Now;

            var retorno = servicoSCT.CadastrarCliente(cliente);

            return retorno;

        }

        public Tuple<EstruturaErro, DataTable> ListarCompraPorUsuario(int referencial, string nome)
        {
            //Compra compra = new Compra();

            DataTable dt = null;


            if (referencial == 0 || nome == "" || nome == null)
            {
                EstruturaErro estruturaErro = new EstruturaErro();

                estruturaErro.CodigoMensagem = "";
                estruturaErro.DescricaoMensagem = "DADOS OBRIGATORIO NÃO INFORMADO";
                estruturaErro.indicadorErro = 1;

                return new Tuple<EstruturaErro, DataTable>(estruturaErro, dt);

            }
            else
            {

                var retorno = servicoSCT.ListarCompraPorUsuario(referencial, nome);

                return new Tuple<EstruturaErro, DataTable>(retorno.Item2, retorno.Item1);

            }

        }

        public Tuple<EstruturaErro, DataTable> ListarCliente()
        {

            var retorno = servicoSCT.ListarCliente();

            //ArrayList usuarios = new ArrayList();
            //usuarios.Add(retorno.Item2);

            return new Tuple<EstruturaErro, DataTable>(retorno.Item1, retorno.Item2);

        }

        public Tuple<EstruturaErro, DataTable> ListarProduto()
        {

            var retorno = servicoSCT.ListarProduto();

            return new Tuple<EstruturaErro, DataTable>(retorno.Item1, retorno.Item2);

        }



    }
}
