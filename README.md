# Sistema de Agendamento de Serviços Acadêmicos (ASA) - Front-End

Este repositório contém o front-end do **Sistema de Agendamento de Serviços Acadêmicos (ASA)**, desenvolvido em C# utilizando o **Blazor WebAssembly**. O sistema tem como objetivo centralizar e automatizar os processos de agendamento de serviços acadêmicos disponibilizados pela **Ufersa**.

## 📋 Visão Geral

O front-end do ASA é responsável por fornecer uma interface intuitiva e responsiva para que estudantes e profissionais possam interagir com o sistema de agendamento de serviços acadêmicos, permitindo que os usuários realizem o agendamento, edição e visualização de horários disponíveis de forma eficiente. A integração com o back-end é feita de forma segura e otimizada, assegurando que os dados dos usuários sejam tratados de forma adequada.

## 🛠️ Tecnologias Utilizadas

- **Blazor (C# e Razor)** (recurso para a paginação)
- **CSS e Boostrap** (recurso para estilização)
- **WebAssembly**

## 📦 Estrutura de Pastas

- `/Layout`: Componentes de layout reutilizáveis.
- `/Pages`: Páginas da aplicação.
- `/wwwroot`: Elementos de estilização.

## 🚀 Como Configurar o Projeto

### 1. Pré-requisitos

Certifique-se de ter o seguinte software instalado em sua máquina:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)

#### Instruções opcionais para a instalação no linux:

- Adicione o repositório da Microsoft:
```bash
sudo apt-get update && sudo apt-get install -y wget
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```
- Instale o SDK do .NET 8.0:
```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

- Verifique a versão
```bash
dotnet --version
```

### 2. Clonar o Repositório

```bash
git clone https://github.com/liviabeatrizml/sistema-de-agendamento-de-servicos-academicos.git
cd sistema-de-agendamento-de-servicos-academicos/
git checkout front-end
```

### 3. Instalar Dependências

Execute o seguinte comando para restaurar os pacotes necessários:

```bash
dotnet restore
```

### 4. Executar a Aplicação

Agora você pode rodar a aplicação com o seguinte comando:

```bash
dotnet watch run --project Front-end/Front-end.csproj
```

O front-end estará disponível em `http://localhost:5156`.
