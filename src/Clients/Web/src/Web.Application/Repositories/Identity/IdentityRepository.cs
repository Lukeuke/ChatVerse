using System.Net;
using Newtonsoft.Json;
using Web.Domain.DTOs;

namespace Web.Application.Repositories.Identity;

public class IdentityRepository : IIdentityRepository
{
    private readonly HttpClient _httpClient;

    public IdentityRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5222");
    }
    
    public async Task<(bool, object)> RegisterUser(RegisterUserDto userDto)
    {        
        var serializedUser = JsonConvert.SerializeObject(userDto);

        var requestMessage = new HttpRequestMessage(HttpMethod.Put, _httpClient.BaseAddress + "auth");
        requestMessage.Content = new StringContent(serializedUser);

        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.SendAsync(requestMessage);
        
        var responseBody = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.Created)
        {
            return (true, string.Empty);
        }

        object? content = JsonConvert.DeserializeObject<MessageResponseDto>(responseBody);
        return (false, content.ToString());
    }

    public async Task<(bool, object)> LoginUser(SignInRequestDto userDto)
    {
        var serializedUser = JsonConvert.SerializeObject(userDto);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress + "auth");
        requestMessage.Content = new StringContent(serializedUser);
        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        
        var response = await _httpClient.SendAsync(requestMessage);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = JsonConvert.DeserializeObject<SignInResponseDto>(responseBody);
            return (true, content);
        }

        var error = JsonConvert.DeserializeObject<MessageResponseDto>(responseBody);
        return (false, error.ToString());
    }
}