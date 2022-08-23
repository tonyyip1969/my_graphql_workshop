using GraphQL.Data;

namespace GraphQL;

public class AddSpeakerPayload
{
	public AddSpeakerPayload(Speaker speaker)
	{
		Speaker = speaker;
	}

	public Speaker Speaker { get; }
}
