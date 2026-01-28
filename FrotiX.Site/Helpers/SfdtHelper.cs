// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: SfdtHelper.cs                                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Helper para conversão de documentos usando Syncfusion e SkiaSharp.           ║
// ║ Converte DOCX → PDF → PNG para preview de documentos.                        ║
// ║                                                                              ║
// ║ MÉTODOS DISPONÍVEIS:                                                         ║
// ║ - SalvarImagemDeDocx() → Converte byte[] DOCX para byte[] PNG                ║
// ║                                                                              ║
// ║ PIPELINE DE CONVERSÃO:                                                       ║
// ║ 1. DOCX → WordDocument (Syncfusion.DocIO)                                    ║
// ║ 2. WordDocument → PdfDocument (Syncfusion.DocIORenderer)                     ║
// ║ 3. PDF → SKBitmap (Syncfusion.EJ2.PdfViewer + SkiaSharp)                     ║
// ║ 4. SKBitmap → PNG byte[]                                                     ║
// ║                                                                              ║
// ║ DEPENDÊNCIAS:                                                                ║
// ║ - Syncfusion.DocIO, Syncfusion.DocIORenderer                                 ║
// ║ - Syncfusion.Pdf, Syncfusion.EJ2.PdfViewer                                   ║
// ║ - SkiaSharp                                                                  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 12                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
    /// <summary>
    /// Helper para conversão DOCX → PDF → PNG usando Syncfusion e SkiaSharp.
    /// </summary>
    public static class SfdtHelper
        {
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

