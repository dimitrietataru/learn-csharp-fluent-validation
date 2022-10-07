namespace Learn.CSharp.XFluentValidation.Application;

public sealed record Schedule(int Id, ScheduleType Type, ScheduleSlot Slot, Person Person);

public sealed record Person(int Id, int Age, string FullName);

public sealed record ScheduleSlot(DateTime From, DateTime To);

public enum ScheduleType
{
    None = 0,
    Baby = 1,
    Toddler = 2,
    Child = 3,
    Teen = 4,
    Adult = 5
}
