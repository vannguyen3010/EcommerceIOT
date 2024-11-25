using Shared.DTO.Contact;

namespace EcommerceIOT.Client.Services
{
    public class ContactServices
    {
        private readonly HttpClient _httpClient;

        public ContactServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateContactAsync(CreateContactDto contact)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Contact/CreateContact", contact);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
