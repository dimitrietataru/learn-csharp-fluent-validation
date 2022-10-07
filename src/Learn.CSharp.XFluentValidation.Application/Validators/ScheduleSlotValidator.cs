using Learn.CSharp.XFluentValidation.Application.Extensions;
using Learn.CSharp.XFluentValidation.Application.Providers;

namespace Learn.CSharp.XFluentValidation.Application.Validators;

public abstract class ScheduleSlotValidator : AbstractValidator<ScheduleSlot>
{
    private readonly IDateTimeProvider dateTimeProvider;

    protected ScheduleSlotValidator(IDateTimeProvider dateTimeProvider)
    {
        this.dateTimeProvider = dateTimeProvider;

        RegisterRulesForFrom();
        RegisterRulesForTo();
    }

    protected abstract TimeSpan IntervalLowerThreshold { get; }
    protected abstract TimeSpan IntervalUpperThreshold { get; }

    protected bool BeValidSlotRange(ScheduleSlot slot, DateTime _)
    {
        var diff = slot.To - slot.From;

        return IntervalLowerThreshold <= diff && diff < IntervalUpperThreshold;
    }

    private void RegisterRulesForFrom()
    {
        var moment = dateTimeProvider.Now;

        RuleFor(scheduleSlot => scheduleSlot.From)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(moment)
            .Must(BeLessThanTo)
                .WithMessage("From should be less than To");

        static bool BeLessThanTo(ScheduleSlot slot, DateTime from)
        {
            return from < slot.To;
        }
    }

    private void RegisterRulesForTo()
    {
        var moment = dateTimeProvider.Now;

        RuleFor(scheduleSlot => scheduleSlot.To)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(moment)
            .Must(BeGreaterThanFrom)
                .WithMessage("To should be greater than From");

        static bool BeGreaterThanFrom(ScheduleSlot slot, DateTime to)
        {
            return to > slot.From;
        }
    }
}

public sealed class BabyScheduleSlotValidator : ScheduleSlotValidator
{
    public BabyScheduleSlotValidator(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
        RuleFor(scheduleSlot => scheduleSlot.To)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidSlotRange)
                .WithMessage("ScheduleType Baby interval must be between 10-30 minutes");
    }

    protected sealed override TimeSpan IntervalLowerThreshold => TimeSpan.FromMinutes(10);
    protected sealed override TimeSpan IntervalUpperThreshold => TimeSpan.FromMinutes(30);
}

public sealed class ToddlerScheduleSlotValidator : ScheduleSlotValidator
{
    public ToddlerScheduleSlotValidator(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
        RuleFor(scheduleSlot => scheduleSlot.To)
            .Must(BeValidSlotRange)
                .WithMessage("ScheduleType Toddler interval must be between 10-45 minutes");
    }

    protected sealed override TimeSpan IntervalLowerThreshold => TimeSpan.FromMinutes(10);
    protected sealed override TimeSpan IntervalUpperThreshold => TimeSpan.FromMinutes(45);
}

public sealed class ChildScheduleSlotValidator : ScheduleSlotValidator
{
    public ChildScheduleSlotValidator(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
        RuleFor(scheduleSlot => scheduleSlot.To)
            .Must(BeValidSlotRange)
                .WithMessage("ScheduleType Child interval must be between 30-60 minutes");
    }

    protected sealed override TimeSpan IntervalLowerThreshold => TimeSpan.FromMinutes(30);
    protected sealed override TimeSpan IntervalUpperThreshold => TimeSpan.FromMinutes(60);
}

public sealed class TeenScheduleSlotValidator : ScheduleSlotValidator
{
    public TeenScheduleSlotValidator(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
        RuleFor(scheduleSlot => scheduleSlot.To)
            .Must(BeValidSlotRange)
                .WithMessage("ScheduleType Teen interval must be between 30-90 minutes");
    }

    protected sealed override TimeSpan IntervalLowerThreshold => TimeSpan.FromMinutes(30);
    protected sealed override TimeSpan IntervalUpperThreshold => TimeSpan.FromMinutes(90);
}

public sealed class AdultScheduleSlotValidator : ScheduleSlotValidator
{
    public AdultScheduleSlotValidator(IDateTimeProvider dateTimeProvider)
        : base(dateTimeProvider)
    {
        RuleFor(scheduleSlot => scheduleSlot.To)
            .Must(BeValidSlotRange)
                .WithMessage("ScheduleType Adult interval must be between 60-120 minutes");
    }

    protected sealed override TimeSpan IntervalLowerThreshold => TimeSpan.FromMinutes(60);
    protected sealed override TimeSpan IntervalUpperThreshold => TimeSpan.FromMinutes(120);
}

public sealed class ScheduleSlotTypeValidator : AbstractValidator<ScheduleSlot>
{
    private readonly ScheduleType scheduleType;

    private readonly TimeSpan intervalForBaby = TimeSpan.FromMinutes(30);
    private readonly TimeSpan intervalForToddler = TimeSpan.FromMinutes(45);
    private readonly TimeSpan intervalForChild = TimeSpan.FromMinutes(60);
    private readonly TimeSpan intervalForTeen = TimeSpan.FromMinutes(90);
    private readonly TimeSpan intervalForAdult = TimeSpan.FromMinutes(120);

    public ScheduleSlotTypeValidator(ScheduleType scheduleType)
    {
        this.scheduleType = scheduleType;

        RuleFor(scheduleSlot => scheduleSlot.To)
            .Must(BeInAllowedTimespanForBaby)
                .When(ScheduleTypeIsBaby)
            .Must(BeInAllowedTimespanForToddler)
                .When(ScheduleTypeIsToddler)
            .Must(BeInAllowedTimespanForChild)
                .When(ScheduleTypeIsChild)
            .Must(BeInAllowedTimespanForTeen)
                .When(ScheduleTypeIsTeen)
            .Must(BeInAllowedTimespanForAdult)
                .When(ScheduleTypeIsAdult)
            .WithMessage("Invalid time slot");
    }

    private bool BeInAllowedTimespanForBaby(ScheduleSlot slot, DateTime _)
        => BeInAllowedTimeSpan(slot, intervalForBaby);

    private bool BeInAllowedTimespanForToddler(ScheduleSlot slot, DateTime _)
        => BeInAllowedTimeSpan(slot, intervalForToddler);

    private bool BeInAllowedTimespanForChild(ScheduleSlot slot, DateTime _)
        => BeInAllowedTimeSpan(slot, intervalForChild);

    private bool BeInAllowedTimespanForTeen(ScheduleSlot slot, DateTime _)
        => BeInAllowedTimeSpan(slot, intervalForTeen);

    private bool BeInAllowedTimespanForAdult(ScheduleSlot slot, DateTime _)
        => BeInAllowedTimeSpan(slot, intervalForAdult);

    private bool BeInAllowedTimeSpan(ScheduleSlot slot, TimeSpan timeSpan)
    {
        if (scheduleType is ScheduleType.None)
        {
            throw new ArgumentOutOfRangeException("scheduleType", "NOT ALLOWED");
        }

        return slot.To - slot.From <= timeSpan;
    }

    private bool ScheduleTypeIsBaby(ScheduleSlot _) => scheduleType.IsBaby();

    private bool ScheduleTypeIsToddler(ScheduleSlot _) => scheduleType.IsToddler();

    private bool ScheduleTypeIsChild(ScheduleSlot _) => scheduleType.IsChild();

    private bool ScheduleTypeIsTeen(ScheduleSlot _) => scheduleType.IsTeen();

    private bool ScheduleTypeIsAdult(ScheduleSlot _) => scheduleType.IsAdult();
}
