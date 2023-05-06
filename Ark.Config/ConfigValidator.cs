using FluentValidation;

namespace Ark.Base;

public class ConfigValidator : AbstractValidator<Config>
{
    public ConfigValidator()
    {
        RuleFor(x => x.AzureConfig.MqConfig.MqName).NotEmpty();
        RuleFor(x => x.AzureConfig.MqConfig.MqConnectionString).NotEmpty();
    }
}
