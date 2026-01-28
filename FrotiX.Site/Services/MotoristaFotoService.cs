// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MotoristaFotoService.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Serviço para processamento e cache de fotos de motoristas.                   ║
// ║ Redimensiona imagens grandes e mantém cache em memória.                      ║
// ║ IMPORTANTE: Só funciona em Windows ([SupportedOSPlatform("windows")]).       ║
// ║                                                                              ║
// ║ MÉTODOS DISPONÍVEIS:                                                         ║
// ║ - ObterFotoBase64(): Retorna foto em base64 (com cache)                      ║
// ║ - RedimensionarImagem(): Redimensiona para dimensões específicas             ║
// ║                                                                              ║
// ║ CONFIGURAÇÃO DE CACHE:                                                       ║
// ║ - Chave: foto_{motoristaId}                                                  ║
// ║ - TTL: 1 hora                                                                ║
// ║ - Redimensiona se > 50KB para 60x60 pixels                                   ║
// ║                                                                              ║
// ║ USO:                                                                         ║
// ║ - Exibição de miniaturas em grids de motoristas                              ║
// ║ - Otimização de performance em listagens                                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 15                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
    /// <summary>
    /// Serviço de fotos de motoristas com cache e redimensionamento.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class MotoristaFotoService
        {
        private readonly IMemoryCache _cache;

        public MotoristaFotoService(IMemoryCache cache)
            {
            _cache = cache;
            }

        public string ObterFotoBase64(Guid motoristaId, byte[] fotoOriginal)
            {
            if (fotoOriginal == null || fotoOriginal.Length == 0)
                return null;

            string cacheKey = $"foto_{motoristaId}";

            if (_cache.TryGetValue(cacheKey, out string fotoBase64))
                return fotoBase64;

            var resized = fotoOriginal.Length > 50_000
                ? RedimensionarImagem(fotoOriginal, 60, 60)
                : fotoOriginal;

            if (resized == null)
                return null;

            fotoBase64 = $"data:image/jpeg;base64,{Convert.ToBase64String(resized)}";

            _cache.Set(cacheKey, fotoBase64, TimeSpan.FromHours(1));
            return fotoBase64;
            }

        public byte[] RedimensionarImagem(byte[] imagemBytes, int largura, int altura)
            {
            try
                {
                using var inputStream = new MemoryStream(imagemBytes);
                using var imagemOriginal = Image.FromStream(inputStream);
                using var imagemRedimensionada = new Bitmap(largura, altura);
                using var graphics = Graphics.FromImage(imagemRedimensionada);

                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(imagemOriginal, 0, 0, largura, altura);

                using var outputStream = new MemoryStream();
                imagemRedimensionada.Save(outputStream, ImageFormat.Jpeg);
                return outputStream.ToArray();
                }
            catch
                {
                return null;
                }
            }
        }
    }


