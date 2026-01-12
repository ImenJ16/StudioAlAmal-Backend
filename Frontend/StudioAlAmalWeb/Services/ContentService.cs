using System.Net.Http.Json;
using System.Net.Http.Headers;
using StudioAlAmalWeb.Models;

namespace StudioAlAmalWeb.Services;

public class ContentService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public ContentService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
        _httpClient.BaseAddress = new Uri("http://localhost:5002");
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

    // Promos
    public async Task<List<Promo>> GetPromosAsync(bool activeOnly = false)
    {
        try
        {
            var url = activeOnly ? "/api/Promos?activeOnly=true" : "/api/Promos";
            return await _httpClient.GetFromJsonAsync<List<Promo>>(url) ?? new List<Promo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting promos: {ex.Message}");
            return new List<Promo>();
        }
    }

    public async Task<Promo?> GetPromoAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Promo>($"/api/Promos/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreatePromoAsync(PromoCreateDto promo)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/Promos", promo);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating promo: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeletePromoAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"/api/Promos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // Photos
    public async Task<List<Photo>> GetPhotosAsync(bool activeOnly = false, string? category = null)
    {
        try
        {
            var url = "/api/Photos?";
            if (activeOnly) url += "activeOnly=true&";
            if (!string.IsNullOrEmpty(category)) url += $"category={category}&";

            return await _httpClient.GetFromJsonAsync<List<Photo>>(url) ?? new List<Photo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting photos: {ex.Message}");
            return new List<Photo>();
        }
    }

    public async Task<bool> CreatePhotoAsync(PhotoCreateDto photo)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/Photos", photo);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeletePhotoAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"/api/Photos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // Videos
    public async Task<List<Video>> GetVideosAsync(bool activeOnly = false, string? category = null)
    {
        try
        {
            var url = "/api/Videos?";
            if (activeOnly) url += "activeOnly=true&";
            if (!string.IsNullOrEmpty(category)) url += $"category={category}&";

            return await _httpClient.GetFromJsonAsync<List<Video>>(url) ?? new List<Video>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting videos: {ex.Message}");
            return new List<Video>();
        }
    }

    public async Task<bool> CreateVideoAsync(VideoCreateDto video)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("/api/Videos", video);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteVideoAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"/api/Videos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // About Us
    public async Task<AboutUs?> GetAboutUsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AboutUs>("/api/AboutUs");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateAboutUsAsync(AboutUsUpdateDto aboutUs)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync("/api/AboutUs", aboutUs);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}