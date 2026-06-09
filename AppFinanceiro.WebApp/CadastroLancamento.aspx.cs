using AppFinanceiro.Business;
using AppFinanceiro.Data.models;
using AppFinanceiro.Data.Repositories;
using System;
using System.Configuration;
using System.Web.UI;

namespace AppFinanceiro.WebApp
{
    public partial class CadastroLancamento : System.Web.UI.Page
    {
        private LancamentoService _service;

        protected void Page_Load(object sender, EventArgs e)
        {
            var connString = ConfigurationManager.ConnectionStrings["FinanceiroDB"].ConnectionString;
            var repository = new LancamentoRepository(connString);
            _service = new LancamentoService(repository);

            if (!IsPostBack)
            {
                // Se veio um id na URL é edição
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int id = int.Parse(Request.QueryString["id"]);
                    CarregarLancamento(id);
                }
                else
                {
                    // novo lançamento que mostra painel de desconto por padrão (Crédito)
                    pnlDesconto.Visible = true;
                    pnlTaxa.Visible = false;
                }
            }
        }

        private void CarregarLancamento(int id)
        {
            var connString = ConfigurationManager.ConnectionStrings["FinanceiroDB"].ConnectionString;
            var repository = new LancamentoRepository(connString);
            var lancamento = repository.ObterPorId(id);

            if (lancamento == null)
            {
                Response.Redirect("Lancamentos.aspx");
                return;
            }

            hfId.Value = lancamento.Id.ToString();
            txtDescricao.Text = lancamento.Descricao;
            ddlTipo.SelectedValue = lancamento.Tipo.ToString();
            txtValorOriginal.Text = lancamento.ValorOriginal.ToString("F2");
            txtDataLancamento.Text = lancamento.DataLancamento.ToString("yyyy-MM-dd");
            txtCompetencia.Text = lancamento.Competencia;

            if (lancamento.Tipo == Tipo.Credito)
            {
                pnlDesconto.Visible = true;
                pnlTaxa.Visible = false;
                txtPercentualDesconto.Text = lancamento.PercentualDesconto?.ToString("F2");
            }
            else
            {
                pnlDesconto.Visible = false;
                pnlTaxa.Visible = true;
                txtPercentualTaxa.Text = lancamento.PercentualTaxa?.ToString("F2");
            }
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipo.SelectedValue == "C")
            {
                pnlDesconto.Visible = true;
                pnlTaxa.Visible = false;
            }
            else
            {
                pnlDesconto.Visible = false;
                pnlTaxa.Visible = true;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var lancamento = new LancamentoFinanceiro
                {
                    Id = int.Parse(hfId.Value),
                    Descricao = txtDescricao.Text.Trim(),
                    Tipo = ddlTipo.SelectedValue == "C" ? Tipo.Credito : Tipo.Debito,
                    ValorOriginal = decimal.Parse(txtValorOriginal.Text),
                    DataLancamento = DateTime.Parse(txtDataLancamento.Text),
                    Competencia = txtCompetencia.Text.Trim(),
                    Status = StatusLancamento.Aberto
                };

                if (lancamento.Tipo == Tipo.Credito)
                    lancamento.PercentualDesconto = decimal.Parse(txtPercentualDesconto.Text);
                else
                    lancamento.PercentualTaxa = decimal.Parse(txtPercentualTaxa.Text);

                if (lancamento.Id == 0)
                    _service.InserirLancamento(lancamento);
                else
                    _service.AtualizarLancamento(lancamento);

                Response.Redirect("Lancamentos.aspx");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Lancamentos.aspx");
        }
    }
}