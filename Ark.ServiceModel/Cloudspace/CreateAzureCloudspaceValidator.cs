using FluentValidation;
using System.Text.RegularExpressions;

namespace Ark.ServiceModel.Cloudspace;

public class CreateAzureCloudspaceValidator : AbstractValidator<CreateAzureCloudspaceRequest>
{
    public CreateAzureCloudspaceValidator()
    {
        RuleFor(x => x.Name).Length(3, 30)
            .Must(HaveNoSpecialChars).WithMessage("Must have not special characters")
            .Must(StartWithChar).WithMessage("Must start with characters");

        // Name must be "default" for now
        RuleFor(x => x.Name)
            .Must(x => x.ToLower().Equals("default"))
            .WithMessage("Only 'default' is accepted as a cloudspace name");

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