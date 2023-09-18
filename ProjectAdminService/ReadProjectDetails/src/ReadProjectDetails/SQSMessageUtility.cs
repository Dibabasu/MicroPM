namespace ReadProjectDetails;
public class SqsMessage
{
    public string Type { get; set; } = string.Empty;
    public string MessageId { get; set; } = string.Empty;
    public string TopicArn { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public string SignatureVersion { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string SigningCertURL { get; set; } = string.Empty;
    public string UnsubscribeURL { get; set; } = string.Empty;
    public MessageAttributes MessageAttributes { get; set; }
}

public class MessageAttributes
{
    public MessageType MessageType { get; set; }
}

public class MessageType
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class ProjectMessage
{
    public Project Project { get; set; }
}

public class Project
{
    public ProjectDetails ProjectDetails { get; set; }
    public string OwnerId { get; set; }
    public string Id { get; set; } = string.Empty;
}

public class ProjectDetails
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
