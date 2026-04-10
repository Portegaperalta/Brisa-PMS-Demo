using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;
using FluentValidation;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoCommandValidatorTests
{
    private readonly UpdateHotelAddressInfoCommandValidator _commandValidator;

    public UpdateHotelAddressInfoCommandValidatorTests()
    {
        _commandValidator = new UpdateHotelAddressInfoCommandValidator();
    }
    
    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateUpdateHotelAddressInfoCommand(Guid.Empty, "", "", "", "", "");
        
        // Act
        var result =  _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
        result.ShouldHaveValidationErrorFor(x => x.Address1);
        result.ShouldHaveValidationErrorFor(x => x.City);
        result.ShouldHaveValidationErrorFor(x => x.Province);
        result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedsMaxLength()
    {
        // Arrange
        var command = CreateUpdateHotelAddressInfoCommand
        (
            Guid.NewGuid(),
            new string('A', 201), 
            new string('A', 201), 
            new string('C', 101),
            new string('P', 101), 
            new string('Z', 11)
        );
        
        // Act
        var result =  _commandValidator.TestValidate(command);
        
        // Arrange
        result.ShouldHaveValidationErrorFor(x => x.Address1);
        result.ShouldHaveValidationErrorFor(x => x.Address2);
        result.ShouldHaveValidationErrorFor(x => x.City);
        result.ShouldHaveValidationErrorFor(x => x.Province);
        result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    }

    [Theory]
    [InlineData("A12345")]
    [InlineData("@1B23456")]
    public void Validator_HasError_WhenZipCodeIsInvalid(string invalidZipCode)
    {
        // Arrange
        var command = CreateUpdateHotelAddressInfoCommand
            (
                Guid.NewGuid(),
                CreateAddress1(), 
                CreateAddress2(), 
                CreateCity(),
                CreateProvince(), 
                invalidZipCode
            );
        
        // Act
        var result =  _commandValidator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    }
    
    // Helper methods
    private static UpdateHotelAddressInfoCommand 
        CreateUpdateHotelAddressInfoCommand(Guid hotelId, string address1, string address2, string city, 
            string province, string zipCode)
    {
        return new UpdateHotelAddressInfoCommand
        {
            HotelId = hotelId,
            Address1 = address1,
            Address2 = address2,
            City = city,
            Province = province,
            ZipCode = zipCode
        };
    }
    
    private static string CreateAddress1() => "223 North street";
    private static string CreateAddress2() => "Suite 2C";
    private static string CreateCity() => "Santiago";
    private static  string CreateProvince() => "Cibao";
}