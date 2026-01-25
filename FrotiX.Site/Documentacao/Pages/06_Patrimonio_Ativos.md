# Guia Patrimonial: Inventário e Movimentação

Focado em ativos auxiliares (equipamentos, móveis, rádios, sobressalentes) que não são veículos, mas pertencem à frota.

## 📦 Inventário (Pages/Patrimonio)
- **Tagueamento:** Cada item possui um código patrimonial único.
- **Categorização:** Organização por Seções e Setores Patrimoniais para facilitar auditorias anuais.

## 🔄 Movimentação (Pages/MovimentacaoPatrimonio)
- **Termo de Responsabilidade:** Geração automática de documento de cautela quando um item é movimentado entre unidades ou entregue a um colaborador.
- **Histórico de Posse:** Rastro completo de por onde o equipamento passou e quem foi o último responsável.

## 🛠 Detalhes Técnicos
- **Hierarquia de Localização:** Utiliza um sistema de Setores/Seções que reflete a estrutura física (ex: Almoxarifado -> Prateleira A).


## 📂 Arquivos do Módulo (Listagem Completa)

### 📦 Gestão de Itens Patrimoniais
- Pages/Patrimonio/Index.cshtml & .cs: Listagem e busca de bens inventariados.
- Pages/Patrimonio/Upsert.cshtml & .cs: Cadastro técnico, marca e número de série de ativos.

### 🔄 Movimentações e Transferências
- Pages/MovimentacaoPatrimonio/Index.cshtml & .cs: Histórico de trocas de guarda e transferências.
- Pages/MovimentacaoPatrimonio/Upsert.cshtml & .cs: Registro de novas movimentações com geração de termo.

### 🏢 Estrutura de Localização
- Pages/SecaoPatrimonial/Index.cshtml & .cs / Upsert.cshtml & .cs: Divisão física de nível 1.
- Pages/SetorPatrimonial/Index.cshtml & .cs / Upsert.cshtml & .cs: Divisão física de nível 2 (Sub-setor).
