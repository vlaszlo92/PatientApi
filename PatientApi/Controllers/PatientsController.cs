using Application.Patients.Commands.CreatePatient;
using Application.Patients.Commands.DeletePatient;
using Application.Patients.Queries.GetPatients;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PatientApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: /api/v1/patients
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message); // csak a hibaüzenet szövegét küldi vissza
        }
    }

    // GET: /api/v1/patients
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetPatientsQuery(page, pageSize));
        return Ok(result);
    }

    // GET: /api/v1/patients/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // Bővíthető, ha létrehozunk később GetPatientByIdQuery-t
        return NotFound("A GET /api/patients/{id} még nincs implementálva.");
    }

    // PUT: /api/v1/patients/{id}/diagnosis
    [HttpPut("{id}/diagnosis")]
    public async Task<IActionResult> UpdateDiagnosis(Guid id, [FromBody] UpdatePatientCommand command)
    {
        if (id != command.Id)
            return BadRequest("Az ID az útvonalban és a törzsben nem egyezik.");

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeletePatientCommand(id));
        return result ? NoContent() : NotFound();
    }

}
