using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using StudioAlAmalWeb.Models;

namespace StudioAlAmalWeb.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _httpClient.BaseAddress = new Uri("http://localhost:5001");
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await _localStorage.SetItemAsStringAsync(TokenKey, authResponse.Token);
                    SetAuthorizationHeader(authResponse.Token);
                }
                return authResponse;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return null;
        }
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await _localStorage.SetItemAsStringAsync(TokenKey, authResponse.Token);
                    SetAuthorizationHeader(authResponse.Token);
                }
                return authResponse;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Register error: {ex.Message}");
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(TokenKey);
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(TokenKey);
    }

    public async Task InitializeAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            SetAuthorizationHeader(token);
        }
    }

    private void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}