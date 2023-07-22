using Newtonsoft.Json;
using Web.Domain.Models.Chat;

namespace Web.Application.Repositories.Chat;

public class ChatRepository : IChatRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IEnumerable<ChatNameIdModel>> GetChats(string jwt)
    {
        var client = _httpClientFactory.CreateClient("group");
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress + "groups/me");

        requestMessage.Headers.Add("Authorization", $"Bearer {jwt}");
        
        var response = await client.SendAsync(requestMessage);
        
        var responseBody = await response.Content.ReadAsStringAsync();

        var chats = JsonConvert.DeserializeObject<List<ChatNameIdModel>>(responseBody);

        return chats!;
    }
}