using Blazored.LocalStorage;

namespace Ecommerce.Client.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string StorageKey = "authentication-token";
        public async Task<string> GetToken() => await localStorageService.GetItemAsStringAsync(StorageKey);
        public async Task SetToken(string item) => await localStorageService.SetItemAsStringAsync(StorageKey, item);
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(StorageKey);
    }
}
