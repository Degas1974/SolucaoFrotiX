# 📅 Guia de Gestão de Escalas e Turnos

> **Status**: ✅ **PROSA LEVE**  
> **Área**: Operação e Logística (Escalonamento)  
> **Padrão**: Escala Diária + SignalR Hub

---

## 📖 Visão Geral

O módulo de **Escalas** é responsável por organizar a jornada de trabalho dos motoristas, garantindo que cada veículo tenha um condutor designado e que as folgas, férias e recessos sejam respeitados. Ele substitui as antigas planilhas manuais por um sistema dinâmico e integrado.

---

## 🏗️ Estrutura de Escalonamento

### 1. Tipos de Serviço e Turnos

**O que faz?** Define a natureza do trabalho (ex: Administrativo, Operacional, Plantão) e os horários (ex: 12x36, Diurno, Noturno).

- **Flexibilidade:** Permite configurar janelas de tempo específicas para cada tipo de contrato.

### 2. Escala Diária (`Pages/Escalas`)

**O que faz?** É o coração do módulo. Permite visualizar em um quadro quem está de serviço, quem está de folga e quem é a cobertura.

- **Vincular Motorista x Veículo:** Garante que o motorista tenha o veículo associado corretamente para o seu turno.
- **Gestão de Ausências:** O sistema cruza dados de Férias e Folgas para evitar que um motorista ausente seja escalado por engano.

### 3. Ficha de Escala (`FichaEscalas.cshtml`)

**O que faz?** Gera um documento visual (tipo espelho) para impressão ou visualização em pátio, detalhando todos os postos de trabalho do dia.

---

## 🧠 Inteligências e Sincronia

### 1. EscalaHub (SignalR)

As atualizações de escala são enviadas em tempo real para os painéis de visualização. Se um encarregado altera um motorista no servidor, a tela do pátio atualiza o nome sem necessidade de recarregar a página.

### 2. Validação de Cobertura

Ao lançar uma folga, o sistema sugere automaticamente "coberturas" baseadas na disponibilidade de outros motoristas do mesmo turno que não estejam escalados.

---

## 🛠 Detalhes Técnicos para Desenvolvedores

- **Complexidade de Dados:** O sistema utiliza views complexas (`ViewEscalasCompletas`) para unir dados de motoristas, veículos, turnos e tipos de serviço em uma única consulta performática.
- **API Dedicada:** `EscalaController_Api` fornece endpoints JSON rápidos para o grid de escalas, suportando filtragem por data e unidade.

---

## 📂 Arquivos do Módulo (Listagem Completa)

### 📅 Interface de Escalas

- `Pages/Escalas/ListaEscala.cshtml` & `.cs`: Quadro geral de visualização das escalas.
- `Pages/Escalas/UpsertCEscala.cshtml` & `.cs`: Cadastro/Criação de novas escalas.
- `Pages/Escalas/UpsertEEscala.cshtml` & `.cs`: Edição de escalas existentes.
- `Pages/Escalas/FichaEscalas.cshtml` & `.cs`: Visualização formatada para impressão (Mirror).

### 🎮 Controladores e Tempo Real

- `Controllers/EscalaController.cs`: Regras de negócio e navegação.
- `Controllers/EscalaController_Api.cs`: Interface de dados para DataTables/Grids.
- `Hubs/EscalaHub.cs`: Motor de atualização em tempo real via WebSockets.

### 📦 Modelos e Repositórios

- `Models/Cadastros/Escalas.cs`: Agrupador de modelos (Escala, Turno, TipoServico).
- `Models/Cadastros/EscalaDiaria.cs`: Registro unitário de escala por dia.
- `Models/Cadastros/CoberturaFolga.cs`: Controle de substituições.
- `Models/Cadastros/ObservacoesEscala.cs`: Notas e intercorrências.
- `Models/Cadastros/FiltroEscala.cs`: DTO para pesquisa.
- `Models/Views/ViewEscalasCompletas.cs`: Projeção de dados para interface.
- `Models/Views/ViewMotoristaVez.cs`: Lógica de fila de saída.
- `Models/Views/ViewStatusMotoristas.cs`: Visão rápida de disponibilidade.
- `Repository/EscalasRepository.cs` & `IEscalasRepository.cs`: Camada de acesso a dados (Multi-classes).

### 📜 Scripts de Interface

- `wwwroot/js/cadastros/CriarEscala.js`: Inteligência de formulário de nova escala.
- `wwwroot/js/cadastros/EditarEscala.js`: Lógica de alteração e validação.
- `wwwroot/js/cadastros/ListaEscala.js`: Controle do grid dinâmico.
