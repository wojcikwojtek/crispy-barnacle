using System.Net.Http.Json;

public class ContactClient
{
    private readonly HttpClient _httpClient;

    public ContactClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SendAsync(object contact)
    {
        var resp = await _httpClient.PostAsJsonAsync("api/contact", contact);
        return resp.IsSuccessStatusCode;
    }
}
