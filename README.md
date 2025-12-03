# Dynamics 365 Service Endpoint Discovery Tool

A CLI tool to automate discovery of Dynamics 365 service endpoints. 

## Installation 

Compile from source: 
```bash
make install
```

## Usage 

Using only required parameters will map all discoverable service endpoints.

```bash
dynsvcdiscovery \
    -c '00000000-0000-0000-0000-000000000000' \
    -s '1ab2C~...' \
    -r 'https://org00000000.operations.dynamics.com' \
    -t 'https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000/oauth2/token' \
 ```

 Additional options and sub-command can be found using `dynsvcdiscovery -?`.