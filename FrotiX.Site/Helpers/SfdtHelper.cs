/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: SfdtHelper.cs                                                                         ║
   ║ 📂 CAMINHO: Helpers/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Helper Syncfusion para conversão de documentos Word (DOCX -> PDF -> PNG).                       ║
   ║    Usa DocIO/DocIORenderer, PdfViewer e SkiaSharp para gerar imagem da 1ª página.                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • SalvarImagemDeDocx(byte[] docxBytes)                                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Syncfusion.DocIO, Syncfusion.DocIORenderer, Syncfusion.Pdf, SkiaSharp            ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.IO;
using System.Text;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.EJ2.PdfViewer;
using SkiaSharp;

namespace FrotiX.Helpers
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: SfdtHelper                                                                        │
    // │ 📦 TIPO: Estática                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Converter DOCX em PNG (1ª página) usando pipeline Syncfusion + SkiaSharp.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Fluxos de geração de imagens/documentos
    // ➡️ CHAMA       : WordDocument, DocIORenderer, PdfRenderer, SKImage
    
    
    public static class SfdtHelper
        {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SalvarImagemDeDocx                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Uploads/preview de documentos                                       │
        // │    ➡️ CHAMA       : DocIORenderer.ConvertToPDF(), PdfRenderer.ExportAsImage()           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Converter um DOCX em PNG (1ª página) e retornar os bytes da imagem.
        
        
        
        // 📥 PARÂMETROS:
        // docxBytes - Conteúdo do arquivo DOCX.
        
        
        
        // 📤 RETORNO:
        // byte[] - Imagem PNG gerada a partir da 1ª página.
        
        
        // Param docxBytes: Conteúdo do arquivo DOCX.
        // Returns: Imagem PNG gerada a partir da 1ª página.
        public static byte[] SalvarImagemDeDocx(byte[] docxBytes)
            {
            using var docStream = new MemoryStream(docxBytes);
            using var document = new WordDocument(docStream, FormatType.Docx);
            using var renderer = new DocIORenderer();
            using var pdfDoc = renderer.ConvertToPDF(document);

            using var pdfStream = new MemoryStream();
            pdfDoc.Save(pdfStream);
            byte[] pdfBytes = pdfStream.ToArray();

            using var input = new MemoryStream(pdfBytes);
            using var loadedPdf = new PdfLoadedDocument(input);
            var pdfRenderer = new PdfRenderer();
            pdfRenderer.Load(input);

            using var bitmap = pdfRenderer.ExportAsImage(0);
            using var img = SKImage.FromBitmap(bitmap);
            using var encoded = img.Encode(SKEncodedImageFormat.Png, 100);
            return encoded.ToArray();
            }
        }
    }
