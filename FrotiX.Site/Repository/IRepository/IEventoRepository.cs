// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IEventoRepository.cs                                            ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Evento. Define contrato para operações com       ║
// ║ eventos/solenidades incluindo query otimizada paginada.                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS DEFINIDOS                                                            ║
// ║ • GetEventoListForDropDown() → SelectList para dropdowns                     ║
// ║ • GetEventosPaginadoAsync() → Lista paginada com cálculo de custos           ║
// ║ • Update() → Atualização de evento                                           ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    public interface IEventoRepository : IRepository<Evento>
        {

        IEnumerable<SelectListItem> GetEventoListForDropDown();

        void Update(Evento evento);

        
        // Lista eventos com paginação e otimização
        
        Task<(List<EventoListDto> eventos, int totalItems)> GetEventosPaginadoAsync(
            int page ,
            int pageSize ,
            string filtroStatus = null
        );

    }
    }


