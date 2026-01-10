namespace CommunicationService.Models;

public class ContactSubmission
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
}