using System.Data.SqlClient;
using AppFinanceiro.Data.models;

namespace AppFinanceiro.Data.Repositories
{
    public class LancamentoRepository
    {
        private readonly string _connectionString;

        public LancamentoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Inserir(LancamentoFinanceiro lancamento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"INSERT INTO LancamentoFinanceiro (Descricao, Tipo, ValorOriginal, PercentualTaxa, PercentualDesconto, ValorCalculado, DataLancamento, Competencia, Status)
                              VALUES (@Descricao, @Tipo, @ValorOriginal, @PercentualTaxa, @PercentualDesconto, @ValorCalculado, @DataLancamento, @Competencia, @Status)";

                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Descricao", lancamento.Descricao);
                cmd.Parameters.AddWithValue("@Tipo", (char)lancamento.Tipo);
                cmd.Parameters.AddWithValue("@ValorOriginal", lancamento.ValorOriginal);
                cmd.Parameters.AddWithValue("@PercentualTaxa", (object)lancamento.PercentualTaxa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PercentualDesconto", (object)lancamento.PercentualDesconto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ValorCalculado", lancamento.ValorCalculado);
                cmd.Parameters.AddWithValue("@DataLancamento", lancamento.DataLancamento);
                cmd.Parameters.AddWithValue("@Competencia", lancamento.Competencia);
                cmd.Parameters.AddWithValue("@Status", (int)lancamento.Status);
                cmd.ExecuteNonQuery();
            }
        }

        public LancamentoFinanceiro ObterPorId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM LancamentoFinanceiro WHERE Id = @Id";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new LancamentoFinanceiro
                        {
                            Id = (int)reader["Id"],
                            Descricao = reader["Descricao"].ToString(),
                            Tipo = (Tipo)reader["Tipo"].ToString()[0],
                            ValorOriginal = (decimal)reader["ValorOriginal"],
                            PercentualTaxa = reader["PercentualTaxa"] as decimal?,
                            PercentualDesconto = reader["PercentualDesconto"] as decimal?,
                            ValorCalculado = (decimal)reader["ValorCalculado"],
                            DataLancamento = (DateTime)reader["DataLancamento"],
                            DataCriacao = (DateTime)reader["DataCriacao"],
                            DataPagamento = reader["DataPagamento"] as DateTime?,
                            DataCancelamento = reader["DataCancelamento"] as DateTime?,
                            Competencia = reader["Competencia"].ToString(),
                            Status = (StatusLancamento)Convert.ToInt32(reader["Status"])
                        };
                    }
                }
            }
            return null;
        }

        public List<LancamentoFinanceiro> ListarTodos()
        {
            var lancamentos = new List<LancamentoFinanceiro>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM LancamentoFinanceiro";
                var cmd = new SqlCommand(query, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lancamentos.Add(new LancamentoFinanceiro
                        {
                            Id = (int)reader["Id"],
                            Descricao = reader["Descricao"].ToString(),
                            Tipo = (Tipo)reader["Tipo"].ToString()[0],
                            ValorOriginal = (decimal)reader["ValorOriginal"],
                            PercentualTaxa = reader["PercentualTaxa"] as decimal?,
                            PercentualDesconto = reader["PercentualDesconto"] as decimal?,
                            ValorCalculado = (decimal)reader["ValorCalculado"],
                            DataLancamento = (DateTime)reader["DataLancamento"],
                            DataCriacao = (DateTime)reader["DataCriacao"],
                            DataPagamento = reader["DataPagamento"] as DateTime?,
                            DataCancelamento = reader["DataCancelamento"] as DateTime?,
                            Competencia = reader["Competencia"].ToString(),
                            Status = (StatusLancamento)Convert.ToInt32(reader["Status"])
                        });
                    }
                }
            }
            return lancamentos;
        }

        public void Atualizar(LancamentoFinanceiro lancamento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"UPDATE LancamentoFinanceiro SET 
                              Descricao = @Descricao, 
                              Tipo = @Tipo, 
                              ValorOriginal = @ValorOriginal, 
                              PercentualTaxa = @PercentualTaxa, 
                              PercentualDesconto = @PercentualDesconto, 
                              ValorCalculado = @ValorCalculado, 
                              DataLancamento = @DataLancamento, 
                              Competencia = @Competencia 
                              WHERE Id = @Id AND Status = 0";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Descricao", lancamento.Descricao);
                cmd.Parameters.AddWithValue("@Tipo", (char)lancamento.Tipo);
                cmd.Parameters.AddWithValue("@ValorOriginal", lancamento.ValorOriginal);
                cmd.Parameters.AddWithValue("@PercentualTaxa", (object)lancamento.PercentualTaxa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PercentualDesconto", (object)lancamento.PercentualDesconto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ValorCalculado", lancamento.ValorCalculado);
                cmd.Parameters.AddWithValue("@DataLancamento", lancamento.DataLancamento);
                cmd.Parameters.AddWithValue("@Competencia", lancamento.Competencia);
                cmd.Parameters.AddWithValue("@Id", lancamento.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Pagar(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"UPDATE LancamentoFinanceiro SET 
                              Status = 1, 
                              DataPagamento = @DataPagamento 
                              WHERE Id = @Id AND Status = 0";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@DataPagamento", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Cancelar(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"UPDATE LancamentoFinanceiro SET 
                              Status = 2, 
                              DataCancelamento = @DataCancelamento 
                              WHERE Id = @Id AND Status IN (0, 1)";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@DataCancelamento", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<LancamentoFinanceiro> ListarPorCompetencia(string competencia)
        {
            var lancamentos = new List<LancamentoFinanceiro>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM LancamentoFinanceiro WHERE Competencia = @Competencia";
                var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Competencia", competencia);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lancamentos.Add(new LancamentoFinanceiro
                        {
                            Id = (int)reader["Id"],
                            Descricao = reader["Descricao"].ToString(),
                            Tipo = (Tipo)reader["Tipo"].ToString()[0],
                            ValorOriginal = (decimal)reader["ValorOriginal"],
                            PercentualTaxa = reader["PercentualTaxa"] as decimal?,
                            PercentualDesconto = reader["PercentualDesconto"] as decimal?,
                            ValorCalculado = (decimal)reader["ValorCalculado"],
                            DataLancamento = (DateTime)reader["DataLancamento"],
                            DataCriacao = (DateTime)reader["DataCriacao"],
                            DataPagamento = reader["DataPagamento"] as DateTime?,
                            DataCancelamento = reader["DataCancelamento"] as DateTime?,
                            Competencia = reader["Competencia"].ToString(),
                            Status = (StatusLancamento)Convert.ToInt32(reader["Status"])
                        });
                    }
                }
            }
            return lancamentos;
        }

        public SaldoResumo ObterSaldoResumo()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"SELECT 
                              SUM(CASE WHEN Tipo = 'C' THEN ValorCalculado ELSE 0 END) AS TotalCredito,
                              SUM(CASE WHEN Tipo = 'D' THEN ValorCalculado ELSE 0 END) AS TotalDebito
                              FROM LancamentoFinanceiro 
                              WHERE Status = 1";
                var cmd = new SqlCommand(query, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var totalCredito = reader["TotalCredito"] as decimal? ?? 0;
                        var totalDebito = reader["TotalDebito"] as decimal? ?? 0;
                        return new SaldoResumo
                        {
                            TotalCredito = totalCredito,
                            TotalDebito = totalDebito,
                            Saldo = totalCredito - totalDebito
                        };
                    }
                }
            }
            return new SaldoResumo();
        }
    }
}
