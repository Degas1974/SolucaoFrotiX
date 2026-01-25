/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HELPERS - PROCESSAMENTO DE IMAGENS                                                 #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;

namespace FrotiX.Helpers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ImageHelper                                                         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Helper estático para manipulação de imagens (validação e redimensionamento)║
    /// ║    Usa System.Drawing (requer Windows).                                      ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA:                                                              ║
    /// ║    Processa fotos de motoristas e documentos (CNH, CRLV).                    ║
    /// ║                                                                              ║
    /// ║ ⚠️  REQUISITO:                                                                ║
    /// ║    Windows (System.Drawing.Common). Use ImageSharp em Linux/Mac.            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES:                                                                  ║
    /// ║    • IsImageValid() → Valida se bytes representam imagem                     ║
    /// ║    • ResizeImage() → Redimensiona mantendo proporção                         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [SupportedOSPlatform("windows")]
    public static class ImageHelper
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: IsImageValid                                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica se um array de bytes representa uma imagem válida.               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static bool IsImageValid(byte[] imageData)
            {
            try
                {
                using var ms = new MemoryStream(imageData);
                using var img = Image.FromStream(ms, false, true);
                return true;
                }
            catch
                {
                return false;
                }
            }

        /// <summary>
        /// Redimensiona uma imagem JPEG ou PNG em byte[], retornando o byte[] redimensionado ou null.
        /// </summary>
        public static byte[] ResizeImage(byte[] imageData, int width, int height)
            {
            if (imageData == null || imageData.Length == 0 || !IsImageValid(imageData))
                return null;

            try
                {
                using var inputStream = new MemoryStream(imageData);
                using var originalImage = Image.FromStream(inputStream);
                using var resizedImage = new Bitmap(width, height);
                using var graphics = Graphics.FromImage(resizedImage);

                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(originalImage, 0, 0, width, height);

                using var outputStream = new MemoryStream();
                resizedImage.Save(outputStream, ImageFormat.Jpeg);
                return outputStream.ToArray();
                }
            catch (Exception ex)
                {
                Alerta.TratamentoErroComLinha("ImageHelper.cs", "ResizeImage", ex);
                return null;
                }
            }
        }
    }

