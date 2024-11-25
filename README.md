# gestão-de-pedidos

Este é o projeto **gestão-de-pedidos**, que inclui o Frontend (Vue.js) e o Backend (ASP.NET). Abaixo estão as instruções para inicializar ambos os ambientes e rodar o projeto localmente.

## Frontend (Vue.js)

Para iniciar o Frontend do projeto, siga os passos abaixo:

1. **Abra a pasta do Frontend**: Navegue até a pasta onde o código do Frontend está localizado.

2. **Instale as dependências**:
   No terminal, execute o seguinte comando para instalar todas as dependências do projeto:

   ```bash
   npm install
3. **Inicie o servidor de desenvolvimento**:
    Após a instalação das dependências, inicie o servidor de desenvolvimento com o seguinte comando:

   ```bash
   npm run serve
4. **Acesse o projeto**:
   O projeto estará disponível em http://localhost:8080

## Backend (ASP.NET)

1. Abra o NuGet Package Manager Console no Visual Studio.
2. **Atualize o banco de dados**:
   Selecione o projeto infra/infra e execute o seguinte comando no console para criar o banco de dados e as tabelas necessárias:
   ```bash
   Update-Database
3. **Inicie a Aplicação**:
   Após a execução do comando, inicie a aplicação clicando em Iniciar no Visual Studio ou aperte F5.
