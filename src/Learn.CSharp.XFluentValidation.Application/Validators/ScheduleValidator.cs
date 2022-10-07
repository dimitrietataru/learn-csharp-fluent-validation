using Learn.CSharp.XFluentValidation.Application.Contracts;
using Learn.CSharp.XFluentValidation.Application.Extensions;
using Learn.CSharp.XFluentValidation.Application.Providers;

namespace Learn.CSharp.XFluentValidation.Application.Validators;

public sealed class ScheduleValidator : AbstractValidator<Schedule>
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IPersonService personService;

    public ScheduleValidator(IDateTimeProvider dateTimeProvider, IPersonService personService)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.personService = personService;

        RegisterRulesForId();
        RegisterRulesForType();
        RegisterRulesForSlot();
        RegisterRulesForPerson();
    }

    private void RegisterRulesForId()
    {
        const int MinValue = 1;

        RuleFor(schedule => schedule.Id)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(MinValue);
    }

    private void RegisterRulesForType()
    {
        RuleFor(schedule => schedule.Type)
            .Cascade(CascadeMode.Stop)
            .IsInEnum()
            .Must(NotBeDefault)
                .WithMessage($"ScheduleType value must not be default ({ScheduleType.None}).");

        static bool NotBeDefault(ScheduleType schedule)
        {
            return schedule != ScheduleType.None;
        }
    }

    private void RegisterRulesForSlot()
    {
        RuleFor(schedule => schedule.Slot)
            .Cascade(CascadeMode.Stop)
            .NotNull();
            ////.SetValidator((schedule, _) => new ScheduleSlotTypeValidator(schedule.Type));

        When(
            schedule => schedule.Type.IsBaby(),
            () => RuleFor(schedule => schedule.Slot)
                .SetValidator(new BabyScheduleSlotValidator(dateTimeProvider)));

        When(
            schedule => schedule.Type.IsToddler(),
            () => RuleFor(schedule => schedule.Slot)
                .SetValidator(new ToddlerScheduleSlotValidator(dateTimeProvider)));

        When(
            schedule => schedule.Type.IsChild(),
            () => RuleFor(schedule => schedule.Slot)
                .SetValidator(new ChildScheduleSlotValidator(dateTimeProvider)));

        When(
            schedule => schedule.Type.IsTeen(),
            () => RuleFor(schedule => schedule.Slot)
                .SetValidator(new TeenScheduleSlotValidator(dateTimeProvider)));

        When(
            schedule => schedule.Type.IsAdult(),
            () => RuleFor(schedule => schedule.Slot)
                .SetValidator(new AdultScheduleSlotValidator(dateTimeProvider)));
    }

    private void RegisterRulesForPerson()
    {
        RuleFor(schedule => schedule.Person)
            .Cascade(CascadeMode.Stop)
            .NotNull();
            ////.SetValidator(new BabyPersonValidator(personService))
            ////    .When(schedule => schedule.Type.IsBaby(), ApplyConditionTo.CurrentValidator)
            ////.SetValidator(new ToddlerPersonValidator(personService))
            ////    .When(schedule => schedule.Type.IsToddler(), ApplyConditionTo.CurrentValidator)
            ////.SetValidator(new ChildPersonValidator(personService))
            ////    .When(schedule => schedule.Type.IsChild(), ApplyConditionTo.CurrentValidator)
            ////.SetValidator(new TeenPersonValidator(personService))
            ////    .When(schedule => schedule.Type.IsTeen(), ApplyConditionTo.CurrentValidator)
            ////.SetValidator(new AdultPersonValidator(personService))
            ////    .When(schedule => schedule.Type.IsAdult(), ApplyConditionTo.CurrentValidator);

        When(
            schedule => schedule.Type.IsBaby(),
            () => RuleFor(schedule => schedule.Person)
                .SetValidator(new BabyPersonValidator(personService)));

        When(
            schedule => schedule.Type.IsToddler(),
            () => RuleFor(schedule => schedule.Person)
                .SetValidator(new ToddlerPersonValidator(personService)));

        When(
            schedule => schedule.Type.IsChild(),
            () => RuleFor(schedule => schedule.Person)
                .SetValidator(new ChildPersonValidator(personService)));

        When(
            schedule => schedule.Type.IsTeen(),
            () => RuleFor(schedule => schedule.Person)
                .SetValidator(new TeenPersonValidator(personService)));

        When(
            schedule => schedule.Type.IsAdult(),
            () => RuleFor(schedule => schedule.Person)
                .SetValidator(new AdultPersonValidator(personService)));
    }
}
