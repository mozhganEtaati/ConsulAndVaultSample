# Consul + Vault + Winton.NET Sample 

### A minimal yet enterprise-grade example demonstrating how to:
#### - Load application configuration from **Consul KV**
#### - Retrieve secrets securely from HashiCorp Vault using the **VaultSharp** client
#### - Auto-reload configuration on change using the **Winton.Extensions.Configuration.Consul** dynamic configuration provider
All implemented cleanly in a modern **.NET application**.

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
