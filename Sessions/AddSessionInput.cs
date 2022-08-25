using HotChocolate.Types.Relay;

namespace GraphQL.Sessions;

public record AddSessionInput(
    string Title,
    string? Abstract,
    [ID(nameof(Speakers))] IReadOnlyList<int> SpeakerIds);

