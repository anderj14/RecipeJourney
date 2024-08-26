using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InstructionsController : BaseApiController
    {
        private readonly IGenericRepository<Instruction> _instructionRepo;
        private readonly IMapper _mapper;

        public InstructionsController(IGenericRepository<Instruction> instructionRepo, IMapper mapper)
        {
            _instructionRepo = instructionRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructionDto>> GetInstruction(int id)
        {
            var ingredient = await _instructionRepo.GetByIdAsync(id);

            if (ingredient == null) return NotFound(new ApiResponse(404, "Instruction Not Found"));

            var data = _mapper.Map<InstructionDto>(ingredient);

            return Ok(data);
        }


        [HttpPost]
        public async Task<ActionResult<InstructionDto>> CreateInstruction([FromBody] CreateInstructionDto createInstructionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));

            var newInstruction = _mapper.Map<CreateInstructionDto, Instruction>(createInstructionDto);

            _instructionRepo.Create(newInstruction);
            await _instructionRepo.SaveAsync();

            var data = _mapper.Map<InstructionDto>(newInstruction);

            return CreatedAtAction(nameof(GetInstruction), new { id = newInstruction.Id }, data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InstructionDto>> UpdateInstruction(int id, CreateInstructionDto updateInstructionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));
            try
            {
                var spec = new InstructionSpecification(id);
                var existingInstruction = await _instructionRepo.GetEntityWithSpecAsync(spec);

                if (existingInstruction == null) return NotFound(new ApiResponse(404, "Instruction Not Found"));

                _mapper.Map(updateInstructionDto, existingInstruction);

                _instructionRepo.Update(existingInstruction);
                await _instructionRepo.SaveAsync();

                var data = _mapper.Map<InstructionDto>(existingInstruction);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> UpdateInstruction(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data"));
            try
            {
                var spec = new InstructionSpecification(id);
                var existingInstruction = await _instructionRepo.GetEntityWithSpecAsync(spec);

                if (existingInstruction == null) return NotFound(new ApiResponse(404, "Instruction Not Found"));

                _instructionRepo.Delete(existingInstruction);
                await _instructionRepo.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}