using Ark.ServiceModel.Cloudspace;
using FluentValidation;

namespace Ark.Server;

public class ConfigValidator : AbstractValidator<Config>
{
    public ConfigValidator()
    {
        RuleFor(x => x.AzureConfig.MqConfig.MqName).NotEmpty();
        RuleFor(x => x.AzureConfig.MqConfig.MqConnectionString).NotEmpty();
    }
}
