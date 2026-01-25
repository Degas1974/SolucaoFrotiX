// wwwroot/js/sig.js
window.sig = {
    resizeSignatureCanvasById: function (hostId)
    {
        const host = document.getElementById(hostId);
        if (!host) return;

        const canvas = host.querySelector('canvas');
        if (!canvas) return;

        const ratio = window.devicePixelRatio || 1;
        const rect = host.getBoundingClientRect();

        const w = Math.max(1, Math.floor(rect.width * ratio));
        const h = Math.max(1, Math.floor(rect.height * ratio));

        canvas.width = w;
        canvas.height = h;

        const ctx = canvas.getContext('2d');
        if (ctx) ctx.setTransform(ratio, 0, 0, ratio, 0, 0);
    }
};