<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroLancamento.aspx.cs" Inherits="AppFinanceiro.WebApp.CadastroLancamento" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Cadastro de Lançamento</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Cadastro de Lançamento</h2>

        <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" />
        <br />

        <asp:HiddenField ID="hfId" runat="server" Value="0" />

        <div>
            <label>Descrição:</label>
            <asp:TextBox ID="txtDescricao" runat="server" /><br />

            <label>Tipo:</label>
            <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                <asp:ListItem Value="C" Text="Crédito" />
                <asp:ListItem Value="D" Text="Débito" />
            </asp:DropDownList><br />

            <label>Valor Original:</label>
            <asp:TextBox ID="txtValorOriginal" runat="server" /><br />

            <asp:Panel ID="pnlDesconto" runat="server">
                <label>Percentual de Desconto (%):</label>
                <asp:TextBox ID="txtPercentualDesconto" runat="server" /><br />
            </asp:Panel>

            <asp:Panel ID="pnlTaxa" runat="server" Visible="false">
                <label>Percentual de Taxa (%):</label>
                <asp:TextBox ID="txtPercentualTaxa" runat="server" /><br />
            </asp:Panel>

            <label>Data do Lançamento:</label>
            <asp:TextBox ID="txtDataLancamento" runat="server" TextMode="Date" /><br />

            <label>Competência (MM/YYYY):</label>
            <asp:TextBox ID="txtCompetencia" runat="server" /><br />
        </div>

        <br />
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" CausesValidation="false" />
    </form>
</body>
</html>