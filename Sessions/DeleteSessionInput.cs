using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Sessions;

public record DeleteSessionInput(
    [ID(nameof(Session))] int sessionId);