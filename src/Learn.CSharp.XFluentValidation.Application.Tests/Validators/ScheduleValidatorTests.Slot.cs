using FluentValidation;
using FluentValidation.TestHelper;
using Learn.CSharp.XFluentValidation.Application.Contracts;
using Learn.CSharp.XFluentValidation.Application.Providers;
using Learn.CSharp.XFluentValidation.Application.Validators;
using Moq;

namespace Learn.CSharp.XFluentValidation.Application.Tests.Validators;

public sealed partial class ScheduleValidatorTests
{
    private readonly IValidator<Schedule> scheduleValidator;
    private readonly Mock<IDateTimeProvider> mockDateTimeProvider;
    private readonly Mock<IPersonService> mockPersonService;

    public ScheduleValidatorTests()
    {
        mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider.Setup(_ => _.Now).Returns(moment).Verifiable();

        mockPersonService = new Mock<IPersonService>();

        scheduleValidator = new ScheduleValidator(
            mockDateTimeProvider.Object,
            mockPersonService.Object);
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsNotValidThenAuditFails()
    {
        // Arrange
        var testSchedule = schedule with
        {
            Type = ScheduleType.None
        };

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(_ => _.Type);
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsValidThenAuditPasses()
    {
        // Arrange
        var testSchedule = schedule;
        mockPersonService
            .Setup(_ => _.ExistsAsync(testSchedule.Person.Id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
        mockPersonService.VerifyAll();
        mockPersonService.VerifyNoOtherCalls();
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsValidAndScheduleTypeIsBabyThenAuditPasses()
    {
        // Arrange
        var testSchedule = schedule with
        {
            Type = ScheduleType.Baby,
            Slot = new ScheduleSlot(new(2020, 01, 01, 12, 00, 00), new(2020, 01, 01, 12, 29, 59)),
            Person = new Person(1, 0, "Baby")
        };
        mockPersonService
            .Setup(_ => _.ExistsAsync(testSchedule.Person.Id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
        mockPersonService.VerifyAll();
        mockPersonService.VerifyNoOtherCalls();
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsValidAndScheduleTypeIsToddlerThenAuditPasses()
    {
        // Arrange
        var testSchedule = schedule with
        {
            Type = ScheduleType.Toddler,
            Slot = new ScheduleSlot(new(2020, 01, 01, 12, 00, 00), new(2020, 01, 01, 12, 44, 59)),
            Person = new Person(1, 2, "Toddler")
        };
        mockPersonService
            .Setup(_ => _.ExistsAsync(testSchedule.Person.Id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
        mockPersonService.VerifyAll();
        mockPersonService.VerifyNoOtherCalls();
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsValidAndScheduleTypeIsChildThenAuditPasses()
    {
        // Arrange
        var testSchedule = schedule with
        {
            Type = ScheduleType.Child,
            Slot = new ScheduleSlot(new(2020, 01, 01, 12, 00, 00), new(2020, 01, 01, 12, 59, 59)),
            Person = new Person(1, 11, "Child")
        };
        mockPersonService
            .Setup(_ => _.ExistsAsync(testSchedule.Person.Id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
        mockPersonService.VerifyAll();
        mockPersonService.VerifyNoOtherCalls();
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsValidAndScheduleTypeIsTeenThenAuditPasses()
    {
        // Arrange
        var testSchedule = schedule with
        {
            Type = ScheduleType.Teen,
            Slot = new ScheduleSlot(new(2020, 01, 01, 12, 00, 00), new(2020, 01, 01, 13, 29, 59)),
            Person = new Person(1, 17, "Teen")
        };
        mockPersonService
            .Setup(_ => _.ExistsAsync(testSchedule.Person.Id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
        mockPersonService.VerifyAll();
        mockPersonService.VerifyNoOtherCalls();
    }

    [Fact]
    internal async Task GivenValidateWhenDataIsValidAndScheduleTypeIsAdultThenAuditPasses()
    {
        // Arrange
        var testSchedule = schedule with
        {
            Type = ScheduleType.Adult,
            Slot = new ScheduleSlot(new(2020, 01, 01, 12, 00, 00), new(2020, 01, 01, 13, 59, 59)),
            Person = new Person(1, 18, "Adult")
        };
        mockPersonService
            .Setup(_ => _.ExistsAsync(testSchedule.Person.Id))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var validationResult = await scheduleValidator.TestValidateAsync(testSchedule);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
        mockDateTimeProvider.VerifyAll();
        mockDateTimeProvider.VerifyNoOtherCalls();
        mockPersonService.VerifyAll();
        mockPersonService.VerifyNoOtherCalls();
    }
}
