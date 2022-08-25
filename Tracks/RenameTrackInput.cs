using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Tracks;

public record RenameTrackInput([ID(nameof(Track))] int Id, string Name);
