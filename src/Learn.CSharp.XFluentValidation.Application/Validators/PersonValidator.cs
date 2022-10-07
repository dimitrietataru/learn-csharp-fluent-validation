using Learn.CSharp.XFluentValidation.Application.Contracts;

namespace Learn.CSharp.XFluentValidation.Application.Validators;

public abstract class PersonValidator : AbstractValidator<Person>
{
    private readonly IPersonService personService;

    protected PersonValidator(IPersonService personService)
    {
        this.personService = personService;

        RegisterRulesForId();
        RegisterRulesForAge();
        RegisterRulesForFullName();
    }

    protected abstract int AgeLowerThreshold { get; }
    protected abstract int AgeUpperThreshold { get; }

    protected bool BeValidAge(Person person, int _)
    {
        return AgeLowerThreshold <= person.Age && person.Age < AgeUpperThreshold;
    }

    private void RegisterRulesForId()
    {
        const int MinValue = 1;

        RuleFor(person => person.Id)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(MinValue)
            .MustAsync(ExistInDatabaseAsync);

        async Task<bool> ExistInDatabaseAsync(int personId, CancellationToken _)
        {
            return await personService.ExistsAsync(personId).ConfigureAwait(false);
        }
    }

    private void RegisterRulesForAge()
    {
        const int MinValue = 0;

        RuleFor(person => person.Age)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(MinValue);
    }

    private void RegisterRulesForFullName()
    {
        const int MaxLength = 100;

        RuleFor(person => person.FullName)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MaximumLength(MaxLength);
    }
}

public sealed class BabyPersonValidator : PersonValidator
{
    public BabyPersonValidator(IPersonService personService)
        : base(personService)
    {
        RuleFor(person => person.Age)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidAge)
                .WithMessage("ScheduleType Baby age must be between 0-1 years");
    }

    protected override int AgeLowerThreshold => 0;
    protected override int AgeUpperThreshold => 1;
}

public sealed class ToddlerPersonValidator : PersonValidator
{
    public ToddlerPersonValidator(IPersonService personService)
        : base(personService)
    {
        RuleFor(person => person.Age)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidAge)
                .WithMessage("ScheduleType Toddler age must be between 1-3 years");
    }

    protected override int AgeLowerThreshold => 1;
    protected override int AgeUpperThreshold => 3;
}

public sealed class ChildPersonValidator : PersonValidator
{
    public ChildPersonValidator(IPersonService personService)
        : base(personService)
    {
        RuleFor(person => person.Age)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidAge)
                .WithMessage("ScheduleType Child age must be between 3-12 years");
    }

    protected override int AgeLowerThreshold => 3;
    protected override int AgeUpperThreshold => 12;
}

public sealed class TeenPersonValidator : PersonValidator
{
    public TeenPersonValidator(IPersonService personService)
        : base(personService)
    {
        RuleFor(person => person.Age)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidAge)
                .WithMessage("ScheduleType Teen age must be between 12-18 years");
    }

    protected override int AgeLowerThreshold => 12;
    protected override int AgeUpperThreshold => 18;
}

public sealed class AdultPersonValidator : PersonValidator
{
    public AdultPersonValidator(IPersonService personService)
        : base(personService)
    {
        RuleFor(person => person.Age)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidAge)
                .WithMessage("ScheduleType Adult age must be 18+ years");
    }

    protected override int AgeLowerThreshold => 18;
    protected override int AgeUpperThreshold => int.MaxValue;
}
