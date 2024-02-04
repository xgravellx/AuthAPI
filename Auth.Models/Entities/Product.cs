using Microsoft.EntityFrameworkCore;

namespace Auth.Models.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    [Precision(18,2)]
    public decimal Price { get; set; }
}