using System.ComponentModel.DataAnnotations;

namespace GraphQL.Data;

public class Conference
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Name { get; set; }

    public ICollection<Session> Sessions { get; set; } =
        new List<Session>();

    public ICollection<Track> Tracks { get; set; } = new List<Track>();

    public ICollection<ConferenceAttendee> ConferenceAttendees { get; set; } =
        new List<ConferenceAttendee>();
}
