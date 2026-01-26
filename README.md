# Dynamics 365 Service Endpoint Discovery Tool

[![Git](https://img.shields.io/badge/Git-F05032?logo=git&logoColor=fff)](#)
[![GitHub](https://img.shields.io/badge/GitHub-%23121011.svg?logo=github&logoColor=white)](#)
[![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?logo=github-actions&logoColor=white)](#)
[![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)](#)
[![Blazor](https://img.shields.io/badge/Blazor-512BD4?logo=blazor&logoColor=fff)](#)
[![Windows](https://custom-icon-badges.demolab.com/badge/Windows-0078D6?logo=windows11&logoColor=white)](#)
[![macOS](https://img.shields.io/badge/macOS-000000?logo=apple&logoColor=F0F0F0)](#)
[![Linux](https://img.shields.io/badge/Linux-FCC624?logo=linux&logoColor=black)](#)
[![NuGet](https://img.shields.io/badge/NuGet-004880?logo=nuget&logoColor=fff)](#)
[![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#)
[![JSON](https://img.shields.io/badge/JSON-000?logo=json&logoColor=fff)](#)
[![YAML](https://img.shields.io/badge/YAML-CB171E?logo=yaml&logoColor=fff)](#)

A CLI+GUI tool to automate discovery of Dynamics 365 service endpoints.

## ⚠️ Warning ⚠️

**Excessive use of this tool can result in HTTP 429 responses.** Consider using the filtering options to reduce redundant requests. 

## ⬇️⌨️ Installation (CLI)

Install as a .NET tool: 
```bash
dotnet tool install -g dynsvcdiscovery
```

Compile from source: 
```bash
make install
```

## ⬇️🖼️ Installation (GUI)

Download the [latest release](https://github.com/brunt-toast/d365-endpoint-discovery/releases/latest) (MSIX bundled).

Run Install.ps1, or trust the certificate and run the MSIX file manually.

## 📐 Setup

You'll need an Azure application which can communicate with your Dynamics 365 instance. If you don't have one, here's how you can set one up: 

1. Create an [Azure Application](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) and configure it to communicate with your Dynamics environment. 
2. Under **Overview**, copy the Application (client) ID. Save this for later.
3. Staying under **Overview**, copy the Directory (tenant) ID. Save this for later. 
4. Under **Manage > Certificates &amp; secrets**, add a new client secret. Save this for later. 
5. Under **Manage > API Permissions**, add the permission Ax.FullAccess (you can search for it using its service principal ID, f92c3f85-4759-4901-810d-5da8943dea39). Grant admin consent for your organisation. 
6. In Dynamics, navigate to **System Administration > Setup > Entra ID Applications**, and add a new record with the Application (client) ID, assigned to an appropriate user. Remember to click "Save"!

## ⌨️ Usage 

Using only required parameters will map all discoverable service endpoints.

```bash
dynsvcdiscovery \
    -c '<your-client-id>' \
    -s '<your-client-secret>' \
    -r 'https://<your-org-id>.operations.dynamics.com' \
    -t 'https://login.microsoftonline.com/<your-tenant-id>/oauth2/token' 
 ```

 Additional options and sub-commands can be found using `dynsvcdiscovery -?`.

## 🐛 Debug 

Failed HTTP requests are not treated as errors. Instead, their relevent group/service/operation is left unpopulated. 

To diagnose errors, set the log level to Warning and redirect standard error to a different destination. 

```bash
dynsvcdiscovery \
    -csrt ... \
    --log-level Warning \
    >services.json \
    2>dynsvcdiscovery.log
```

For more verbose results, set the log level to Trace. Note that there will be no way to separate the final output from logging messages below the Warning level. 

## ✏️ Contributors' Notice
The use of AI-generated code is strictly prohibited in this repository. See [CONTRIBUTING.md](./CONTRIBUTING.md).

## 🪪 License

This code is released under the MIT license. See [LICENSE.md](./LICENSE.md).

## 🔒 Security

Please report any security concerns to the maintainer of this repository via email. See [SECURITY.md](./SECURITY.md).
