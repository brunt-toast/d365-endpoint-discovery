# Dynamics 365 Service Endpoint Discovery Tool

A CLI tool to automate discovery of Dynamics 365 service endpoints. 

## ⚠️ Warning ⚠️

**Excessive use of this tool can result in HTTP 429 responses.** Consider using the filtering options to reduce redundant requests. 

## ⬇️ Installation 

Install as a .NET tool: 
```bash
dotnet tool install -g dynsvcdiscovery
```

Compile from source: 
```bash
make install
```

## 📐 Setup

You'll need an Azure application which can communicate with your Dynamics 365 instance. If you don't have one, here's how you can set one up: 

1. Create an [Azure Application](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) and configure it to communicate with your Dynamics environment. 
2. Under **Overview**, copy the Application (client) ID. Save this for later.
3. Staying under **Overview**, copy the Directory (tenant) ID. Save this for later. 
4. Under **Manage > Certificates &amp; secrets**, add a new client secret. Save this for later. 
5. Under **Manage > API Permissions**, add the permission Ax.FullAccess (you can search for it using its service principal ID, f92c3f85-4759-4901-810d-5da8943dea39). Grant admin consent for your organisation. 

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
