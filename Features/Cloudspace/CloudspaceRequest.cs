using System.Text.Json;
using System.Text.RegularExpressions;
using FluentValidation;

namespace ArkServer.Features.Cloudspace;


public class CloudspaceRequest
{
    public required string ProjectName { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class CloudspaceRequestValidator : AbstractValidator<CloudspaceRequest>
{
    public CloudspaceRequestValidator()
    {
        RuleFor(x => x.ProjectName).Length(3, 30).Must(StartWithChar).WithMessage("Must start with characters");
    }

    private bool StartWithChar(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z]");
    }
}

