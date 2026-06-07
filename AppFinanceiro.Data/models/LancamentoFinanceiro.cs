namespace AppFinanceiro.Data.models;

public class LancamentoFinanceiro
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public Tipo Tipo { get; set; }
    public decimal ValorOriginal{ get; set; }
    public decimal? PercentualTaxa { get; set; }
    public decimal? PercentualDesconto { get; set; }
    public decimal ValorCalculado { get; set; }
    public DateTime DataLancamento { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataPagamento { get; set; }
    public DateTime? DataCancelamento { get; set; }
    public string Competencia { get; set; }
    public StatusLancamento Status { get; set; }
}
