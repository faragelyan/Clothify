using System;

namespace Clothify.Application.DTOs.UserPhone
{
    public class CreateUserPhoneDto
    {
        public string PhoneNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
