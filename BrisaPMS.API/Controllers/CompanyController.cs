using BrisaPMS.API.DTOs.Company;
using BrisaPMS.Application.UseCases.Companies;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;
using BrisaPMS.Application.UseCases.Companies.Queries.GetCompanyInfo;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers;

[ApiController]
[Route("api/company")]
[Authorize(Policy = "AdminManagerOnly")]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/info")]
    public async Task<CompanyDto> GetCompanyInfo([FromRoute] Guid id)
    {
        var query = new GetCompanyInfoQuery { CompanyId = id };
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpPut("{id:guid}/address")]
    public async Task<IActionResult> UpdateCompanyAddress([FromRoute] Guid id, [FromBody] UpdateCompanyAddressDto dto)
    {
        var command = new UpdateCompanyAddressInfoCommand
        {
            CompanyId = id,
            NewAddress1 = dto.NewAddress1,
            NewAddress2 = dto.NewAddress2,
            NewCity = dto.NewCity,
            NewProvince = dto.NewProvince,
            NewZipCode = dto.NewZipCode
        };
        
        await  _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/brand")]
    public async Task<IActionResult> UpdateCompanyBrand([FromRoute] Guid id, [FromBody] UpdateCompanyBrandDto dto)
    {
        var command = new UpdateCompanyBrandInfoCommand
        {
            CompanyId = id,
            NewCommercialName = dto.NewCommercialName,
            NewLegalName = dto.NewLegalName,
            NewLogoUrl = dto.NewLogoUrl,
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/contact")]
    public async Task<IActionResult> UpdateCompanyContactInfo([FromRoute] Guid id, [FromBody] UpdateCompanyContactInfoDto dto)
    {
        var command = new UpdateCompanyContactInfoCommand
        {
            CompanyId = id,
            NewBusinessEmail = dto.NewBusinessEmail,
            NewBusinessPhoneNumber = dto.NewBusinessPhoneNumber
        };
        
        await  _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/rnc")]
    public async Task<IActionResult> UpdateCompanyRnc([FromRoute] Guid id, [FromBody] UpdateCompanyRncDto dto)
    {
        var command = new UpdateCompanyRncCommand
        {
            CompanyId = id,
            NewRnc = dto.NewRnc
        };
        
        await  _mediator.Send(command);
        return NoContent();
    }
}