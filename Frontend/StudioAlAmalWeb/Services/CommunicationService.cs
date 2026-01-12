using System.Net.Http.Json;
using System.Net.Http.Headers;
using StudioAlAmalWeb.Models;

namespace StudioAlAmalWeb.Services;

public class CommunicationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public CommunicationService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
        _httpClient.BaseAddress = new Uri("http://localhost:5003");
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<bool> SubmitContactFormAsync(ContactSubmissionCreateDto contact)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Contact", contact);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error submitting contact form: {ex.Message}");
            return false;
        }
    }

    public async Task<List<ContactSubmission>> GetContactSubmissionsAsync(bool unreadOnly = false)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var url = unreadOnly ? "/api/Contact?unreadOnly=true" : "/api/Contact";
            return await _httpClient.GetFromJsonAsync<List<ContactSubmission>>(url) ?? new List<ContactSubmission>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting contact submissions: {ex.Message}");
            return new List<ContactSubmission>();
        }
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsync($"/api/Contact/{id}/mark-read", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteSubmissionAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"/api/Contact/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}