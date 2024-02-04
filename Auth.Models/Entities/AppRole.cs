using Microsoft.AspNetCore.Identity;

namespace Auth.Models.Entities;

public class AppRole : IdentityRole<Guid>
{
    public int City { get; set; }
}