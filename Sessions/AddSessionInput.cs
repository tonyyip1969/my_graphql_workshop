using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Sessions;

public record AddSessionInput(
    string Title,
    string? Abstract,
    int ConferenceId,
    [ID(nameof(Speaker))] IReadOnlyList<int> SpeakerIds);

