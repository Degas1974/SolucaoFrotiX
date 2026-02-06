/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Configurações avançadas do modelo EF Core (precisão decimal e ajustes globais).
 * =========================================================================================
 */

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    public partial class FrotiXDbContext
    {
        /// <summary>
        /// Aplica configurações de precisão decimal padrão para todas as propriedades decimal.
        /// Chamado pelo OnModelCreating no arquivo FrotiXDbContext.cs
        /// </summary>
        private static void ApplyDecimalPrecisionDefaults(ModelBuilder modelBuilder)
        {
            try
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var property in entityType.GetProperties()
                                 .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                    {
                        if (property.GetPrecision() == null && string.IsNullOrWhiteSpace(property.GetColumnType()))
                        {
                            property.SetPrecision(18);
                            property.SetScale(4);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha(ex, "FrotiXDbContext.ModelConfiguration.cs", "ApplyDecimalPrecisionDefaults");
            }
        }
    }
}
