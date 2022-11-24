using System.Runtime.CompilerServices;
using Utf8Json;

// ReSharper disable UnusedMember.Global

namespace Hermes.Services;

internal sealed class JsonService
{
    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public string ToJsonString<T>(in T source)
    {
        return JsonSerializer.ToJsonString(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public byte[] SerializeJson<T>(in T source)
    {
        return JsonSerializer.Serialize(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public Task SerializeJsonAsync<T>(in T source, ref Stream output)
    {
        return JsonSerializer.SerializeAsync(output, source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public T DeserializeJson<T>(in byte[] source)
    {
        return JsonSerializer.Deserialize<T>(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public Task<T> DeserializeJsonAsync<T>(in byte[] source)
    {
        return JsonSerializer.DeserializeAsync<T>(new MemoryStream(source));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
    public Task<T> DeserializeJsonAsync<T>(in Stream source)
    {
        return JsonSerializer.DeserializeAsync<T>(source);
    }
}
