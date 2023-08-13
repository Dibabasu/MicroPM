namespace ProjectService.Domain.Entity;
public record Details
{
    public string Name { get; init; }
    public string Description { get; init; }
    public Details(string name, string description)
    {
        ValidateName(name);
        ValidateDescription(description);

        Name = name;
        Description = description;
    }
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }

        if (name.Length > 50)
        {
            throw new ArgumentException("Name cannot be longer than 50 characters.", nameof(name));
        }

        if (name.Length < 3)
        {
            throw new ArgumentException("Name cannot be shorter than 3 characters.", nameof(name));
        }

        if (!char.IsLetter(name[0]))
        {
            throw new ArgumentException("Name cannot start with a special character or number.", nameof(name));
        }

        if (!char.IsLetterOrDigit(name[^1]))
        {
            throw new ArgumentException("Name cannot end with a special character.", nameof(name));
        }
    }
    private void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));
        }

        if (description.Length > 50)
        {
            throw new ArgumentException("Description cannot be longer than 50 characters.", nameof(description));
        }
    }
};
