﻿using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = "";
}
