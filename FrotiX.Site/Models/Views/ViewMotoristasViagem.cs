// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewMotoristasViagem.cs                                            ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model simplificado para seleção de motoristas em viagens.              ║
// ║ Contém apenas dados essenciais para dropdowns e exibição rápida.            ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • MotoristaId - Identificador único do motorista                            ║
// ║ • Nome - Nome do motorista para exibição                                    ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • MotoristaCondutor - Nome formatado para seleção                           ║
// ║ • TipoCondutor - Tipo (Titular, Reserva, Terceiro)                          ║
// ║ • Foto - Foto do motorista para exibição em cards                           ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Seleção de motorista em cadastro de viagens                               ║
// ║ • Cards de motoristas disponíveis                                           ║
// ║ • Dropdowns com foto e tipo de condutor                                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;

namespace FrotiX.Models.Views
    {
    public class ViewMotoristasViagem
        {

        public Guid MotoristaId { get; set; }

        public string? Nome { get; set; }

        public bool Status { get; set; }

        public string? MotoristaCondutor { get; set; }

        public string? TipoCondutor { get; set; }

        public byte[]? Foto { get; set; }


        }
    }


