using FluentValidation;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ArkServer.Features.Cloudspace;


public class CloudspaceRequest
{
    public required string ProjectName { get; set; }
    public DateTime DtTimeStamp { get; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public CloudspaceRequest()
    {
        DtTimeStamp = DateTime.Now;
    }
}

public class CloudspaceRequestValidator : AbstractValidator<CloudspaceRequest>
{
    public CloudspaceRequestValidator()
    {
        RuleFor(x => x.ProjectName).Length(3, 30)
            .Must(HaveNoSpecialChars).WithMessage("Must have not special characters")
            .Must(StartWithChar).WithMessage("Must start with characters");
    }

    private bool StartWithChar(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z]");
    }

    private bool HaveNoSpecialChars(string yourString)
    {
        return yourString.Any(ch =>  char.IsLetterOrDigit(ch));
    }
}