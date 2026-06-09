CREATE TABLE LancamentoFinanceiro (
    Id INT IDENTITY(1,1) NOT NULL,
    Descricao VARCHAR(255) NOT NULL,
    Tipo CHAR(1) NOT NULL,
    ValorOriginal DECIMAL(18, 2) NOT NULL,
    PercentualTaxa DECIMAL(5, 2),
    PercentualDesconto DECIMAL(5, 2),
    ValorCalculado DECIMAL(18,2) NOT NULL,
    DataLancamento DATETIME NOT NULL,
    DataCriacao DATETIME NOT NULL DEFAULT GETDATE(),
    DataPagamento DATETIME NULL,
    DataCancelamento DATETIME NULL,
    Competencia VARCHAR(7) NOT NULL,
    Status TINYINT NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT CK_Tipo CHECK (Tipo IN ('C', 'D')),
    CONSTRAINT CK_Status CHECK (Status IN (0, 1, 2)),
    CONSTRAINT CK_Taxa_Debito CHECK (PercentualTaxa IS NULL OR Tipo = 'D'),
    CONSTRAINT CK_Desconto_Credito CHECK (PercentualDesconto IS NULL OR Tipo = 'C'),
    CONSTRAINT CK_Debito_Requer_Taxa CHECK (Tipo <> 'D' OR PercentualTaxa IS NOT NULL),
    CONSTRAINT CK_Credito_Requer_Desconto CHECK (Tipo <> 'C' OR PercentualDesconto IS NOT NULL)
)

CREATE UNIQUE INDEX UX_Lancamento_Duplicado
    ON LancamentoFinanceiro (Competencia, Descricao, Tipo)