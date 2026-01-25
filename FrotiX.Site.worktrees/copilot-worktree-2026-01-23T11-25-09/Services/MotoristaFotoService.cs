/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - PROCESSAMENTO DE FOTOS DE MOTORISTAS                                    #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;
using Microsoft.Extensions.Caching.Memory;

namespace FrotiX.Services
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MotoristaFotoService                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Serviço de processamento e cache de fotos de motoristas. Redimensiona    ║
    /// ║    imagens grandes e mantém versões otimizadas em cache para performance.    ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Otimização de performance. Evita carregar fotos grandes repetidamente,   ║
    /// ║    reduzindo consumo de banda e melhorando tempo de resposta das APIs.       ║
    /// ║                                                                              ║
    /// ║ ⚠️  REQUISITO DE PLATAFORMA:                                                 ║
    /// ║    Requer Windows (System.Drawing.Common). Use SixLabors.ImageSharp em      ║
    /// ║    ambientes Linux/Mac.                                                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • ObterFotoBase64() → Retorna foto em base64 (cached)                     ║
    /// ║    • RedimensionarImagem() → Reduz tamanho para 60x60px                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Serviço de otimização                                   ║
    /// ║    • Arquivos relacionados: Motorista (Model), MotoristaController          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [SupportedOSPlatform("windows")]
    public class MotoristaFotoService
    {
        private readonly IMemoryCache _cache;

        public MotoristaFotoService(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ObterFotoBase64                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna foto do motorista em formato base64 (data URI). Usa cache de 1h.  ║
        /// ║    Redimensiona automaticamente se maior que 50KB.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • motoristaId: GUID do motorista                                          ║
        /// ║    • fotoOriginal: Array de bytes da foto                                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • string: data:image/jpeg;base64,... ou null                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public string ObterFotoBase64(Guid motoristaId, byte[] fotoOriginal)
        {
            // [REGRA] Validação de entrada
            if (fotoOriginal == null || fotoOriginal.Length == 0)
                return null;

            // [DADOS] Chave de cache única por motorista
            string cacheKey = $"foto_{motoristaId}";

            // [PERFORMANCE] Retorna do cache se disponível
            if (_cache.TryGetValue(cacheKey, out string fotoBase64))
                return fotoBase64;

            // [LOGICA] Redimensiona se maior que 50KB (otimização)
            var resized = fotoOriginal.Length > 50_000
                ? RedimensionarImagem(fotoOriginal, 60, 60)
                : fotoOriginal;

            if (resized == null)
                return null;

            // [DADOS] Converte para data URI base64
            fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(resized)}";

            // [PERFORMANCE] Armazena em cache por 1 hora
            _cache.Set(cacheKey, fotoBase64, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), Size = 1 });
            return fotoBase64;
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RedimensionarImagem                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Redimensiona imagem para dimensões específicas usando GDI+ (Windows).     ║
        /// ║    Prioriza velocidade sobre qualidade para thumbnails.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • imagemBytes: Bytes da imagem original                                   ║
        /// ║    • largura, altura: Dimensões alvo (pixels)                                ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • byte[]: Imagem redimensionada em JPEG ou null se falhar                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public byte[] RedimensionarImagem(byte[] imagemBytes, int largura, int altura)
        {
            try
            {
                // [DADOS] Carrega imagem do array de bytes
                using var inputStream = new MemoryStream(imagemBytes);
                using var imagemOriginal = Image.FromStream(inputStream);
                using var imagemRedimensionada = new Bitmap(largura, altura);
                using var graphics = Graphics.FromImage(imagemRedimensionada);

                // [PERFORMANCE] Configurações otimizadas para velocidade (thumbnails)
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(imagemOriginal, 0, 0, largura, altura);

                // [DADOS] Converte para JPEG e retorna bytes
                using var outputStream = new MemoryStream();
                imagemRedimensionada.Save(outputStream, ImageFormat.Jpeg);
                return outputStream.ToArray();
            }
            catch
            {
                // [REGRA] Retorna null em caso de falha (imagem corrompida, etc)
                return null;
            }
        }
    }
}


