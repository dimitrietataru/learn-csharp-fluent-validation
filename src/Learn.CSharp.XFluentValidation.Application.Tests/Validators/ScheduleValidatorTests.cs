namespace Learn.CSharp.XFluentValidation.Application.Tests.Validators;

public sealed partial class ScheduleValidatorTests
{
    private readonly DateTime moment = new(2000, 01, 01, 00, 00, 00);

    private readonly Schedule schedule = new(
        Id: 1,
        Type: ScheduleType.Adult,
        Slot: new ScheduleSlot(new(2010, 01, 01, 12, 00, 00), new(2010, 01, 01, 13, 00, 00)),
        Person: new Person(1, 42, "John Doe"));
}
