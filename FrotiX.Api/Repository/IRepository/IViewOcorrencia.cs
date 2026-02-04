/* ****************************************************************************************
 * ⚡ ARQUIVO: IViewOcorrencia.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Interface repository para acesso à View de Ocorrências de Viagem,
 *                   consulta e manipulação de dados de eventos/problemas registrados
 *                   durante viagens
 *
 * 📥 ENTRADAS     : Parâmetros de filtro (data, viagem, tipo), transações CRUD
 *
 * 📤 SAÍDAS       : List<ViewOcorrencia>, SelectListItem para dropdowns, dados agregados
 *
 * 🔗 CHAMADA POR  : OcorrenciaViagemController, relatórios de viagens, dashboards
 *
 * 🔄 CHAMA        : IRepository<ViewOcorrencia> (CRUD padrão), EF Core DbSet
 *
 * 📦 DEPENDÊNCIAS : IRepository[T], ViewOcorrencia model, SelectListItem, EF Core
 *
 * 📝 OBSERVAÇÕES  : Repository especializado para View de Ocorrências
 *                   Métodos: GetViewOcorrenciaListForDropDown() - populate UI controls
 *                   Update() - sincronizar dados de ocorrências com changetracker
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiXApi.Repository.IRepository
    {
    public interface IViewOcorrenciaRepository : IRepository<ViewOcorrencia>
        {

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetViewOcorrenciaListForDropDown
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar lista de ocorrências formatada para popular controles
         *                   de seleção (dropdown/combobox) na interface
         *
         * 📥 ENTRADAS     : Nenhuma (usa contexto EF Core)
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> com Text (descrição) e Value (Id)
         *
         * ⬅️ CHAMADO POR  : Controllers ao popular formulários, páginas Razor
         *
         * ➡️ CHAMA        : Db.ViewOcorrencia.Select() [EF Core LINQ]
         *
         * 📝 OBSERVAÇÕES  : Otimizado para binding em ASP.NET Core MVC/Razor Pages
         ****************************************************************************************/
        IEnumerable<SelectListItem> GetViewOcorrenciaListForDropDown();

        /****************************************************************************************
         * ⚡ FUNÇÃO: Update
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Atualizar registro de ocorrência sincronizando com Change Tracker
         *                   do Entity Framework
         *
         * 📥 ENTRADAS     : viewOcorrencia [ViewOcorrencia] - objeto com dados modificados
         *
         * 📤 SAÍDAS       : void (modifica estado EF Core para detecção de mudanças)
         *
         * ⬅️ CHAMADO POR  : Controllers ao processar edições de ocorrências
         *
         * ➡️ CHAMA        : DbContext.Update() [EF Core ChangeTracker]
         *
         * 📝 OBSERVAÇÕES  : Preparação para SaveChanges(), não persiste imediatamente
         ****************************************************************************************/
        void Update(ViewOcorrencia viewOcorrencia);

        }
    }


