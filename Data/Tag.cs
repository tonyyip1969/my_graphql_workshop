using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data;

public class Tag
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
}
