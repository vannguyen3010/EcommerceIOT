using Shared.DTO.Contact;

namespace Shared.DTO.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class ContactResponse
    {
        public int totalCount { get; set; }
        public IEnumerable<ContactDto> contacts { get; set; }
    }
}
