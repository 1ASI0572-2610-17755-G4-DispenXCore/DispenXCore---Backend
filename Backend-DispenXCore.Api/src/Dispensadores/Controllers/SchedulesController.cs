using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend_DispenXCore.Api.src.Dispensadores.Application.UseCases;
using Backend_DispenXCore.Api.src.Dispensadores.Application.Interfaces;

namespace Backend_DispenXCore.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/schedules")]
    [ApiController]
    [Authorize]
    public class SchedulesController : ControllerBase
    {
        private readonly AdministrarSchedulesCommand _admin;
        private readonly IDispenserRepository _repo;

        public SchedulesController(AdministrarSchedulesCommand admin, IDispenserRepository repo)
        {
            _admin = admin;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetByDispensator([FromQuery] int dispensatorId)
        {
            var schedules = await _repo.GetActiveSchedulesAsync(dispensatorId);
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _repo.GetScheduleByIdAsync(id);
            return schedule != null ? Ok(schedule) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleRequest dto)
        {
            var schedule = await _admin.Create(dto.DispensatorId, dto.Name, dto.SupplyType,
                dto.Amount, dto.ScheduledTime, dto.FrequencyDays, dto.SmartRefill);
            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleRequest dto)
        {
            await _admin.Update(id, dto.Name, dto.SupplyType, dto.Amount,
                dto.ScheduledTime, dto.FrequencyDays, dto.SmartRefill, dto.IsActive);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _admin.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id)
        {
            await _admin.Toggle(id);
            return NoContent();
        }
    }

    public record ScheduleRequest(int DispensatorId, string Name, string SupplyType,
        int Amount, string ScheduledTime, List<int> FrequencyDays, bool SmartRefill, bool IsActive = true);
}