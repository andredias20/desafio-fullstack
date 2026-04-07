using System.ComponentModel.DataAnnotations;

namespace WeatherApp.API.Requests;

public record AuthRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password);
