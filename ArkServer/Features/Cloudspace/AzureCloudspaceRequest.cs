using FluentValidation;
using System.Text.RegularExpressions;
using System.Text.Json;
namespace ArkServer.Features.Cloudspace;

public class AzureCloudspaceRequest : BaseRequest
{
    public required string  Name { get; set; }
    public required List<string> Environments { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true});
    }
}

public class CloudspaceRequestValidator : AbstractValidator<AzureCloudspaceRequest>
{
    public CloudspaceRequestValidator()
    {
        RuleFor(x => x.Name).Length(3, 30)
            .Must(HaveNoSpecialChars).WithMessage("Must have not special characters")
            .Must(StartWithChar).WithMessage("Must start with characters");
        RuleFor(x => x.Environments)
            .Must(HaveNoSpecialChars).WithMessage("Must have not special characters");
    }

    private bool StartWithChar(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z]");
    }

    private bool HaveNoSpecialChars(string word)
    {
        return word.Any(ch => char.IsLetterOrDigit(ch));
    }

    private bool HaveNoSpecialChars(IEnumerable<string> items)
    {
        var status = false;
        foreach (var item in items)
        {
            if (item.Any(ch => char.IsLetterOrDigit(ch))) {
                status = true;
            }
        }

        return status;
    }
}