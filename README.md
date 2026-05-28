# 📚 Clube da Leitura - Web Application

## 📖 Sobre o Projeto

O **Clube da Leitura** é uma aplicação web desenvolvida em **ASP.NET Core MVC** com o objetivo de auxiliar no gerenciamento de um clube de leitura.

O sistema permite controlar:

- 📦 Caixas de revistas
- 📕 Revistas
- 👥 Amigos
- 🔄 Empréstimos

A aplicação foi desenvolvida seguindo o padrão arquitetural **MVC (Model-View-Controller)**, utilizando separação em módulos para facilitar a organização, manutenção e escalabilidade do projeto.

---

# 🚀 Funcionalidades

## 📦 Controle de Caixas

- Cadastro de caixas
- Edição de caixas
- Exclusão de caixas
- Listagem de caixas
- Controle de cor
- Definição de dias de empréstimo
- Validações de dados

---

## 📕 Controle de Revistas

- Cadastro de revistas
- Edição de revistas
- Exclusão de revistas
- Listagem de revistas
- Associação de revistas às caixas
- Controle de status:
  - Disponível
  - Emprestada
  - Reservada
- Validação de duplicidade de edição

---

## 👥 Controle de Amigos

- Cadastro de amigos
- Edição de amigos
- Exclusão de amigos
- Listagem de amigos
- Controle de telefone
- Controle de responsável

---

## 🔄 Controle de Empréstimos

- Registro de empréstimos
- Registro de devoluções
- Controle automático de atraso
- Atualização de status:
  - Aberto
  - Concluído
  - Atrasado
- Filtro por amigo
- Filtro por status
- Verificação de disponibilidade de revistas
- Bloqueio de múltiplos empréstimos ativos para o mesmo amigo

---

# 🧱 Arquitetura do Projeto

O sistema foi organizado em módulos seguindo separação de responsabilidades.

## Estrutura Geral

```text
ClubeDaLeitura.WebApplication
│
├── ModuloAmigo
├── ModuloCaixa
├── ModuloEmprestimo
├── ModuloHome
├── ModuloRevista
│
├── Compartilhado
├── wwwroot
└── Program.cs
```

---

# 🏗️ Estrutura dos Módulos

Cada módulo possui:

```text
ModuloX
│
├── Apresentacao
│   ├── Views
│   ├── Controller
│   └── ViewModels
│
├── Dominio
│   ├── Entidades
│   ├── Interfaces
│   └── Regras de Negócio
│
└── Infraestrutura
    └── Repositórios
```

---

# 🖥️ Tecnologias Utilizadas

## Backend

- ASP.NET Core MVC
- C#
- .NET

## Frontend

- Razor Views
- HTML5
- CSS3
- Bootstrap
- JavaScript

## Persistência

- Serialização em arquivos

---

# 🎨 Interface do Sistema

O sistema utiliza:

- Layout responsivo
- Tema moderno com gradientes
- Componentes estilizados com Bootstrap
- Tabelas personalizadas
- Formulários com validação
- Feedback visual para ações do usuário

---

# 📂 Organização das Camadas

## 📌 Apresentação

Responsável pela interface visual da aplicação.

Contém:

- Controllers
- Views
- ViewModels

### Exemplo:

```text
ModuloRevista/Apresentacao
```

---

## 📌 Domínio

Responsável pelas regras de negócio.

Contém:

- Entidades
- Enumerações
- Interfaces de repositório
- Validações

### Exemplo:

```csharp
public enum StatusEmprestimo
{
    Aberto,
    Concluido,
    Atrasado
}
```

---

## 📌 Infraestrutura

Responsável pelo acesso e persistência dos dados.

Contém:

- Repositórios
- Serialização
- Manipulação de arquivos

---

# 🔄 Fluxo de Empréstimos

## Registrar Empréstimo

1. Usuário seleciona um amigo
2. Usuário seleciona uma revista disponível
3. Sistema valida:
   - Existência do amigo
   - Existência da revista
   - Disponibilidade da revista
   - Empréstimos ativos do amigo
4. Empréstimo é registrado
5. Revista passa para status "Emprestada"

---

## Registrar Devolução

1. Usuário seleciona o empréstimo
2. Sistema registra a data atual
3. Empréstimo passa para status "Concluído"
4. Revista volta para status "Disponível"

---

## Controle Automático de Atraso

Ao listar empréstimos:

- O sistema verifica automaticamente os prazos
- Empréstimos vencidos são alterados para:

```text
Atrasado
```

---

# ✅ Validações Implementadas

## Revistas

- Título obrigatório
- Número da edição maior que zero
- Data válida
- Caixa obrigatória
- Não permitir revistas duplicadas

---

## Empréstimos

- Amigo obrigatório
- Revista obrigatória
- Revista disponível
- Um empréstimo ativo por amigo
- Datas válidas

---

# 🎯 Padrões Utilizados

- MVC (Model-View-Controller)
- Repository Pattern
- Separação por módulos
- ViewModels
- Injeção de Dependência
- Responsabilidade Única

---

# ⚙️ Configuração do Projeto

## Pré-requisitos

- .NET SDK instalado
- Visual Studio ou VS Code

---

# ▶️ Como Executar

## 1. Clonar o repositório

```bash
git clone <URL_DO_REPOSITORIO>
```

---

## 2. Entrar na pasta do projeto

```bash
cd ClubeDaLeitura.WebApplication
```

---

## 3. Restaurar dependências

```bash
dotnet restore
```

---

## 4. Executar a aplicação

```bash
dotnet run
```

---

# 📁 Estrutura de Diretórios

```text
ClubeDaLeitura.WebApplication
│
├── Compartilhado
│
├── ModuloAmigo
│   ├── Apresentacao
│   ├── Dominio
│   └── Infraestrutura
│
├── ModuloCaixa
│   ├── Apresentacao
│   ├── Dominio
│   └── Infraestrutura
│
├── ModuloEmprestimo
│   ├── Apresentacao
│   ├── Dominio
│   └── Infraestrutura
│
├── ModuloHome
│   └── Apresentacao
│
├── ModuloRevista
│   ├── Apresentacao
│   ├── Dominio
│   └── Infraestrutura
│
├── wwwroot
│   ├── css
│   ├── js
│   └── imagens
│
└── Program.cs
```

---

# 🧠 Conceitos Aplicados

Durante o desenvolvimento foram aplicados conceitos como:

- Programação Orientada a Objetos
- Encapsulamento
- Modularização
- Persistência de dados
- Arquitetura MVC
- CRUD
- Regras de negócio
- Injeção de dependência
- Responsividade
- UX/UI

---

# 📌 Possíveis Melhorias Futuras

- Banco de dados relacional
- Sistema de autenticação
- Dashboard com gráficos
- Relatórios PDF
- Controle de reservas
- Notificações de atraso
- API REST
- Deploy em nuvem
- Upload de imagens

---

# 👨‍💻 Autor

Projeto desenvolvido por Gustavo Tessaro e Alec Luí para fins acadêmicos.

