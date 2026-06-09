using System;
using System.Collections.Generic;
using System.Collections.Generic;
using AppFinanceiro.Data.models;
using AppFinanceiro.Data.Repositories;

namespace AppFinanceiro.Business;

public class LancamentoService
{
    private readonly LancamentoRepository _lancamentoRepository;

    public LancamentoService(LancamentoRepository lancamentoRepository)
    {
        _lancamentoRepository = lancamentoRepository;
    }

    private decimal CalcularValor(LancamentoFinanceiro lancamento)
    {
        if (lancamento.Tipo == Tipo.Credito)
            return lancamento.ValorOriginal - (lancamento.ValorOriginal * lancamento.PercentualDesconto.Value / 100);
        else
            return lancamento.ValorOriginal + (lancamento.ValorOriginal * lancamento.PercentualTaxa.Value / 100);
    }

    public void InserirLancamento(LancamentoFinanceiro lancamento)
    {
        if(string.IsNullOrEmpty(lancamento.Descricao))
            throw new Exception("Descrição do lançamento é obrigatória.");

        if (lancamento.ValorOriginal <= 0)
            throw new Exception("Valor original deve ser maior que zero.");

        if (lancamento.Tipo == Tipo.Credito && !lancamento.PercentualDesconto.HasValue)
            throw new Exception("Desconto é obrigatório para Crédito");

        if (lancamento.Tipo == Tipo.Debito && !lancamento.PercentualTaxa.HasValue)
            throw new Exception("Taxa é obrigatório para Debito");

        lancamento.ValorCalculado = CalcularValor(lancamento);
        _lancamentoRepository.Inserir(lancamento);
    }

    public void AtualizarLancamento(LancamentoFinanceiro lancamento)
    {
        var existente = _lancamentoRepository.ObterPorId(lancamento.Id);

        if (existente == null || existente.Status != StatusLancamento.Aberto)
            throw new Exception("Apenas lançamentos Abertos podem ser editados");

        if (lancamento.Id <= 0)
            throw new Exception("ID do lançamento é inválido.");

        if (string.IsNullOrEmpty(lancamento.Descricao))
            throw new Exception("Descrição do lançamento é obrigatória.");

        if (lancamento.ValorOriginal <= 0)
            throw new Exception("Valor original deve ser maior que zero.");

        if (lancamento.Tipo == Tipo.Credito && !lancamento.PercentualDesconto.HasValue)
            throw new Exception("Desconto é obrigatório para Crédito");

        if (lancamento.Tipo == Tipo.Debito && !lancamento.PercentualTaxa.HasValue)
            throw new Exception("Taxa é obrigatório para Debito");

        lancamento.ValorCalculado = CalcularValor(lancamento);
        _lancamentoRepository.Atualizar(lancamento);
    }

    public void PagarLancamento(int id)
    {
        var lancamento = _lancamentoRepository.ObterPorId(id);
        if (lancamento == null)
            throw new Exception("Lançamento não encontrado.");
        if (lancamento.Status != StatusLancamento.Aberto)
            throw new Exception("Apenas lançamentos Abertos podem ser pagos.");

        _lancamentoRepository.Pagar(id);
    }

    public void CancelarLancamento(int id)
    {
        var lancamento = _lancamentoRepository.ObterPorId(id);
        if (lancamento == null)
            throw new Exception("Lançamento não encontrado.");

        if (lancamento.Status == StatusLancamento.Cancelado)
            throw new Exception("Lançamento já está cancelado.");

        _lancamentoRepository.Cancelar(id);
    }
    public List<LancamentoFinanceiro> ListarTodos()
    {
        return _lancamentoRepository.ListarTodos();
    }

    public SaldoResumo ObterSaldoResumo()
    {
        return _lancamentoRepository.ObterSaldoResumo();
    }
}
