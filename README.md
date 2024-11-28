# 🔄️ Tarefas a Fazer

Este repositório contém a aplicação ["Tarefas a Fazer"](https://webfrontend.politesky-105466da.brazilsouth.azurecontainerapps.io), desenvolvida com o intuito de gerenciar tarefas diárias, utilizando um conjunto moderno de tecnologias e boas práticas de desenvolvimento. O projeto segue uma arquitetura minimalista, aproveitando a **Minimal API** para o backend e **Blazor Server** para o frontend.

## 👨‍💻 Tecnologias Utilizadas

- **Aspire Host**: Hosting para a API minimalista.
- **.NET 8**: Framework principal da aplicação, utilizando a versão mais recente do .NET.
- **Blazor Server**: Aplicação web interativa para o gerenciamento de tarefas.
- **SQL Server**: Banco de dados relacional utilizado para armazenamento das tarefas.
- **Entity Framework Core**: Mapeamento objeto-relacional (ORM) para manipulação do banco de dados.
- **xUnit**: Framework de testes unitários para garantir a qualidade do código.
- **CI/CD no Azure**: Pipeline de integração e entrega contínua configurado para deploy no **Azure Container Instances**.

## ✅ Funcionalidades

- Criação, atualização, visualização e exclusão de tarefas.
- Marcação de tarefas como concluídas.
- Integração com o SQL Server via **Entity Framework**.
- Testes automatizados com **xUnit** e **bUnit**.
- Deploy automatizado via **Git WorkFlow**.
- Hospedado no Azure Container

## 🗂️ Estrutura do Projeto

```bash
.
├── TodDosProject.ApiService         # Projeto da API
├── TodDosProject.AppHost            # Orquestrador do aplicativo
├── TodDosProject.Domain             # Biblioteca de classe da regra de negócio
├── TodDosProject.Infraestructure    # Biblioteca de classe do acesso a dados
├── TodDosProject.ServiceDefaults    # Conjunto de métodos para adicionar as funcionalidades do Aspire
├── TodDosProject.Test               # Projeto de Testes unitários e de integração e funcionalidade
├── TodDosProject.Web                # Projeto do Web
└── README.md
```
## 🔥 Configuração e Execução

### Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Docker](https://www.docker.com/)
- [IDE como o Visual Studio (opicional)](https://visualstudio.microsoft.com/vs/)
  [Mais informações](https://learn.microsoft.com/pt-br/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio)
  
### Passos para Executar Localmente

1. **Clonar o Repositório**

   ```bash
   git clone https://github.com/raphaelfalves/ToDosProject.git
   ```

2. **Configurar o Banco de Dados**

   - Navegue até o arquivo `appsettings.json` localizado na pasta `src/AppHost`.
   - Atualize a _connection string_ com os dados do seu ambiente local ou de desenvolvimento:
     ```json
     "ConnectionStrings": {
       "SqlServer": "Server=seu_servidor;Database=sua_base_de_dados;User Id=seu_usuario;Password=sua_senha;"
     }
     ```
3. **Rode o Docker**

   O Aspire precisa de um [OCI(Open Container Initiate)](https://opencontainers.org/), nesse projeto uso o Docker.

4. **Inicialize o Projeto**

   Rode o projeto no visual studio utilizando o AppHost como projeto de inicialização ou navegue até a pasta do ToDosProjet.AppHost no terminal e rode o comando 

```bash
dotnet run
```

no terminal vai aparecer um log com o link do dashboard só entrar e tandan 
![image](https://github.com/user-attachments/assets/44ed150c-7058-4354-98d0-c8223e738f9e)

