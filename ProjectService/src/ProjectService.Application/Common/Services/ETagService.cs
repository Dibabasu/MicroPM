using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace ProjectService.Application.Common.Services;


public class ETagService
{
    private readonly string salt;

    public ETagService(IConfiguration configuration)
    {
        salt = configuration.GetValue<string>("etag:salt")!;
    }

    public string ComputeWithHashFunction(object value)
    {
        var serialized = JsonSerializer.Serialize(value);
        var valueBytes = KeyDerivation.Pbkdf2(
                password: serialized,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                 iterationCount: 10000,
                numBytesRequested: 256 / 8);
        return Convert.ToBase64String(valueBytes);
    }
}