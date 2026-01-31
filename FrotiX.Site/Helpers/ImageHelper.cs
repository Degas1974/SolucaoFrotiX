/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ImageHelper.cs                                                                         ║
   ║ 📂 CAMINHO: Helpers/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Utilitários de manipulação de imagens usando System.Drawing. Valida imagem e                   ║
   ║    redimensiona JPEG/PNG para dimensões específicas.                                               ║
   ║    ⚠️ ATENÇÃO: [SupportedOSPlatform(\"windows\")] - funciona apenas em Windows.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • IsImageValid(byte[] imageData)                                                                ║
   ║    • ResizeImage(byte[] imageData, int width, int height)                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.Drawing, System.Drawing.Drawing2D, System.Drawing.Imaging                  ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;

namespace FrotiX.Helpers
    {
    [SupportedOSPlatform("windows")]
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ImageHelper                                                                       │
    // │ 📦 TIPO: Estática                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Oferecer validação e redimensionamento de imagens no backend (Windows-only).
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Uploads e fluxos de processamento de imagem
    // ➡️ CHAMA       : System.Drawing (Image, Bitmap, Graphics)
    
    
    
    // ⚠️ ATENÇÃO:
    // Compatível apenas com Windows devido ao uso de System.Drawing.
    
    
    public static class ImageHelper
        {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: IsImageValid                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : ResizeImage()                                                       │
        // │    ➡️ CHAMA       : Image.FromStream()                                                   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Verificar se um array de bytes representa uma imagem válida.
        
        
        
        // 📥 PARÂMETROS:
        // imageData - Bytes da imagem.
        
        
        
        // 📤 RETORNO:
        // bool - True se a imagem for válida; caso contrário, false.
        
        
        // Param imageData: Bytes da imagem.
        // Returns: True se a imagem for válida; caso contrário, false.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ResizeImage                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Uploads e fluxos de imagens                                         │
        // │    ➡️ CHAMA       : IsImageValid(), Image.FromStream(), Bitmap.Save()                   │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Redimensionar uma imagem JPEG/PNG e retornar o byte[] resultante.
        
        
        
        // 📥 PARÂMETROS:
        // imageData - Bytes da imagem original
        // width - Largura desejada
        // height - Altura desejada
        
        
        
        // 📤 RETORNO:
        // byte[] - Imagem redimensionada em formato JPEG, ou null se inválida.
        
        
        // Param imageData: Bytes da imagem original.
        // Param width: Largura desejada.
        // Param height: Altura desejada.
        // Returns: Imagem redimensionada em formato JPEG, ou null se inválida.
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
                Console.WriteLine($"[ImageHelper] Erro ao redimensionar imagem: {ex.Message}");
                return null;
                }
            }
        }
    }
