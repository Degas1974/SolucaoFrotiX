$ErrorActionPreference = 'Stop'

$assets = @(
    @{ Url = 'https://kendo.cdn.telerik.com/2025.2.520/js/jszip.min.js'; Destination = 'wwwroot/lib/kendo/2025.2.520/js/jszip.min.js' },
    @{ Url = 'https://kendo.cdn.telerik.com/2025.2.520/js/kendo.all.min.js'; Destination = 'wwwroot/lib/kendo/2025.2.520/js/kendo.all.min.js' },
    @{ Url = 'https://kendo.cdn.telerik.com/2025.2.520/js/kendo.aspnetmvc.min.js'; Destination = 'wwwroot/lib/kendo/2025.2.520/js/kendo.aspnetmvc.min.js' },
    @{ Url = 'https://kendo.cdn.telerik.com/2025.2.520/js/cultures/kendo.culture.pt-BR.min.js'; Destination = 'wwwroot/lib/kendo/2025.2.520/js/cultures/kendo.culture.pt-BR.min.js' },
    @{ Url = 'https://kendo.cdn.telerik.com/2025.2.520/js/messages/kendo.messages.pt-BR.min.js'; Destination = 'wwwroot/lib/kendo/2025.2.520/js/messages/kendo.messages.pt-BR.min.js' },
    @{ Url = 'https://kendo.cdn.telerik.com/themes/12.0.1/bootstrap/bootstrap-main.css'; Destination = 'wwwroot/lib/kendo/themes/12.0.1/bootstrap/bootstrap-main.css' }
)

foreach ($asset in $assets) {
    try {
        $destinationDir = Split-Path -Path $asset.Destination -Parent
        New-Item -ItemType Directory -Force -Path $destinationDir | Out-Null
        Invoke-WebRequest -Uri $asset.Url -OutFile $asset.Destination -UseBasicParsing
        Write-Host "OK: $($asset.Url) -> $($asset.Destination)"
    } catch {
        Write-Error "Falha ao baixar $($asset.Url): $($_.Exception.Message)"
        throw
    }
}
