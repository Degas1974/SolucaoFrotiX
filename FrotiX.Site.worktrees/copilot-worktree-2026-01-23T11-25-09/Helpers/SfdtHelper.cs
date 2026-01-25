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
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  📄 ARQUIVO: SfdtHelper.cs (Syncfusion Document Conversion Helper)          ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Helper para conversão de documentos DOCX para imagem PNG.                  ║
    /// ║  Utiliza Syncfusion DocIO + DocIORenderer + PdfRenderer + SkiaSharp.        ║
    /// ║                                                                              ║
    /// ║  PIPELINE DE CONVERSÃO:                                                      ║
    /// ║  byte[] DOCX → WordDocument → PDF → PdfRenderer → Bitmap → PNG byte[]       ║
    /// ║                                                                              ║
    /// ║  TECNOLOGIAS:                                                                ║
    /// ║  - Syncfusion.DocIO: Leitura de arquivos DOCX (WordDocument).               ║
    /// ║  - Syncfusion.DocIORenderer: Conversão DOCX → PDF (ConvertToPDF).           ║
    /// ║  - Syncfusion.EJ2.PdfViewer: Renderização PDF → Bitmap (PdfRenderer).       ║
    /// ║  - SkiaSharp: Codificação Bitmap → PNG (SKImage.Encode).                    ║
    /// ║                                                                              ║
    /// ║  MÉTODO PRINCIPAL:                                                           ║
    /// ║  - SalvarImagemDeDocx(): Converte DOCX → PNG (primeira página).             ║
    /// ║                                                                              ║
    /// ║  APLICAÇÃO NO SISTEMA:                                                       ║
    /// ║  - Preview de documentos DOCX em modal/dashboard.                           ║
    /// ║  - Geração de thumbnails de documentos para galeria.                        ║
    /// ║  - Exportação de documentos como imagem para relatórios.                    ║
    /// ║                                                                              ║
    /// ║  LICENÇAS NECESSÁRIAS:                                                       ║
    /// ║  - Syncfusion (configurada em Program.cs).                                  ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>
    public static class SfdtHelper
    {
        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: SalvarImagemDeDocx (Conversão DOCX → PNG)
        /// │──────────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Converte arquivo DOCX (byte[]) em imagem PNG (byte[]).
        /// │    Renderiza APENAS a PRIMEIRA PÁGINA do documento.
        /// │
        /// │ PARÂMETROS:
        /// │    - docxBytes: Array de bytes do arquivo DOCX (recebido de upload/DB).
        /// │
        /// │ RETORNO:
        /// │    - byte[]: Array de bytes da imagem PNG (primeira página do DOCX).
        /// │
        /// │ PIPELINE DE CONVERSÃO (5 ETAPAS):
        /// │    1️⃣ DOCX → WordDocument (Syncfusion.DocIO)
        /// │    2️⃣ WordDocument → PDF (DocIORenderer.ConvertToPDF)
        /// │    3️⃣ PDF → PdfLoadedDocument (PdfLoadedDocument)
        /// │    4️⃣ PDF → Bitmap (PdfRenderer.ExportAsImage primeira página)
        /// │    5️⃣ Bitmap → PNG bytes (SkiaSharp SKImage.Encode)
        /// │
        /// │ TECNOLOGIAS ENVOLVIDAS:
        /// │    - Syncfusion.DocIO: Parsing de DOCX.
        /// │    - Syncfusion.DocIORenderer: Conversão DOCX → PDF.
        /// │    - Syncfusion.EJ2.PdfViewer: Renderização PDF → Bitmap (PdfRenderer).
        /// │    - SkiaSharp: Codificação de imagem (PNG, qualidade 100).
        /// │
        /// │ QUALIDADE DA IMAGEM:
        /// │    - Formato: PNG (sem perda de qualidade).
        /// │    - Qualidade: 100 (máxima).
        /// │    - Resolução: Padrão do PdfRenderer (96 DPI).
        /// │
        /// │ MEMORY MANAGEMENT:
        /// │    - Usa 'using' em TODOS os objetos IDisposable:
        /// │      * MemoryStream (docStream, pdfStream, input)
        /// │      * WordDocument (document)
        /// │      * DocIORenderer (renderer)
        /// │      * PdfDocument (pdfDoc)
        /// │      * PdfLoadedDocument (loadedPdf)
        /// │      * SKBitmap (bitmap)
        /// │      * SKImage (img)
        /// │      * SKData (encoded)
        /// │    - Garante liberação de memória (importante para arquivos grandes).
        /// │
        /// │ LIMITAÇÕES:
        /// │    - Renderiza SOMENTE primeira página (índice 0).
        /// │    - Para múltiplas páginas, chamar ExportAsImage(i) em loop.
        /// │
        /// │ APLICAÇÃO NO SISTEMA:
        /// │    - Preview de documentos carregados em modal.
        /// │    - Thumbnail de documentos em galeria/dashboard.
        /// │    - Exportação de documentos como imagem para relatórios PDF.
        /// │
        /// │ TRATAMENTO DE ERRO:
        /// │    - NÃO tem try-catch (exceções propagam para Controller).
        /// │    - Controller deve envolver em try-catch com Alerta.TratamentoErroComLinha().
        /// │
        /// │ EXEMPLO DE USO:
        /// │    byte[] docxBytes = await file.OpenReadStream().ReadAsByteArrayAsync();
        /// │    byte[] pngBytes = SfdtHelper.SalvarImagemDeDocx(docxBytes);
        /// │    return File(pngBytes, "image/png", "preview.png");
        /// │──────────────────────────────────────────────────────────────────────────
        /// </summary>
        public static byte[] SalvarImagemDeDocx(byte[] docxBytes)
        {
            // [ETAPA 1] - Carrega DOCX de byte[] para WordDocument (Syncfusion.DocIO)
            using var docStream = new MemoryStream(docxBytes);
            using var document = new WordDocument(docStream, FormatType.Docx);

            // [ETAPA 2] - Converte WordDocument para PDF (DocIORenderer)
            using var renderer = new DocIORenderer();
            using var pdfDoc = renderer.ConvertToPDF(document);

            // [ETAPA 2.1] - Salva PDF em MemoryStream (necessário para PdfRenderer)
            using var pdfStream = new MemoryStream();
            pdfDoc.Save(pdfStream);
            byte[] pdfBytes = pdfStream.ToArray();

            // [ETAPA 3] - Carrega PDF em PdfLoadedDocument e PdfRenderer
            using var input = new MemoryStream(pdfBytes);
            using var loadedPdf = new PdfLoadedDocument(input);
            var pdfRenderer = new PdfRenderer();
            pdfRenderer.Load(input);

            // [ETAPA 4] - Renderiza primeira página (índice 0) do PDF para Bitmap
            using var bitmap = pdfRenderer.ExportAsImage(0);

            // [ETAPA 5] - Converte Bitmap para PNG usando SkiaSharp (qualidade 100)
            using var img = SKImage.FromBitmap(bitmap);
            using var encoded = img.Encode(SKEncodedImageFormat.Png, 100);

            // [RETORNO] - Array de bytes da imagem PNG
            return encoded.ToArray();
        }
    }
}

