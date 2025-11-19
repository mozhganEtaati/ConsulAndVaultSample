using System.Diagnostics;
using System.Text.Json;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace ConsulAndVaultSample.VaultSharp;

public class VaultConfigurationProvider : ConfigurationProvider, IDisposable
{

    private readonly IVaultClient _client;
    private readonly VaultConfiguration _config;
    private readonly ILogger? _logger;
    private readonly Timer _timer;

    public VaultConfigurationProvider(VaultConfiguration config, ILogger? logger)
    {
        _config = config;
        _logger = logger;

        var vaultAuthMethod = new TokenAuthMethodInfo(_config.Token);
        var vaultClientSettings = new VaultClientSettings(_config.BaseAddress, vaultAuthMethod);
        _client = new VaultClient(vaultClientSettings);
        Load();
    }

    public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    private async Task LoadAsync()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        try
        {
            var secret = await _client.V1.Secrets.KeyValue.V2.ReadSecretAsync(_config.SecretLocationPath, mountPoint: _config.MountPoint);

            if (secret?.Data?.Data != null)
            {
                var serializedData = JsonSerializer.Serialize(secret?.Data?.Data);

                JsonConfigurationParser parser = new JsonConfigurationParser();
                var secrets = parser.Parse(serializedData);

                Data = secrets;
                OnReload();
            }
        }
        catch (Exception exception)
        {
            stopWatch.Stop();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _timer != null)
            _timer.Dispose();
    }

    ~VaultConfigurationProvider()
    {
        Dispose(false);
    }
}