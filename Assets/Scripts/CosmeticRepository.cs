using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D.Animation;

internal sealed class CosmeticRepository : IDisposable
{
    private static readonly Dictionary<string, AsyncOperationHandle<IList<SpriteLibraryAsset>>> Operations = new();
    private readonly string[] _keys;

    public CosmeticRepository(IEnumerable<string> keys)
    {
        _keys = keys.ToArray();
    }
    
    public async Task InitializeAsync()
    {
        foreach (var key in _keys)
        {
            var handle = Addressables.LoadAssetsAsync<SpriteLibraryAsset>(key, null);

            await handle.Task;
            
            Operations.Add(key, handle);
        }
    }

    public async Task<SpriteLibraryAsset> GetHeadByIdAsync(int id) => await GetSpriteLibraries(_keys[0], id);
    public async Task<SpriteLibraryAsset> GetBodyByIdAsync(int id) => await GetSpriteLibraries(_keys[1], id);
    public async Task<SpriteLibraryAsset> GetHairByIdAsync(int id) => await GetSpriteLibraries(_keys[2], id);
    public async Task<SpriteLibraryAsset> GetTorsoByIdAsync(int id) => await GetSpriteLibraries(_keys[3], id);
    public async Task<SpriteLibraryAsset> GetLegsByIdAsync(int id) => await GetSpriteLibraries(_keys[4], id);
    public async Task<SpriteLibraryAsset> GetFeetByIdAsync(int id) => await GetSpriteLibraries(_keys[5], id);

    public void Dispose()
    {
        foreach (var key in _keys)
        {
            Addressables.Release(Operations[key]);
            
            Operations.Remove(key);
        }
    }

    private async Task<SpriteLibraryAsset> GetSpriteLibraries(string key, int id)
    {
        if (HasLoadedSprites)
        {
            return await LoadSpriteByIdAsync(key, id);
        }

        LogWarning();
        return null;
    }

    private Task<SpriteLibraryAsset> LoadSpriteByIdAsync(string key, int id)
    {
        var sprites = Operations[key];

        return Task.FromResult(id >= sprites.Result.Count ? null : sprites.Result[id]);
    }

    private bool HasLoadedSprites => Operations.Count > 0;
    
    private static void LogWarning() => Debug.LogWarning($"No sprites found, ensure {nameof(InitializeAsync)} has been called first");
}