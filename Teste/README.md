# Sistema de Agendamento de Serviços Acadêmicos (ASA) - Testes

Este repositório contém os testes para o **Sistema de Agendamento de Serviços Acadêmicos (ASA)**, utilizando **Selenium** para testes de interface; **NUnit** para a automatização de execução dos testes; **Lighthouse** para testes de qualidade de página; **DevTools** para testes de responsividade; e **SonarLint** para testes de qualidade de código. Dessa forma, o objetivo dos testes é garantir que as funcionalidades do ASA estejam funcionando conforme o esperado.

## 📋 Visão Geral

Os testes são responsáveis por validar e verificar o comportamento adequado do sistema. Garantindo que as funcionalidades estejam corretas e atendem aos requisitos especificados. Os testes incluem teste de unidade, integração e sistema, bem como testes não funcionais (usabilidade, qualidade de página responsividade, qualidade de código).

## 🛠️ Tecnologias Utilizadas

- **NUnit** (para estrutura de testes e asserções)
- **Selenium WebDriver** (para automação de navegador)

## 📦 Estrutura de Pastas

- `/testeDeSistema`: Contém os arquivos de teste.

## 🚀 Como Configurar e Executar os Testes

### 1. Pré-requisitos

Certifique-se de ter o seguinte software instalado em sua máquina:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [Google Chrome](https://www.google.com/chrome/)

#### Instruções opcionais para a instalação no Linux:

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

- Verifique a versão:
    ```bash
    dotnet --version
    ```

### 2. Clonar o Repositório

Clone o repositório para sua máquina local:

```bash
git clone https://github.com/liviabeatrizml/sistema-de-agendamento-de-servicos-academicos.git
cd sistema-de-agendamento-de-servicos-academicos/
git checkout teste
```

### 3. Instalar Dependências

Instale os pacotes necessários para o projeto:

```bash
dotnet restore
```

### 4. Executar os Testes

Agora você pode rodar os testes com o seguinte comando:

```bash
cd teste/testeDeUnidade ou cd teste/testeDeSistema
dotnet test
```

### 5. Resultados dos Testes

O arcabouço construído sobre os testes realizados pode ser visualizado por meio do site: [Relatório de testes - Sistema ASA](https://sites.google.com/alunos.ufersa.edu.br/relatorio-de-testes-asa/p%C3%A1gina-inicial).