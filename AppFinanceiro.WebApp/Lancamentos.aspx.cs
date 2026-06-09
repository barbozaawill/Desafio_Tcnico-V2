using AppFinanceiro.Business;
using AppFinanceiro.Data.models;
using AppFinanceiro.Data.Repositories;
using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppFinanceiro.WebApp
{
    public partial class Lancamentos : System.Web.UI.Page
    {
        private LancamentoService _service;

        protected void Page_Load(object sender, EventArgs e)
        {
            var connString = ConfigurationManager.ConnectionStrings["FinanceiroDB"].ConnectionString;
            var repository = new LancamentoRepository(connString);
            _service = new LancamentoService(repository);

            if (!IsPostBack)
                CarregarDados();
        }

        private void CarregarDados()
        {
            var lancamentos = _service.ListarTodos();
            gvLancamentos.DataSource = lancamentos;
            gvLancamentos.DataBind();

            var saldo = _service.ObterSaldoResumo();
            lblTotalCredito.Text = saldo.TotalCredito.ToString("C");
            lblTotalDebito.Text = saldo.TotalDebito.ToString("C");
            lblSaldo.Text = saldo.Saldo.ToString("C");
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("CadastroLancamento.aspx");
        }

        protected void gvLancamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());

            try
            {
                if (e.CommandName == "Editar")
                {
                    Response.Redirect($"CadastroLancamento.aspx?id={id}");
                }
                else if (e.CommandName == "Pagar")
                {
                    _service.PagarLancamento(id);
                    lblMensagem.ForeColor = System.Drawing.Color.Green;
                    lblMensagem.Text = "Lançamento pago com sucesso!";
                }
                else if (e.CommandName == "Cancelar")
                {
                    _service.CancelarLancamento(id);
                    lblMensagem.ForeColor = System.Drawing.Color.Green;
                    lblMensagem.Text = "Lançamento cancelado com sucesso!";
                }

                CarregarDados();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}