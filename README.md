# Como rodar a aplicação

Antes de rodar a aplicação, certifique-se de que você tem o **.NET 8.0 SDK** instalado na sua máquina.

Download no windows: https://dotnet.microsoft.com/pt-br/download/dotnet/8.0

Instruções para linux:

1. Adicione o repositório da Microsoft:
```bash
sudo apt-get update && sudo apt-get install -y wget
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```
2. Instale o SDK do .NET 8.0:
```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

3. Verifique a versão
```bash
dotnet --version
```

Caso tenha alguma dificuldade na instalação consulte a <a href="https://learn.microsoft.com/pt-br/dotnet/core/install/linux" target="_blank"> documentação oficial do .NET </a>

Após isso, siga os seguintes passos para executar a aplicação:

1. Clone este repositório
```bash
git clone https://github.com/liviabeatrizml/sistema-de-agendamento-de-servicos-academicos.git
```
2. Acesse a pasta do projeto
```bash
cd sistema-de-agendamento-de-servicos-academicos/
```
3. Execute a aplicação
```bash
dotnet watch run --project Front-end/Front-end.csproj
```
4. O servidor iniciará na porta:5156 - acesse <http://localhost:5156>
