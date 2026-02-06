/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  PDF EXPORT PROFISSIONAL - FrotiX                                        ║
 * ║                                                                          ║
 * ║  Biblioteca compartilhada para exportação de gráficos em PDF com         ║
 * ║  visual profissional, usando html2canvas + jsPDF + fonte Outfit          ║
 * ║                                                                          ║
 * ║  Última atualização: 15/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// =====================================================================
// CONFIGURAÇÕES DE CORES POR TEMA
// =====================================================================
const PDF_TEMAS = {
    // Tema Abastecimento (Caramelo)
    abastecimento: {
        primary: [168, 120, 76],      // #a8784c
        secondary: [196, 149, 106],   // #c4956a
        accent: [212, 165, 116],      // #d4a574
        dark: [139, 94, 60],          // #8b5e3c
        darker: [109, 71, 44],        // #6d472c
        cream: [245, 235, 224],       // #f5ebe0
        white: [255, 255, 255],
        gray: [100, 100, 100],
        lightGray: [240, 240, 240]
    },
    // Tema Economildo (Terracota)
    economildo: {
        primary: [180, 90, 60],       // #b45a3c
        secondary: [201, 109, 78],    // #c96d4e
        accent: [232, 168, 124],      // #e8a87c
        dark: [139, 69, 49],          // #8b4531
        darker: [100, 50, 35],        // #643223
        cream: [250, 246, 244],       // #faf6f4
        white: [255, 255, 255],
        gray: [100, 100, 100],
        lightGray: [240, 240, 240]
    }
};

// =====================================================================
// ÍCONES SVG FONTAWESOME DUOTONE (viewBox 0 0 640 640)
// =====================================================================
const PDF_ICONS_DUOTONE = {
    gasPump: {
        secondary: 'M96 128L96 529.4C98.5 528.5 101.2 528 104 528L376 528C378.8 528 381.5 528.5 384 529.4L384 128C384 92.7 355.3 64 320 64L160 64C124.7 64 96 92.7 96 128zM160 144C160 135.2 167.2 128 176 128L304 128C312.8 128 320 135.2 320 144L320 240C320 248.8 312.8 256 304 256L176 256C167.2 256 160 248.8 160 240L160 144z',
        primary: 'M480 160L448 128C439.2 119.2 439.2 104.8 448 96C456.8 87.2 471.2 87.2 480 96L557.3 173.3C569.3 185.3 576 201.6 576 218.6L576 440C576 479.8 543.8 512 504 512C464.2 512 432 479.8 432 440L432 408C432 385.9 414.1 368 392 368L384 368L384 320L392 320C440.6 320 480 359.4 480 408L480 440C480 453.3 490.7 464 504 464C517.3 464 528 453.3 528 440L528 286C500.4 278.9 480 253.8 480 224L480 160zM104 528L376 528C389.3 528 400 538.7 400 552C400 565.3 389.3 576 376 576L104 576C90.7 576 80 565.3 80 552C80 538.7 90.7 528 104 528zM176 128L304 128C312.8 128 320 135.2 320 144L320 240C320 248.8 312.8 256 304 256L176 256C167.2 256 160 248.8 160 240L160 144C160 135.2 167.2 128 176 128z'
    },
    chartLine: {
        secondary: 'M64 128L64 464C64 508.2 99.8 544 144 544L544 544C561.7 544 576 529.7 576 512C576 494.3 561.7 480 544 480L144 480C135.2 480 128 472.8 128 464L128 128C128 110.3 113.7 96 96 96C78.3 96 64 110.3 64 128z',
        primary: 'M534.6 169.4C547.1 181.9 547.1 202.2 534.6 214.7L406.6 342.7C394.1 355.2 373.8 355.2 361.3 342.7L303.9 285.3L230.5 358.7C218 371.2 197.7 371.2 185.2 358.7C172.7 346.2 172.7 325.9 185.2 313.4L281.2 217.4C293.7 204.9 314 204.9 326.5 217.4L384 274.7L489.4 169.4C501.9 156.9 522.2 156.9 534.7 169.4z'
    },
    droplet: {
        secondary: 'M128 384C128 292.8 258.2 109.9 294.6 60.5C300.5 52.5 309.8 48 319.8 48L320.2 48C330.2 48 339.5 52.5 345.4 60.5C381.8 109.9 512 292.8 512 384C512 490 426 576 320 576C214 576 128 490 128 384zM192 376C192 451.1 252.9 512 328 512C341.3 512 352 501.3 352 488C352 474.7 341.3 464 328 464C279.4 464 240 424.6 240 376C240 362.7 229.3 352 216 352C202.7 352 192 362.7 192 376z',
        primary: 'M216 352C229.3 352 240 362.7 240 376C240 424.6 279.4 464 328 464C341.3 464 352 474.7 352 488C352 501.3 341.3 512 328 512C252.9 512 192 451.1 192 376C192 362.7 202.7 352 216 352z'
    },
    gaugeHigh: {
        secondary: 'M64 320C64 461.4 178.6 576 320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320zM192 320C192 337.7 177.7 352 160 352C142.3 352 128 337.7 128 320C128 302.3 142.3 288 160 288C177.7 288 192 302.3 192 320zM240 208C240 225.7 225.7 240 208 240C190.3 240 176 225.7 176 208C176 190.3 190.3 176 208 176C225.7 176 240 190.3 240 208zM256 416C256 380.7 284.7 352 320 352C321.7 352 323.4 352.1 325.1 352.2L394.6 213.3C400.5 201.4 414.9 196.6 426.8 202.6C438.7 208.6 443.5 222.9 437.5 234.8L368 373.7C378 385 384 399.8 384 416C384 451.3 355.3 480 320 480C284.7 480 256 451.3 256 416zM352 160C352 177.7 337.7 192 320 192C302.3 192 288 177.7 288 160C288 142.3 302.3 128 320 128C337.7 128 352 142.3 352 160zM512 320C512 337.7 497.7 352 480 352C462.3 352 448 337.7 448 320C448 302.3 462.3 288 480 288C497.7 288 512 302.3 512 320z',
        primary: 'M394.5 213.3C400.4 201.4 414.8 196.6 426.7 202.6C438.6 208.6 443.4 222.9 437.4 234.8L368 373.7C378 385 384 399.8 384 416C384 451.3 355.3 480 320 480C284.7 480 256 451.3 256 416C256 380.7 284.7 352 320 352C321.7 352 323.4 352.1 325.1 352.2L394.6 213.3z'
    },
    moneyBillTrendUp: {
        secondary: 'M64 368L64 528C64 554.5 85.5 576 112 576L528 576C554.5 576 576 554.5 576 528L576 368C576 341.5 554.5 320 528 320L112 320C85.5 320 64 341.5 64 368zM112 368L160 368C160 394.5 138.5 416 112 416L112 368zM112 480C138.5 480 160 501.5 160 528L112 528L112 480zM384 448C384 483.3 355.3 512 320 512C284.7 512 256 483.3 256 448C256 412.7 284.7 384 320 384C355.3 384 384 412.7 384 448zM480 368L528 368L528 416C501.5 416 480 394.5 480 368zM480 528C480 501.5 501.5 480 528 480L528 528L480 528z',
        primary: 'M520 48C533.3 48 544 58.7 544 72L544 173.8C544 187.1 533.3 197.8 520 197.8C506.7 197.8 496 187.1 496 173.8L496 129.9L369 256.9C360.1 265.8 345.9 266.3 336.4 258.2L240 175.6L143.6 258.2C133.5 266.8 118.4 265.7 109.8 255.6C101.2 245.5 102.3 230.4 112.4 221.8L224.4 125.8C233.4 118.1 246.6 118.1 255.6 125.8L350.7 207.3L462 96L418.1 96C404.8 96 394.1 85.3 394.1 72C394.1 58.7 404.8 48 418.1 48L520 48z'
    },
    truck: {
        secondary: 'M96 488C96 536.6 135.3 576 184 576.1C232.6 576.1 272 536.7 272 488.1C272 439.5 232.7 400.1 184.1 400.1C135.4 400 96 439.4 96 488zM224 488C224 510.1 206.1 528 184 528C161.9 528 144 510.1 144 488C144 465.9 161.9 448 184 448C206.1 448 224 465.9 224 488zM368 488C368 536.6 407.3 576 456 576.1C504.6 576.1 544 536.7 544 488.1C544 439.5 504.7 400.1 456.1 400.1C407.4 400 368 439.4 368 488zM496 488C496 510.1 478.1 528 456 528C433.9 528 416 510.1 416 488C416 465.9 433.9 448 456 448C478.1 448 496 465.9 496 488z',
        primary: 'M32 160C32 124.7 60.7 96 96 96L384 96C419.3 96 448 124.7 448 160L448 192L498.7 192C515.7 192 532 198.7 544 210.7L589.3 256C601.3 268 608 284.3 608 301.3L608 448C608 483.3 579.3 512 544 512L540.7 512C542.9 504.4 544 496.3 544 488C544 439.4 504.6 400 456 400C407.4 400 368 439.4 368 488C368 496.3 369.2 504.4 371.3 512L268.7 512C270.9 504.4 272 496.3 272 488C272 439.4 232.6 400 184 400C135.4 400 96 439.4 96 488C96 496.3 97.2 504.4 99.3 512L96 512C60.7 512 32 483.3 32 448L32 160zM544 352L544 301.3L498.7 256L448 256L448 352L544 352z'
    },
    fireFlame: {
        secondary: 'M128 385.6C128 490.7 214.8 576 320 576C425.2 576 512 490.7 512 385.6C512 363.4 508.1 341.4 500.5 320.5L499.8 318.6C465.8 224.8 410 140.5 337.1 72.5L333.8 69.5C330.1 66 325.1 64 320 64C314.9 64 309.9 66 306.2 69.5L302.9 72.5C230 140.5 174.2 224.8 140.2 318.6L139.5 320.5C131.9 341.3 128 363.4 128 385.6zM208 375.9C208 354.7 215.8 334.3 229.8 318.5L236.7 310.7C238.8 308.3 241.8 307 245 307C251.1 307 256 311.9 256 318L256 362C256 386.3 275.8 406 300.1 406C324.3 406 344 386.4 344 362.2C344 350.6 339.4 339.4 331.2 331.2L318 318C304 304 296.1 284.9 296.1 265C296.1 248.8 301.4 233 311.1 220.1L316.9 212.3C318.9 209.6 322.1 208 325.4 208C331.3 208 336.1 212.8 336.1 218.7L336.1 230.1C336.1 239 339.7 247.5 346 253.7L397.5 304.4C419.6 326.2 432.1 355.9 432.1 386.9C432.1 447.1 383.3 495.9 323.1 495.9L320.1 495.9C258.2 495.9 208.1 445.8 208.1 383.9L208.1 375.7z',
        primary: 'M311 220L316.8 212.2C318.8 209.5 322 207.9 325.3 207.9C331.2 207.9 336 212.7 336 218.6L336 230C336 238.9 339.6 247.4 345.9 253.6L397.4 304.3C419.5 326.1 432 355.8 432 386.8C432 447 383.2 495.8 323 495.8L320 495.8C258.1 495.8 208 445.7 208 383.8L208 375.6C208 354.4 215.8 334 229.8 318.2L236.7 310.4C238.8 308 241.8 306.7 245 306.7C251.1 306.7 256 311.6 256 317.7L256 361.7C256 386 275.8 405.7 300.1 405.7C324.3 405.7 344 386.1 344 361.9C344 350.3 339.4 339.1 331.2 330.9L318 317.7C304 303.7 296.1 284.6 296.1 264.7C296.1 248.5 301.4 232.7 311.1 219.8z'
    },
    filePdf: {
        secondary: 'M64 128C64 92.7 92.7 64 128 64L277.5 64C294.5 64 310.8 70.7 322.8 82.7L429.3 189.3C441.3 201.3 448 217.6 448 234.6L448 400.1L272 400.1C236.7 400.1 208 428.8 208 464.1L208 576.1L128 576.1C92.7 576.1 64 547.4 64 512.1L64 128zM272 122.5L272 216C272 229.3 282.7 240 296 240L389.5 240L272 122.5z',
        primary: 'M252 464C252 453 261 444 272 444L304 444C337.1 444 364 470.9 364 504C364 537.1 337.1 564 304 564L292 564L292 592C292 603 283 612 272 612C261 612 252 603 252 592L252 464zM292 524L304 524C315 524 324 515 324 504C324 493 315 484 304 484L292 484L292 524zM380 464C380 453 389 444 400 444L432 444C460.7 444 484 467.3 484 496L484 560C484 588.7 460.7 612 432 612L400 612C389 612 380 603 380 592L380 464zM420 484L420 572L432 572C438.6 572 444 566.6 444 560L444 496C444 489.4 438.6 484 432 484L420 484zM528 444L576 444C587 444 596 453 596 464C596 475 587 484 576 484L548 484L548 508L576 508C587 508 596 517 596 528C596 539 587 548 576 548L548 548L548 592C548 603 539 612 528 612C517 612 508 603 508 592L508 464C508 453 517 444 528 444z'
    },
    car: {
        secondary: 'M135 224C157 178.7 203 149 253.3 149L386.7 149C437 149 483 178.7 505 224L512 240L128 240L135 224zM96 408C96 430.1 113.9 448 136 448L168 448C190.1 448 208 430.1 208 408L208 392L432 392L432 408C432 430.1 449.9 448 472 448L504 448C526.1 448 544 430.1 544 408L544 392L544 320C544 284.7 515.3 256 480 256L160 256C124.7 256 96 284.7 96 320L96 392L96 408z',
        primary: 'M160 320C160 302.3 174.3 288 192 288L256 288C273.7 288 288 302.3 288 320C288 337.7 273.7 352 256 352L192 352C174.3 352 160 337.7 160 320zM352 320C352 302.3 366.3 288 384 288L448 288C465.7 288 480 302.3 480 320C480 337.7 465.7 352 448 352L384 352C366.3 352 352 337.7 352 320z'
    }
};

// =====================================================================
// FUNÇÃO PARA RENDERIZAR ÍCONE DUOTONE
// =====================================================================
async function renderizarIconeDuotonePdf(icon, corPrimaria, corSecundaria) {
    return new Promise((resolve) => {
        try {
            const canvas = document.createElement('canvas');
            const scale = 4;
            canvas.width = 640 * scale / 10;
            canvas.height = 640 * scale / 10;
            const ctx = canvas.getContext('2d');
            ctx.scale(scale / 10, scale / 10);

            // Desenhar camada secundária (com opacidade)
            ctx.globalAlpha = 0.4;
            ctx.fillStyle = `rgb(${corSecundaria[0]}, ${corSecundaria[1]}, ${corSecundaria[2]})`;
            const pathSecondary = new Path2D(icon.secondary);
            ctx.fill(pathSecondary);

            // Desenhar camada primária
            ctx.globalAlpha = 1.0;
            ctx.fillStyle = `rgb(${corPrimaria[0]}, ${corPrimaria[1]}, ${corPrimaria[2]})`;
            const pathPrimary = new Path2D(icon.primary);
            ctx.fill(pathPrimary);

            resolve(canvas.toDataURL('image/png'));
        } catch (e) {
            console.warn('Erro ao renderizar ícone:', e);
            resolve(null);
        }
    });
}

// =====================================================================
// FUNÇÃO PARA RENDERIZAR TEXTO COM FONTE OUTFIT
// =====================================================================
async function renderizarTextoOutfitPdf(texto, tamanhoFonte, cor, negrito = true) {
    return new Promise((resolve) => {
        const tempCanvas = document.createElement('canvas');
        const ctx = tempCanvas.getContext('2d');
        const scale = 4;

        const fontWeight = negrito ? '800' : '600';
        ctx.font = `${fontWeight} ${tamanhoFonte * scale}px 'Outfit', sans-serif`;

        const metrics = ctx.measureText(texto);
        const width = metrics.width + 20;
        const height = tamanhoFonte * scale * 1.5;

        tempCanvas.width = width;
        tempCanvas.height = height;

        ctx.font = `${fontWeight} ${tamanhoFonte * scale}px 'Outfit', sans-serif`;
        ctx.fillStyle = `rgb(${cor[0]}, ${cor[1]}, ${cor[2]})`;
        ctx.textBaseline = 'middle';
        ctx.fillText(texto, 0, height / 2);

        resolve({
            dataUrl: tempCanvas.toDataURL('image/png'),
            width: width / scale,
            height: height / scale
        });
    });
}

// =====================================================================
// FUNÇÃO PRINCIPAL DE EXPORTAÇÃO PDF PROFISSIONAL
// =====================================================================
async function exportarGraficoPdfProfissional(elementId, titulo, tema = 'abastecimento', tituloDashboard = 'Dashboard de Abastecimentos', iconePrincipal = 'gasPump', forcarOrientacao = null) {
    try {
        if (typeof AppToast !== 'undefined') {
            AppToast.show('orange', 'Gerando PDF profissional...', 5000);
        }

        const cores = PDF_TEMAS[tema] || PDF_TEMAS.abastecimento;

        const elemento = document.getElementById(elementId);
        if (!elemento) {
            throw new Error('Elemento não encontrado: ' + elementId);
        }

        // Para tabelas, capturar o container pai
        let elementoCaptura = elemento;
        if (elementId.includes('tabela') || elementId.includes('Tabela')) {
            elementoCaptura = elemento.closest('.ftx-grid-tabela') || elemento.closest('.eco-card') || elemento;
        }

        // Capturar com html2canvas
        const canvas = await html2canvas(elementoCaptura, {
            scale: 2.5,
            useCORS: true,
            allowTaint: true,
            backgroundColor: '#ffffff',
            logging: false
        });

        // Pré-renderizar ícones
        const iconeBase = PDF_ICONS_DUOTONE[iconePrincipal] || PDF_ICONS_DUOTONE.gasPump;
        const iconPrincipalImg = await renderizarIconeDuotonePdf(iconeBase, [255, 255, 255], [220, 200, 180]);
        const iconDroplet = await renderizarIconeDuotonePdf(PDF_ICONS_DUOTONE.droplet, cores.dark, cores.secondary);
        const iconGauge = await renderizarIconeDuotonePdf(PDF_ICONS_DUOTONE.gaugeHigh, cores.dark, cores.secondary);
        const iconMoney = await renderizarIconeDuotonePdf(PDF_ICONS_DUOTONE.moneyBillTrendUp, cores.dark, cores.secondary);
        const iconTruck = await renderizarIconeDuotonePdf(PDF_ICONS_DUOTONE.truck, cores.dark, cores.secondary);
        const iconFire = await renderizarIconeDuotonePdf(PDF_ICONS_DUOTONE.fireFlame, cores.dark, cores.secondary);
        const iconPdf = await renderizarIconeDuotonePdf(PDF_ICONS_DUOTONE.filePdf, cores.darker, cores.secondary);

        // Renderizar textos com fonte Outfit
        const tituloImg = await renderizarTextoOutfitPdf(tituloDashboard, 24, [255, 255, 255], true);
        const subtituloImg = await renderizarTextoOutfitPdf(titulo, 13, [245, 235, 224], true);

        // Criar PDF
        const { jsPDF } = window.jspdf;
        const imgWidth = canvas.width;
        const imgHeight = canvas.height;

        // Determinar orientação: forçada ou automática
        let orientacao;
        if (forcarOrientacao === 'portrait' || forcarOrientacao === 'landscape') {
            orientacao = forcarOrientacao;
        } else {
            // Automático: landscape se imagem for muito larga
            orientacao = (imgWidth > imgHeight * 1.2) ? 'landscape' : 'portrait';
        }

        const pdf = new jsPDF({
            orientation: orientacao,
            unit: 'mm',
            format: 'a4'
        });

        const pageWidth = pdf.internal.pageSize.getWidth();
        const pageHeight = pdf.internal.pageSize.getHeight();
        const margin = 10;

        // ============ FUNDO DECORATIVO ============
        pdf.setFillColor(...cores.cream);
        pdf.rect(0, 0, pageWidth, pageHeight, 'F');

        pdf.setFillColor(cores.accent[0], cores.accent[1], cores.accent[2]);
        pdf.setGState(new pdf.GState({ opacity: 0.05 }));
        pdf.circle(pageWidth - 15, 25, 40, 'F');
        pdf.circle(20, pageHeight - 20, 30, 'F');
        pdf.circle(pageWidth / 2, pageHeight, 60, 'F');
        pdf.setGState(new pdf.GState({ opacity: 1 }));

        // ============ HEADER ============
        const headerHeight = 42;
        pdf.setFillColor(...cores.primary);
        pdf.roundedRect(margin, margin, pageWidth - (margin * 2), headerHeight, 5, 5, 'F');

        // Brilho
        pdf.setFillColor(255, 255, 255);
        pdf.setGState(new pdf.GState({ opacity: 0.15 }));
        pdf.roundedRect(margin + 5, margin + 2, pageWidth - (margin * 2) - 10, 3, 1, 1, 'F');
        pdf.setGState(new pdf.GState({ opacity: 1 }));

        // Ícone principal
        if (iconPrincipalImg) {
            pdf.addImage(iconPrincipalImg, 'PNG', margin + 6, margin + 9, 18, 18);
        }

        // Título com fonte Outfit
        if (tituloImg.dataUrl) {
            pdf.addImage(tituloImg.dataUrl, 'PNG', margin + 27, margin + 5, tituloImg.width * 0.35, tituloImg.height * 0.35);
        }
        if (subtituloImg.dataUrl) {
            pdf.addImage(subtituloImg.dataUrl, 'PNG', margin + 27, margin + 22, subtituloImg.width * 0.35, subtituloImg.height * 0.35);
        }

        // Badge da data
        const dataGeracao = new Date();
        const dataFormatada = dataGeracao.toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' });
        const horaFormatada = dataGeracao.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });

        const badgeWidth = 52;
        const badgeHeight = 32;
        const badgeX = pageWidth - margin - badgeWidth - 3;
        const badgeY = margin + 5;
        pdf.setFillColor(0, 0, 0);
        pdf.setGState(new pdf.GState({ opacity: 0.35 }));
        pdf.roundedRect(badgeX, badgeY, badgeWidth, badgeHeight, 4, 4, 'F');
        pdf.setGState(new pdf.GState({ opacity: 1 }));

        pdf.setTextColor(...cores.cream);
        pdf.setFontSize(7);
        pdf.setFont('helvetica', 'bold');
        pdf.text('GERADO EM', badgeX + badgeWidth/2, badgeY + 8, { align: 'center' });
        pdf.setTextColor(...cores.white);
        pdf.setFontSize(11);
        pdf.text(dataFormatada, badgeX + badgeWidth/2, badgeY + 18, { align: 'center' });
        pdf.setFontSize(10);
        pdf.text(horaFormatada, badgeX + badgeWidth/2, badgeY + 27, { align: 'center' });

        // ============ BARRA DE ÍCONES ============
        const infoBarY = margin + headerHeight + 4;
        const infoBarHeight = 12;
        pdf.setFillColor(...cores.lightGray);
        pdf.roundedRect(margin, infoBarY, pageWidth - (margin * 2), infoBarHeight, 3, 3, 'F');

        const iconsBar = [iconDroplet, iconGauge, iconMoney, iconTruck, iconFire];
        const iconSize = 7.5;
        const iconSpacing = (pageWidth - (margin * 2) - 20) / (iconsBar.length + 1);
        iconsBar.forEach((icon, index) => {
            if (icon) {
                pdf.addImage(icon, 'PNG', margin + 10 + (iconSpacing * (index + 1)) - (iconSize/2), infoBarY + (infoBarHeight - iconSize)/2, iconSize, iconSize);
            }
        });

        // ============ ÁREA DO GRÁFICO ============
        const chartAreaY = infoBarY + infoBarHeight + 5;
        const chartAreaHeight = pageHeight - chartAreaY - 24;

        pdf.setFillColor(150, 150, 150);
        pdf.setGState(new pdf.GState({ opacity: 0.15 }));
        pdf.roundedRect(margin + 3, chartAreaY + 3, pageWidth - (margin * 2), chartAreaHeight, 6, 6, 'F');
        pdf.setGState(new pdf.GState({ opacity: 1 }));

        pdf.setFillColor(...cores.white);
        pdf.roundedRect(margin, chartAreaY, pageWidth - (margin * 2), chartAreaHeight, 6, 6, 'F');

        pdf.setDrawColor(...cores.primary);
        pdf.setLineWidth(0.5);
        pdf.roundedRect(margin, chartAreaY, pageWidth - (margin * 2), chartAreaHeight, 6, 6, 'S');

        pdf.setFillColor(...cores.primary);
        pdf.roundedRect(margin, chartAreaY, 4, 20, 2, 0, 'F');

        // Imagem do gráfico
        const disponWidth = pageWidth - (margin * 2) - 12;
        const disponHeight = chartAreaHeight - 10;
        let finalWidth = disponWidth;
        let finalHeight = (imgHeight * disponWidth) / imgWidth;
        if (finalHeight > disponHeight) {
            finalHeight = disponHeight;
            finalWidth = (imgWidth * disponHeight) / imgHeight;
        }
        const xOffset = margin + ((pageWidth - (margin * 2) - finalWidth) / 2);
        const yOffset = chartAreaY + ((chartAreaHeight - finalHeight) / 2);
        const imgData = canvas.toDataURL('image/png', 1.0);
        pdf.addImage(imgData, 'PNG', xOffset, yOffset, finalWidth, finalHeight);

        // ============ RODAPÉ ============
        const footerY = pageHeight - 18;
        pdf.setDrawColor(...cores.primary);
        pdf.setLineWidth(1);
        pdf.line(margin, footerY - 4, pageWidth - margin, footerY - 4);
        pdf.setDrawColor(...cores.secondary);
        pdf.setLineWidth(0.3);
        pdf.line(margin + 40, footerY - 2, pageWidth - margin - 40, footerY - 2);

        if (iconPdf) {
            pdf.addImage(iconPdf, 'PNG', margin, footerY + 1, 5, 5);
        }
        pdf.setTextColor(...cores.darker);
        pdf.setFontSize(14);
        pdf.setFont('helvetica', 'bold');
        pdf.text('FrotiX', margin + 7, footerY + 6);
        pdf.setFillColor(...cores.primary);
        pdf.circle(margin + 27, footerY + 4, 1, 'F');
        pdf.setTextColor(...cores.gray);
        pdf.setFontSize(8);
        pdf.setFont('helvetica', 'normal');
        pdf.text('Sistema de Gestão de Frotas', margin + 30, footerY + 5);

        pdf.setFontSize(7);
        const footerRight = pageWidth - margin;
        pdf.text('Câmara dos Deputados / Coordenação de Transportes', footerRight, footerY + 2, { align: 'right' });
        pdf.setTextColor(...cores.primary);
        pdf.setFont('helvetica', 'bold');
        pdf.text('www.frotix.camara.leg.br', footerRight, footerY + 7, { align: 'right' });

        // Marca d'água
        pdf.setTextColor(220, 220, 220);
        pdf.setFontSize(50);
        pdf.setGState(new pdf.GState({ opacity: 0.03 }));
        pdf.text('FrotiX', pageWidth / 2, pageHeight / 2, { align: 'center', angle: 30 });
        pdf.setGState(new pdf.GState({ opacity: 1 }));

        // Salvar
        const nomeArquivo = 'FrotiX_' + titulo.replace(/[^a-zA-Z0-9áéíóúãõâêîôûàèìòùçÁÉÍÓÚÃÕÂÊÎÔÛÀÈÌÒÙÇ ]/g, '').replace(/ /g, '_') + '_' + dataGeracao.toISOString().slice(0,10) + '.pdf';
        pdf.save(nomeArquivo);

        if (typeof AppToast !== 'undefined') {
            AppToast.show('green', 'PDF profissional gerado com sucesso!', 4000);
        }

    } catch (error) {
        console.error('Erro ao exportar PDF:', error);
        if (typeof Alerta !== 'undefined') {
            Alerta.TratamentoErroComLinha('pdf-export-profissional.js', 'exportarGraficoPdfProfissional', error);
        }
    }
}

// Alias simplificado para chamadas básicas
// Parâmetros: elementId, titulo, orientacao (opcional: 'portrait', 'landscape' ou null para auto)
async function exportarGraficoPdf(elementId, titulo, orientacao = null) {
    return exportarGraficoPdfProfissional(elementId, titulo, 'abastecimento', 'Dashboard de Abastecimentos', 'gasPump', orientacao);
}

// Exportar funções globalmente
window.exportarGraficoPdfProfissional = exportarGraficoPdfProfissional;
window.exportarGraficoPdf = exportarGraficoPdf;
window.PDF_TEMAS = PDF_TEMAS;
window.PDF_ICONS_DUOTONE = PDF_ICONS_DUOTONE;
