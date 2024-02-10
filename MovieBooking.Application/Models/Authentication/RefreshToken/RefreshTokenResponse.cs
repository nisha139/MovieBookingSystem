namespace MovieBooking.Application.Models.Authentication;

public record RefreshTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
