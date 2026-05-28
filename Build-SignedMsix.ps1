$cn = "DynSvcDiscovery"
$password = "password"
$project = "./src/BlazorHybrid/BlazorHybrid.csproj"
$version = (dotnet msbuild $project -nologo -getproperty:Version | Where-Object { $_ -and $_.Trim() } | Select-Object -Last 1).Trim()

if (-not $version) {
    Write-Host "Failed to determine project version." -ForegroundColor Red
    return $false
}

$displayVersion = ($version -split '[\-\+]')[0]

$cert = New-SelfSignedCertificate `
  -Type CodeSigningCert `
  -Subject "CN=${cn}" `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -KeyExportPolicy Exportable `
  -KeySpec Signature `
  -KeyLength 2048 `
  -HashAlgorithm SHA256 `
  -TextExtension @("2.5.29.19={text}false")

if (-not (Test-Path "C:\Cert")) {
    New-Item -Path "C:\Cert" -ItemType Directory | Out-Null
}

$pfxPath = "C:\Cert\${cn}.pfx"
$securePassword = ConvertTo-SecureString -String "${password}" -Force -AsPlainText

if (Get-Command -Name Export-PfxCertificate -ErrorAction SilentlyContinue) {
    Export-PfxCertificate `
      -Cert $cert `
      -FilePath $pfxPath `
      -Password $securePassword | Out-Null
}
else {
    # Fall back to .NET export for environments where PKI cmdlets are unavailable.
    $bytes = $cert.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Pfx, $password)
    [System.IO.File]::WriteAllBytes($pfxPath, $bytes)
}

if (-not (Test-Path $pfxPath)) {
    Write-Host "Failed to export the PFX certificate." -ForegroundColor Red
    return $false
}

$tp = $cert.Thumbprint

dotnet publish "${project}" `
	-c Release `
	-f net10.0-windows10.0.19041.0 `
    /p:WindowsPackageType="MSIX" `
    /p:EnableMsixTooling="true" `
    /p:GenerateAppxPackageOnBuild="true" `
    /p:AppxPackageSigningEnabled="true" `
  /p:ApplicationDisplayVersion="$displayVersion" `
  /p:PackageCertificateKeyFile="$pfxPath" `
	/p:PackageCertificatePassword="${password}" `
	/p:PackageCertificateThumbprint="$tp"

Get-ChildItem Cert:\CurrentUser\My | Where-Object { $_.Thumbprint -like "$tp" } | Remove-Item

