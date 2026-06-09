<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lancamentos.aspx.cs" Inherits="AppFinanceiro.WebApp.Lancamentos" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Lançamentos Financeiros</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Lançamentos Financeiros</h2>

        <div>
            <strong>Total Créditos: </strong><asp:Label ID="lblTotalCredito" runat="server" />&nbsp;|&nbsp;
            <strong>Total Débitos: </strong><asp:Label ID="lblTotalDebito" runat="server" />&nbsp;|&nbsp;
            <strong>Saldo: </strong><asp:Label ID="lblSaldo" runat="server" />
        </div>

        <br />
        <asp:Button ID="btnNovo" runat="server" Text="Novo Lançamento" OnClick="btnNovo_Click" />
        <br /><br />

        <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" />

        <asp:GridView ID="gvLancamentos" runat="server" AutoGenerateColumns="false"
            OnRowCommand="gvLancamentos_RowCommand" DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                <asp:BoundField DataField="TipoDescricao" HeaderText="Tipo" />
                <asp:BoundField DataField="ValorOriginal" HeaderText="Valor Original" DataFormatString="{0:C}" />
                <asp:BoundField DataField="ValorCalculado" HeaderText="Valor Calculado" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Competencia" HeaderText="Competência" />
                <asp:BoundField DataField="StatusDescricao" HeaderText="Status" />
                <asp:BoundField DataField="DataLancamento" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="Ações">
                    <ItemTemplate>
                        <asp:LinkButton CommandName="Editar" CommandArgument='<%# Eval("Id") %>' 
                            runat="server" Text="Editar" 
                            Visible='<%# (int)Eval("Status") == 0 %>' />
                        &nbsp;
                        <asp:LinkButton CommandName="Pagar" CommandArgument='<%# Eval("Id") %>' 
                            runat="server" Text="Pagar"
                            Visible='<%# (int)Eval("Status") == 0 %>'
                            OnClientClick="return confirm('Confirmar pagamento?');" />
                        &nbsp;
                        <asp:LinkButton CommandName="Cancelar" CommandArgument='<%# Eval("Id") %>' 
                            runat="server" Text="Cancelar"
                            Visible='<%# (int)Eval("Status") != 2 %>'
                            OnClientClick="return confirm('Confirmar cancelamento?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>