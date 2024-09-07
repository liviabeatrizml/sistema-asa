# 📚 **Sistema de Agendamento de Serviços Acadêmicos - ASA**

## ❓ Apresentação 

Na disciplina de “**Verificação e Validação de Software**” é abordado a garantia da qualidade na aplicação de técnicas de verificação e validação como revisão, inspeção e teste de software. Dessa forma, o sistema proposto deve estar conforme a documentação e deve estar consoante a expectativa do usuário, nisso utilizaremos técnicas e ferramentas de apoio ao teste.

Seguinte, na disciplina de “**Métodos Formais de Engenharia de Software**” é ensinado meios de formalizar e refinar nosso sistema mediante abordagens matemáticas fundamentadas no desenvolvimento de software, onde se estabelece um modelo formal do software. Por sua vez, a formalização do sistema se baseia em Notação Z.

Com os ensinamentos obtidos em sala de aula foi elaborado o projeto **Sistema de Agendamento de Serviços Acadêmicos - ASA**, como forma avaliativa para as duas disciplinas “Verificação e Validação” e “Métodos Formais” ministrada pelo professor Alysson Filgueira Milanez.

## 📖 Descrição

O ASA é um sistema desenvolvido na linguagem **C#** que tem como intuito centralizar e automatizar os processos de agendamento de serviços especializados disponibilizados na Ufersa. Atualmente esse sistema ocorre por meio de páginas no portal da Ufersa via preenchimento de formulário. Dessa forma, como forma de automatização e centralização dos serviços, será desenvolvido um sistema que integrará essas informações em um só local.

## 🎯 Objetivos
> Integralizar os serviços acadêmicos oferecidos pela Ufersa a fim de facilitar e organizar o processo de agendamento.

-   **Facilitar** a edição de informações pelos servidores.
-   **Reduzir o tempo** de solicitação de serviços especializados.
-   Garantir a **transparência** das informações para a comunidade acadêmica.
- **Sincronização** com serviços disponibilizados na universidade.

## 🛠️ Desenvolvimento

### 🏛️ Arquitetura Cliente-Servidor

O sistema é construído com uma arquitetura cliente-servidor, utilizando as seguintes tecnologias e padrões:

- **Padrão Criacional**: *Singleton*
  - Garantia de uma única instância de um objeto e fornecimento de um ponto global de acesso a ele.
 
- **Padrão Arquitetural**: *Model-View-Controller*
  - Garantia da separação de tarefas, facilitando assim a reescrita de alguma parte, e a manutenção do código.

### ⚙️ Tecnologias

- **Back-end**:
  - **Linguagem de Programação**: C#
  - **Framework**: ASP.NET Core Web API
  - **Autenticação e Autorização**: JWT (JSON Web Tokens)

- **Front-end**:
  - **Linguagem de Programação**: C#
  - **Framework**: Blazor WebAssembly
  - **Estilização**: CSS e Bootstrap

- **Armazenamento**:
  - **Banco de Dados**: MySQL
  - **Mapeador**: Entity Framework Core

## 🔗 Links Úteis

- [Apresentação](https://github.com/liviabeatrizml/sistema-de-agendamento-de-servicos-academicos/tree/main/Artefatos/Apresentação_V&V_Metodos.pdf)
- [Artefatos](https://github.com/liviabeatrizml/sistema-de-agendamento-de-servicos-academicos/tree/main/Artefatos)
- [Artigo](https://github.com/liviabeatrizml/sistema-de-agendamento-de-servicos-academicos/blob/main/Artefatos/Artigo_Projeto_ASA.pdf)

## 👥 Equipe

Conheça a equipe do **Sistema de Agendamento de Serviços Acadêmicos**:

| Membros da equipe | Principal função | 
|--------------------|------------------------------| 
| [Antonio Cauê Oliveira Morais](https://github.com/AntonioCaue) | Desenvolvimento Back-end | 
| [Cristiana de Paulo](https://github.com/cristiana0) | Analista de Requisitos | 
| [Eriky Abreu Veloso](https://github.com/ErikyAbreu) | Tech lead: design | 
| [Francisco Renan Leite da Costa](https://github.com/RenanCosta2) | Tech lead: front-end |
| [Geísa Morais Gabriel](https://github.com/Geisa-mg) | Tech lead: QA |
| [Lavinia Dantas de Mesquita](https://github.com/LilPuppet) | Analista de Requisitos |
| [Lívia Beatriz Maia de Lima](https://github.com/liviabeatrizml) | Gerente de Projeto |
| [Maria Lanuza dos Santos Silva](https://github.com/LanuzaSantos) | Tech lead: requisitos |
| [Tiago Amaro Nunes](https://github.com/TiagoDev23) | Tech lead: back-end |

---

Trabalho orientado pelo professor: [Alysson Filgueira Milanez](https://github.com/alyssonfm). 

---