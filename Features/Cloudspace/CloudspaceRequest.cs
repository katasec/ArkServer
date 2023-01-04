using System.Text.Json;
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
        RuleFor(x => x.ProjectName).Length(3, 30);
    }
}