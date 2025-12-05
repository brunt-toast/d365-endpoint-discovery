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

## ⌨️ Usage 

Using only required parameters will map all discoverable service endpoints.

```bash
dynsvcdiscovery \
    -c '00000000-0000-0000-0000-000000000000' \
    -s '1ab2C~...' \
    -r 'https://org00000000.operations.dynamics.com' \
    -t 'https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000/oauth2/token' \
 ```

 Additional options and sub-command can be found using `dynsvcdiscovery -?`.

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