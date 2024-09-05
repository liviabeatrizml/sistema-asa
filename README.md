# Sistema de Agendamento de Serviços Acadêmicos (ASA) - Back-End

Este repositório contém o back-end do **Sistema de Agendamento de Serviços Acadêmicos (ASA)**, desenvolvido em C# utilizando a **ASP.NET Core Web API**. O sistema tem como objetivo centralizar e automatizar os processos de agendamento de serviços acadêmicos disponibilizados pela **Ufersa**.

## 📋 Visão Geral

O back-end do ASA gerencia todas as requisições relacionadas ao agendamento de serviços acadêmicos, permitindo que estudantes e profissionais agendem, editem e visualizem horários disponíveis de forma centralizada. A aplicação segue boas práticas de desenvolvimento, utilizando **Verificação e Validação** para garantir a qualidade, bem como **Métodos Formais** para modelar o sistema de forma precisa.

## 🛠️ Tecnologias Utilizadas

- **C#**
- **ASP.NET Core Web API**
- **Entity Framework Core** (para gerenciamento do banco de dados)
- **MySQL** (banco de dados relacional)
- **JWT** (para autenticação e autorização)

## 📦 Estrutura de Pastas

- `/Controllers`: Controladores para gerenciar as requisições HTTP.
- `/Models`: Modelos que representam as entidades do sistema.
- `/Data`: Contexto do banco de dados e classes de migração.
- `/Services`: Implementação de lógica de negócios.
- `/DTOs`: Objetos de transferência de dados para padronizar as respostas da API.

## 🚀 Como Configurar o Projeto

### 1. Pré-requisitos

Certifique-se de ter os seguintes softwares instalados em sua máquina:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) ou superior
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) para o banco de dados
- Um cliente para testar as requisições, como [Postman](https://www.postman.com/) ou [Insomnia](https://insomnia.rest/)

### 2. Clonar o Repositório

```bash
git clone https://github.com/seuusuario/sistema-asa-backend.git
cd sistema-asa-backend
```

### 3. Instalar Dependências

Execute o seguinte comando para restaurar os pacotes necessários:

```bash
dotnet restore
```

### 4. Configuração do Banco de Dados

No arquivo `appsettings.json`, configure a string de conexão com o banco de dados MySQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=asa;User=SeuUsuario;Password=SuaSenha;"
}
```

### 5. Aplicar as Migrações

Para garantir que o banco de dados esteja sincronizado com os modelos, execute o comando para aplicar as migrações:

```bash
dotnet ef database update
```

### 6. Executar a Aplicação

Agora você pode rodar a aplicação com o seguinte comando:

```bash
dotnet run
```

O back-end estará disponível em `http://localhost:3000`.

## 🧩 Pacotes e Dependências

Aqui estão os principais pacotes utilizados no projeto e suas funções:

- **Microsoft.EntityFrameworkCore**: Pacote principal do Entity Framework Core, usado para mapear as classes C# no banco de dados.
- **MySql.EntityFrameworkCore**: Provedor do Entity Framework Core para MySQL.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Implementação de autenticação JWT para proteger as rotas da API.
- **Swashbuckle.AspNetCore**: Utilizado para gerar a documentação da API com o Swagger.
  
Instale os pacotes com os seguintes comandos:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
```

### Autenticação JWT

A autenticação JWT é configurada no arquivo `Program.cs` e protegemos algumas rotas para que apenas usuários autenticados possam acessá-las. A chave e o emissor estão definidos no arquivo `appsettings.json`:

```json
"Jwt": {
  "Issuer": "SeuIssuerAqui",
  "Key": "SuaChaveSecretaAqui"
}
```

## 📑 Documentação da API

A documentação da API é gerada automaticamente utilizando o **Swagger**. Após iniciar o projeto, você pode acessar a interface do Swagger em:

```
http://localhost:5000/swagger
```


