/* ****************************************************************************************
 * ⚡ ARQUIVO: LookupsDto.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Disponibilizar records imutáveis para lookups de motoristas e veículos.
 *
 * 📥 ENTRADAS     : Identificadores e descrições de entidades.
 *
 * 📤 SAÍDAS       : Registros leves para combos e seletores.
 *
 * 🔗 CHAMADA POR  : APIs e serviços de consulta rápida.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : System.
 **************************************************************************************** */

using System;

namespace FrotiX.Models.DTO // <-- ajuste para o namespace do seu projeto
    {
    /****************************************************************************************
     * ⚡ RECORD: MotoristaData
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar motorista em lookups simples.
     *
     * 📥 ENTRADAS     : MotoristaId e Nome.
     *
     * 📤 SAÍDAS       : Record imutável para UI e APIs.
     *
     * 🔗 CHAMADA POR  : Endpoints de busca rápida.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public sealed record MotoristaData(Guid MotoristaId, string Nome);

    /****************************************************************************************
     * ⚡ RECORD: VeiculoData
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar veículo em lookups simples.
     *
     * 📥 ENTRADAS     : VeiculoId e Descricao.
     *
     * 📤 SAÍDAS       : Record imutável para UI e APIs.
     *
     * 🔗 CHAMADA POR  : Endpoints de busca rápida.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public sealed record VeiculoData(Guid VeiculoId, string Descricao);

    /****************************************************************************************
     * ⚡ RECORD: VeiculoReservaData
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar veículo reserva em lookups simples.
     *
     * 📥 ENTRADAS     : VeiculoId e Descricao.
     *
     * 📤 SAÍDAS       : Record imutável para UI e APIs.
     *
     * 🔗 CHAMADA POR  : Endpoints de busca rápida.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public sealed record VeiculoReservaData(Guid VeiculoId, string Descricao);
    }
