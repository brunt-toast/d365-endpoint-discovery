$cn = "DynSvcDiscovery"
$password = "password"
$project = "./src/BlazorHybrid/BlazorHybrid.csproj"

$cert = New-SelfSignedCertificate `
  -Type CodeSigningCert `
  -Subject "CN=${cn}" `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -KeyExportPolicy Exportable `
  -KeySpec Signature `
  -KeyLength 2048 `
  -HashAlgorithm SHA256 `
  -TextExtension @("2.5.29.19={text}false")

Test-Path C:\Cert || mkdir C:\Cert
Export-PfxCertificate `
  -Cert $cert `
  -FilePath "C:\Cert\${cn}.pfx" `
  -Password (ConvertTo-SecureString -String "${password}" -Force -AsPlainText)

if($? -ne $true)
{
    Write-Host "Failed to export the PFX certificate." -ForegroundColor Red
    return $false;
}

$tp = Import-PfxCertificate -FilePath "C:\Cert\${cn}.pfx" `
	-CertStoreLocation Cert:\CurrentUser\My `
	-Password (ConvertTo-SecureString "${password}" -AsPlainText -Force) | Select-Object -ExpandProperty Thumbprint

dotnet publish "${project}" `
	-c Release `
	-f net10.0-windows10.0.19041.0 `
    /p:WindowsPackageType="MSIX" `
    /p:EnableMsixTooling="true" `
    /p:GenerateAppxPackageOnBuild="true" `
    /p:AppxPackageSigningEnabled="true" `
    /p:AppxBundle="never" `
	/p:PackageCertificateKeyFile="C:\Cert\${cn}.pfx" `
	/p:PackageCertificatePassword="${password}" `
	/p:PackageCertificateThumbprint="$tp"

Get-ChildItem Cert:\CurrentUser\My | Where-Object { $_.Thumbprint -like "$tp" } | Remove-Item

