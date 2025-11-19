namespace ConsulAndVaultSample;

public static class ConsulFailover
{
    public static string SelectHealthyConsul(string[] addresses)
    {
        foreach (var address in addresses)
        {
            try
            {
                using var http = new HttpClient();
                var result = http.GetAsync($"{address}/v1/status/leader").Result;

                if (result.IsSuccessStatusCode)
                    return address;
            }
            catch { /* ignore */ }
        }

        throw new Exception("No healthy Consul nodes available!");
    }
}
