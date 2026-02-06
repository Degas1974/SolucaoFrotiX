/**
 * Script para gerar PDF do Checklist de A/C
 * Combina todas as páginas HTML em um único PDF
 */

const puppeteer = require('puppeteer');
const path = require('path');
const fs = require('fs');

async function gerarPDF() {
    console.log('Iniciando geração do PDF do Checklist A/C...');

    const browser = await puppeteer.launch({
        headless: true,
        args: ['--no-sandbox', '--disable-setuid-sandbox']
    });

    try {
        const page = await browser.newPage();

        // Diretório base
        const baseDir = __dirname;
        const cssPath = path.join(baseDir, 'styles.css');

        // Ler o CSS
        const cssContent = fs.readFileSync(cssPath, 'utf8');

        // Ler todas as páginas HTML e extrair o conteúdo (4 páginas)
        const paginas = [];
        for (let i = 1; i <= 4; i++) {
            const htmlPath = path.join(baseDir, `pagina${i}.html`);
            const htmlContent = fs.readFileSync(htmlPath, 'utf8');

            // Extrair apenas o conteúdo do body (a div.page)
            const bodyMatch = htmlContent.match(/<body[^>]*>([\s\S]*)<\/body>/i);
            if (bodyMatch) {
                paginas.push(bodyMatch[1]);
            }
        }

        // Criar HTML combinado
        const htmlCombinado = `
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Checklist de Diagnóstico Completo - A/C Automotivo</title>
    <style>
        ${cssContent}

        /* Ajustes para impressão */
        @page {
            size: A4;
            margin: 0;
        }

        body {
            margin: 0;
            padding: 0;
        }

        .page {
            width: 210mm;
            min-height: 297mm;
            padding: 10mm 12mm;
            margin: 0;
            box-sizing: border-box;
            page-break-after: always;
        }

        .page:last-child {
            page-break-after: auto;
        }
    </style>
</head>
<body>
${paginas.join('\n')}
</body>
</html>`;

        // Salvar HTML combinado para debug (opcional)
        const htmlCombinadoPath = path.join(baseDir, 'checklist-combinado.html');
        fs.writeFileSync(htmlCombinadoPath, htmlCombinado);
        console.log(`HTML combinado salvo em: ${htmlCombinadoPath}`);

        // Carregar o HTML na página
        await page.setContent(htmlCombinado, {
            waitUntil: 'networkidle0',
            timeout: 30000
        });

        // Gerar o PDF
        const pdfPath = path.join(baseDir, 'Checklist_AC_Xterra_2005_v4.pdf');
        await page.pdf({
            path: pdfPath,
            format: 'A4',
            printBackground: true,
            margin: {
                top: '0mm',
                right: '0mm',
                bottom: '0mm',
                left: '0mm'
            },
            preferCSSPageSize: true
        });

        console.log(`\n✅ PDF gerado com sucesso!`);
        console.log(`📄 Arquivo: ${pdfPath}`);

    } catch (error) {
        console.error('❌ Erro ao gerar PDF:', error);
        throw error;
    } finally {
        await browser.close();
    }
}

// Executar
gerarPDF().catch(console.error);
