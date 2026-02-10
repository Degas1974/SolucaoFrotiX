# Guia de Governança: Administração e Segurança

O cérebro administrativo do FrotiX, focado em auditoria, controle de acesso e saúde do sistema.

## 👥 Gestão de Usuários (Pages/Usuarios)
- **Controle de Acessos (Claims):** O FrotiX utiliza um sistema robusto de permissões granulares. Um usuário pode ter acesso para "Ver Viagens" mas não para "Aprovar Glosas".
- **Log de Atividade:** Registro de quem visualizou ou alterou dados sensíveis (Auditoria).

## ⚙️ Administração (Pages/Administracao)
- **Higienização de Dados:** Ferramentas para remover registros órfãos ou corrigir quilometragens inconsistentes em massa.
- **Estatísticas Globais:** Gerador de indicadores de performance que recalcula os KPIs de todo o sistema.

## 🔔 Alertas e Notificações (Pages/AlertasFrotiX)
- **Alertas Preditivos:** O sistema avisa sobre vencimentos de CNH, Seguros, Documentação do Carro e Revisões (por data ou por KM).
- **Notificações em Tempo Real:** Utiliza SignalR para "empurrar" avisos urgentes na Navbar do gestor.

## 🛠 Detalhes Técnicos
- **Middleware de Erros:** Captura todas as exceções do sistema e as registra em uma tabela de log (LogErros), permitindo aos desenvolvedores diagnosticar problemas sem acesso direto ao servidor.


## 📂 Arquivos do Módulo (Listagem Completa)

### 👥 Gestão de Usuários e Permissões
- Pages/Usuarios/Index.cshtml & .cs: Administração da lista de usuários.
- Pages/Usuarios/Upsert.cshtml & .cs: Gestão de perfil, senha e unidades vinculadas.
- Pages/Usuarios/Recursos.cshtml & .cs: Árvore de telas e funções permitidas.
- Pages/Usuarios/UpsertRecurso.cshtml & .cs: Criação de novas chaves de permissão.
- Pages/Usuarios/InsereRecursosUsuarios.cshtml & .cs: Atribuição em massa de permissões para perfis.
- Pages/Usuarios/Registrar.cshtml & .cs: Fluxo de auto-cadastro (quando habilitado).
- Pages/Usuarios/Report.cshtml & .cs: Auditoria de quem tem acesso a que.

### ⚙️ Administração e Ferramentas de Sistema
- Pages/Administracao/DashboardAdministracao.cshtml & .cs: KPIs de saúde técnica do sistema.
- Pages/Administracao/LogErros.cshtml & .cs: Visualizador de exceções não tratadas para debugging.
- Pages/Administracao/HigienizarViagens.cshtml & .cs: Wizard para correção de trajetos e KMs discrepantes.
- Pages/Administracao/CalculaCustoViagensTotal.cshtml & .cs: Engine de reprocessamento financeiro de viagens passadas.
- Pages/Administracao/AjustaCustosViagem.cshtml & .cs: Correção manual pontual de valores de deslocamento.
- Pages/Administracao/GerarEstatisticasViagens.cshtml & .cs: Task runner para reconstrução de cubos de dados.
- Pages/Administracao/GestaoRecursosNavegacao.cshtml & .cs: Configuração dinâmica do menu 
av.json.
- Pages/Administracao/DocGenerator.cshtml & .cs: Factory de templates para documentos automáticos.

### 🔔 Mensageria e Alertas (WhatsApp/SignalR)
- Pages/AlertasFrotiX/AlertasFrotiX.cshtml & .cs: Painel central de notificações de vencimento.
- Pages/AlertasFrotiX/Upsert.cshtml & .cs: Configuração de gatilhos (Triggers) para novos alertas.
- Pages/WhatsApp/Index.cshtml & .cs: Monitoramento e envio de notificações via API de mensageria.
