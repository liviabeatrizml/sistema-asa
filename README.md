# Sistema de Agendamento de Serviços Acadêmicos (ASA) - Contracts

Este repositório contém o back-end com os contratos feitos em **Code Contracts** do **Sistema de Agendamento de Serviços Acadêmicos (ASA)**, desenvolvido em C# utilizando a **ASP.NET Core Web API**. O sistema tem como objetivo centralizar e automatizar os processos de agendamento de serviços acadêmicos disponibilizados pela **Ufersa**. Os contratos foram usados para verificar a eficácia do tratamento dos requisitos.

## 📋 Visão Geral

O back-end do ASA gerencia todas as requisições relacionadas ao agendamento de serviços acadêmicos, permitindo que estudantes e profissionais agendem, editem e visualizem horários disponíveis de forma centralizada. A aplicação segue boas práticas de desenvolvimento, utilizando **Verificação e Validação** para garantir a qualidade, bem como **Métodos Formais** para modelar o sistema de forma precisa.

**Code Contracts** são uma ferramenta no desenvolvimento de software que permite especificar condições que devem ser cumpridas pelo código durante sua execução. Essas condições podem ser aplicadas para verificar as entradas (pré-condições), saídas (pós-condições), e estados intermediários (invariantes) de métodos e classes, ajudando a detectar erros mais cedo e aumentando a confiabilidade do software.

No contexto do ASA, onde contratos são usados durante o desenvolvimento, eles ajudam a garantir que o sistema de agendamento funcione corretamente, verificando:

Pré-condições: Validam que os parâmetros passados para métodos estão corretos antes de a execução começar. Por exemplo, ao agendar um horário, garantir que a data e o serviço existam e estejam disponíveis.

Pós-condições: Asseguram que, após a execução do método, o estado do sistema esteja conforme esperado. Por exemplo, ao concluir um agendamento, verificar que ele realmente foi registrado no sistema.

Invariantes: Garantem que certas condições se mantenham verdadeiras durante a execução de um objeto. Isso pode ser útil para garantir, por exemplo, que uma agenda nunca tenha conflitos de horários.

Durante o desenvolvimento do ASA, esses contratos podem ser inseridos no código para validação automática. Contudo, como os contratos impactam no desempenho, eles geralmente são removidos ou desativados na versão final do software, quando a validação já está concluída, por isso, essa branch existe, separando a versão final da versão com contratos.

## 🛠️ Tecnologias Utilizadas

- **C#**
- **ASP.NET Core Web API**
- **Entity Framework Core** (para gerenciamento do banco de dados)
- **MySQL** (banco de dados relacional)
- **JWT** (para autenticação e autorização)
- - **Code Contracts** (para validação dos requisitos)

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


