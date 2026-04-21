using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;

public class ChangeAssignedRoomCommandValidatorTests
{
    private readonly ChangeAssignedRoomCommandValidator _validator;

    public ChangeAssignedRoomCommandValidatorTests()
    {
        _validator = new ChangeAssignedRoomCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenBookingIdIsEmpty()
    {
        var command = CreateCommand(Guid.Empty, Guid.NewGuid());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BookingId);
    }

    [Fact]
    public void Validator_HasErrors_WhenRoomIdIsEmpty()
    {
        var command = CreateCommand(Guid.NewGuid(), Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        var command = CreateCommand(Guid.Empty, Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BookingId);
        result.ShouldHaveValidationErrorFor(x => x.RoomId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        var command = CreateValidCommand();

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static ChangeAssignedRoomCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid(), Guid.NewGuid());
    }

    private static ChangeAssignedRoomCommand CreateCommand(Guid bookingId, Guid roomId)
    {
        return new ChangeAssignedRoomCommand
        {
            BookingId = bookingId,
            RoomId = roomId
        };
    }
}
