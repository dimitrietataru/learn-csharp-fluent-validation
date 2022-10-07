namespace Learn.CSharp.XFluentValidation.Application.Extensions;

public static class ScheduleTypeExtensions
{
    public static bool IsBaby(this ScheduleType @this) => @this == ScheduleType.Baby;

    public static bool IsToddler(this ScheduleType @this) => @this == ScheduleType.Toddler;

    public static bool IsChild(this ScheduleType @this) => @this == ScheduleType.Child;

    public static bool IsTeen(this ScheduleType @this) => @this == ScheduleType.Teen;

    public static bool IsAdult(this ScheduleType @this) => @this == ScheduleType.Adult;
}
