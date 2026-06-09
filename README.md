# AppFinanceiro

Aplicação web de controle de lançamentos financeiros desenvolvida com ASP.NET Web Forms, C# e SQL Server.

## Tecnologias

- **Front-end:** ASP.NET Web Forms (.aspx) — .NET Framework 4.8
- **Back-end:** C# em Class Libraries
- **Banco de Dados:** SQL Server (LocalDB para desenvolvimento)
- **Acesso a dados:** ADO.NET puro (SqlConnection, SqlCommand, SqlDataReader)

## Estrutura do Projeto

AppFinanceiro.Data        → Entidades, enums e repositórios (ADO.NET)
AppFinanceiro.Business    → Regras de negócio, validações e serviços
AppFinanceiro.WebApp      → Páginas .aspx e code-behind


## Decisões técnicas

**Por onde começar:** Comecei o desenvolvimento pelo banco de dados porque isso me ajudou a enxergar melhor como o sistema seria estruturado. Durante a modelagem, consegui entender quais dados eu precisaria armazenar, como eles se relacionavam e quais regras deveriam existir. Com isso em mente, ficou mais fácil desenvolver as outras camadas da aplicação.

**Tipo TINYINT para Status:** Escolhi o tipo TINYINT no campo Status porque ele armazena apenas alguns valores numéricos pré-definidos, como Aberto, Pago e Cancelado. Como não há necessidade de armazenar texto nesse campo, preferi utilizar TINYINT. Já o tipo CHAR foi utilizado em campos que representam caracteres, como o campo Tipo, que identifica se o lançamento é Crédito ou Débito.

**Constraints no banco de dados:** Além das validações feitas na camada de negócio, também adicionei constraints no banco de dados para garantir que os dados continuem consistentes. Pensei nelas como uma camada extra de segurança: mesmo que alguma validação da aplicação venha a falhar, o banco ainda impede que informações inválidas sejam salvas. Um exemplo é a regra que permite Taxa apenas para Débito e Desconto apenas para Crédito.

**Separação em camadas:** Optei por dividir o projeto em três camadas: Data, Business e WebApp. Com isso, cada parte ficou responsável por uma função específica, deixando o código mais organizado e fácil de entender. A camada Data cuida do banco de dados, a Business das regras e validações, e a WebApp da interface com o usuário. Também pensei em separar algumas validações em uma classe específica, como um LancamentoService, o que deixaria o projeto ainda mais organizado. Porém, como o objetivo era a entrega da atividade e o projeto não possui uma complexidade tão grande, preferi manter essas validações na camada Business para deixar a implementação mais simples.

## Pré-requisitos

- Visual Studio 2022
- .NET Framework 4.8
- SQL Server LocalDB (já incluso no Visual Studio)

## Como rodar

1. Clone o repositório
2. Abra o arquivo AppFinanceiro.sln no Visual Studio
3. No **SQL Server Object Explorer**, crie um banco chamado FinanceiroDB
4. Execute o script em AppFinanceiro.Data/Scripts/script.sql
5. Defina AppFinanceiro.WebApp como projeto de inicialização
6. Pressione F5 para rodar