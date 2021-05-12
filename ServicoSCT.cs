using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


namespace Bebela_Modas
{
    class ServicoSCT
    {

        EstruturaErro estruturaErro = new EstruturaErro();


        private SqlConnection ConectarBanco()
        {
            string stringConexao = "Server=tcp:projetos-producao.database.windows.net,1433;Initial Catalog=BebelaModas;Persist Security Info=False;User ID=hittor;Password=Carol1206;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection conexao = new SqlConnection(stringConexao);

            return conexao;

        }

        public EstruturaErro CadastrarCompra(Compra compra)
        {

            var conexaoBanco = ConectarBanco();

            try
            {
                var referencial = compra.Referencial;
                var produtos = compra.Produtos;
                var quantidade = compra.Quantidade;
                var valor = compra.Valor;
                var situacao = compra.Situacao;
                var data = compra.Data;

                //'"+@referencial+"',

                var query = "Insert into Compra (Referencial,Produto,Quantidade,Valor,Situacao,Data_Compra) values ('" + @referencial + "','" + @produtos + "','" + @quantidade + "','" + @valor + "','" + @situacao + "','" + @data + "')";

                SqlCommand consulta = new SqlCommand(query, conexaoBanco);

                consulta.Parameters.Add(new SqlParameter("@referencial", referencial));
                consulta.Parameters.Add(new SqlParameter("@produtos", produtos));
                consulta.Parameters.Add(new SqlParameter("@quantidade", quantidade));
                consulta.Parameters.Add(new SqlParameter("@valor", valor));
                consulta.Parameters.Add(new SqlParameter("@situacao", situacao));
                consulta.Parameters.Add(new SqlParameter("@data", data));


                conexaoBanco.Open();

                consulta.ExecuteNonQuery();

                conexaoBanco.Close();

                return ConsultarMensagem("CDC001I");

            }
            catch
            {

                return ConsultarMensagem("CDC001E");

            }


        }

        public EstruturaErro CadastrarCliente(Cliente cliente)
        {

            var conexaoBanco = ConectarBanco();

            try
            {
                var referencial = cliente.Referencial;
                var nome = cliente.Nome;
                var telefone = cliente.Telefone;
                var email = cliente.Email;
                var data = cliente.Data;

                var query = "Insert into Cliente (Referencial,Produto,Quantidade,Valor,Situacao,Data_Compra) values ('" + @referencial + "','" + @nome + "','" + @telefone + "','" + @email + "','" + @data + "')";

                SqlCommand consulta = new SqlCommand(query, conexaoBanco);

                consulta.Parameters.Add(new SqlParameter("@referencial", referencial));
                consulta.Parameters.Add(new SqlParameter("@nome", nome));
                consulta.Parameters.Add(new SqlParameter("@telefone", telefone));
                consulta.Parameters.Add(new SqlParameter("@email", email));
                consulta.Parameters.Add(new SqlParameter("@data", data));


                conexaoBanco.Open();

                consulta.ExecuteNonQuery();

                conexaoBanco.Close();

                return ConsultarMensagem("CDC002I"); // Cliente cadastrado com sucesso

            }
            catch
            {

                return ConsultarMensagem("CDC005E"); // Erro ao cadastrar cliente

            }



        }

        public EstruturaErro ConsultarMensagem(string codigoMensagem)
        {

            var conexaoBanco = ConectarBanco();

            var codigomensagem = codigoMensagem;

            var query = "Select Descricao,Indicador from Mensagem where Codigo = '" + @codigomensagem + "'";

            SqlCommand consulta = new SqlCommand(query, conexaoBanco);

            consulta.Parameters.Add(new SqlParameter("@codigomensagem", codigomensagem));

            consulta.CommandType = CommandType.Text;

            DataTable dt = GetDados(consulta);

            //string nome = "";

            if (dt.Rows.Count != 0)
            {

                string aux = dt.Rows[0]["Indicador"].ToString();

                //int IndicadorTipoUsuario = int.Parse(aux);
                //nome = dt.Rows[0]["Nome"].ToString();

                estruturaErro.CodigoMensagem = codigomensagem;
                estruturaErro.DescricaoMensagem = dt.Rows[0]["Descricao"].ToString();
                estruturaErro.indicadorErro = int.Parse(aux);
                return estruturaErro;


            }
            else
            {

                estruturaErro.CodigoMensagem = "";
                estruturaErro.DescricaoMensagem = "ERRO GENERICO";
                return estruturaErro;

            }


            /*estruturaErro.CodigoMensagem = codigoMensagem;
            estruturaErro.DescricaoMensagem = "";
            estruturaErro.indicadorErro = 0;

            return estruturaErro;*/
        }

        public DataTable GetDados(SqlCommand cmd)
        {

            var conectarBanco = ConectarBanco();
            DataTable dt = new DataTable();

            SqlDataAdapter sda = new SqlDataAdapter();

            cmd.CommandType = CommandType.Text;
            cmd.Connection = conectarBanco;
            try
            {
                conectarBanco.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conectarBanco.Close();
                sda.Dispose();
                conectarBanco.Dispose();
            }


        }


        public Tuple<DataTable, EstruturaErro> ListarCompraPorUsuario(int referencial, string nome)
        {
            var conexaoBanco = ConectarBanco();

            var nomes = nome;
            var referencials = referencial;

            var query = "select cl.Nome,c.Produto,c.Quantidade,c.Valor,c.Situacao,c.Data_Compra from Compra c inner join cliente cl " +
                "on c.Referencial = cl.Referencial " +
                "where cl.Nome = '" + @nome + "' and cl.Referencial = '" + @referencial + "';";

            SqlCommand consulta = new SqlCommand(query, conexaoBanco);

            consulta.Parameters.Add(new SqlParameter("@nome", nome));
            consulta.Parameters.Add(new SqlParameter("@referencial", referencial));

            consulta.CommandType = CommandType.Text;

            DataTable dt = GetDados(consulta);

            //string nome = "";

            if (dt.Rows.Count != 0)
            {


                //string aux = dt.Rows[0]["Indicador"].ToString();
                estruturaErro.CodigoMensagem = "";
                estruturaErro.DescricaoMensagem = "";
                estruturaErro.indicadorErro = 0;
                return new Tuple<DataTable, EstruturaErro>(dt, estruturaErro);


            }
            else
            {
                var retorno = ConsultarMensagem("CDC002E"); // Erro ao listar compra

                return new Tuple<DataTable, EstruturaErro>(dt, retorno);

            }
        }

        public Tuple<EstruturaErro, DataTable> ListarCliente()
        {
            var conexaoBanco = ConectarBanco();

            //var nomes = nome;
            //var referencials = referencial;

            var query = "select Referencial,Nome from Cliente";

            SqlCommand consulta = new SqlCommand(query, conexaoBanco);

            //consulta.Parameters.Add(new SqlParameter("@nome", nome));
            //consulta.Parameters.Add(new SqlParameter("@referencial", referencial));

            consulta.CommandType = CommandType.Text;

            DataTable dt = GetDados(consulta);

            //string nome = "";

            if (dt.Rows.Count != 0)
            {
                //string aux = dt.Rows[0]["Indicador"].ToString();
                estruturaErro.CodigoMensagem = "";
                estruturaErro.DescricaoMensagem = "";
                estruturaErro.indicadorErro = 0;

                return new Tuple<EstruturaErro, DataTable>(estruturaErro, dt);
            }
            else
            {
                var retorno = ConsultarMensagem("CDC003E"); // nenhum usuario localizado

                return new Tuple<EstruturaErro, DataTable>(retorno, dt);

            }

        }

        public Tuple<EstruturaErro, DataTable> ListarProduto()
        {
            var conexaoBanco = ConectarBanco();

            //var nomes = nome;
            //var referencials = referencial;

            var query = "select Descricao_Produto from Produto";

            SqlCommand consulta = new SqlCommand(query, conexaoBanco);

            //consulta.Parameters.Add(new SqlParameter("@nome", nome));
            //consulta.Parameters.Add(new SqlParameter("@referencial", referencial));

            consulta.CommandType = CommandType.Text;

            DataTable dt = GetDados(consulta);

            //string nome = "";

            if (dt.Rows.Count != 0)
            {
                //string aux = dt.Rows[0]["Indicador"].ToString();
                estruturaErro.CodigoMensagem = "";
                estruturaErro.DescricaoMensagem = "";
                estruturaErro.indicadorErro = 0;

                return new Tuple<EstruturaErro, DataTable>(estruturaErro, dt);
            }
            else
            {
                var retorno = ConsultarMensagem("CDC004E"); // Nenhum Produto localizado

                return new Tuple<EstruturaErro, DataTable>(retorno, dt);
            }

        }
    }
}
