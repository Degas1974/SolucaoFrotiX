/* ****************************************************************************************
 * ⚡ ARQUIVO: HigienizacaoDto.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Padronizar DTOs de higienização de dados (origens e destinos).
 *
 * 📥 ENTRADAS     : Listas de valores existentes e novos valores corrigidos.
 *
 * 📤 SAÍDAS       : Payloads para correção e saneamento de dados de viagem.
 *
 * 🔗 CHAMADA POR  : Rotinas administrativas de higienização e ajustes.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : System.Collections.Generic.
 **************************************************************************************** */

using System.Collections.Generic;


namespace FrotiX.Models.DTO
    {
    /****************************************************************************************
     * ⚡ DTO: HigienizacaoDto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Consolidar informações de higienização (tipo e novos valores).
     *
     * 📥 ENTRADAS     : Tipo, lista de valores antigos e novo valor aplicado.
     *
     * 📤 SAÍDAS       : Dados para execução de correções em lote.
     *
     * 🔗 CHAMADA POR  : Serviços administrativos de higienização.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class HigienizacaoDto
        {
        // Tipo de correção (origem/destino).
        public string Tipo { get; set; }               // origem ou destino
        // Valores antigos.
        public List<string> AntigosValores { get; set; }
        // Novo valor aplicado.
        public string NovosValores { get; set; }
        }

    /****************************************************************************************
     * ⚡ DTO: CorrecaoOrigemDto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Definir correção para origens de viagem.
     *
     * 📥 ENTRADAS     : Lista de origens atuais e nova origem.
     *
     * 📤 SAÍDAS       : Payload para atualização de origens.
     *
     * 🔗 CHAMADA POR  : Serviços de higienização de origens.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class CorrecaoOrigemDto
        {
        // Lista de origens atuais.
        public List<string> Origens { get; set; }
        // Nova origem.
        public string NovaOrigem { get; set; }
        }

    /****************************************************************************************
     * ⚡ DTO: CorrecaoDestinoDto
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Definir correção para destinos de viagem.
     *
     * 📥 ENTRADAS     : Lista de destinos atuais e novo destino.
     *
     * 📤 SAÍDAS       : Payload para atualização de destinos.
     *
     * 🔗 CHAMADA POR  : Serviços de higienização de destinos.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class CorrecaoDestinoDto
        {
        // Lista de destinos atuais.
        public List<string> Destinos { get; set; }
        // Novo destino.
        public string NovoDestino { get; set; }
        }

    }


