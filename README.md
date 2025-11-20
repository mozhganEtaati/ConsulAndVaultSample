# Consul + Vault + Winton.NET Sample  
A minimal but enterprise-ready example showing **how to load configuration from Consul KV**, **Vault secrets**, and **auto‑reload settings** using the **Winton.Extensions.Configuration.Consul** package in .NET.

This README explains:

- How the architecture works  
- How to configure KV + Secrets  
- How the .NET app loads configuration   

---

## 🧩 Architecture Overview

```
+------------------------------+
|        .NET Application      |
|------------------------------|
|  Winton Consul Config Loader |
|  Vault Client (HTTP API)     |
|------------------------------|
|  Loads KV (config values)    |
|  Loads Vault Secrets         |
+------------------------------+
          |           |
          |           |
     Consul KV     Vault Secrets
          |               |
    Stores JSON       Stores sensitive data
     config files     (connection strings,
        etc.          API keys, credentials)
```

- **Consul → stores non‑sensitive configuration** (`appsettings.json`, feature flags, etc.)
- **Vault → stores secrets** (API keys, DB passwords)
- .NET app loads configs at startup and reloads them on KV change.
