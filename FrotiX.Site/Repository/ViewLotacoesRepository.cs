// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ViewLotacoesRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/                                                     ║
// ║ LOTE       : 24 — Repository                                                 ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Repositório especializado para SQL View ViewLotacoes. Fornece visão         ║
// ║ consolidada de lotações de pessoal por unidade/setor.                         ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository
    {
    public class ViewLotacoesRepository : Repository<ViewLotacoes>, IViewLotacoesRepository
        {
        private new readonly FrotiXDbContext _db;

        public ViewLotacoesRepository(FrotiXDbContext db) : base(db)
            {
            _db = db;
            }

        }
    }


