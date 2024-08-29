
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommentsController : BaseApiController
    {
        private readonly IGenericRepository<Comment> _commentRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(IGenericRepository<Comment> commentRepo, IMapper mapper, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        protected async Task<AppUser> GetAuthenticatedUserAsync()
        {
            var email = User.GetEmail();

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetComments()
        {
            var spec = new CommentSpecification();

            var comments = await _commentRepo.ListWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CommentDto>>(comments);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            var data = _mapper.Map<CommentDto>(comment);

            return Ok(data);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Invalid Model"));

            try
            {
                var user = await GetAuthenticatedUserAsync();

                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized(new ApiResponse(401, useSeriousMessages: true));
                }

                var createComment = _mapper.Map<Comment>(createCommentDto);
                createComment.UserId = user.Id;
                createComment.AppUser = user;
                createComment.CreatedDate = DateTime.UtcNow;
                Console.WriteLine($"User ID: {user.Id}");

                await _commentRepo.Create(createComment);

                var data = _mapper.Map<CommentDto>(createComment);

                return CreatedAtAction(nameof(GetComment), new { id = createComment.Id }, data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}, Inner exception: {ex.InnerException?.Message}");
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] CreateCommentDto updateCommentDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Invalid Model"));

            try
            {
                var user = await GetAuthenticatedUserAsync();

                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized(new ApiResponse(401, useSeriousMessages: true));
                }

                var spec = new CommentSpecification(id);
                var updateComment = await _commentRepo.GetEntityWithSpecAsync(spec);

                if (updateComment == null) return Unauthorized(new ApiResponse(403, "You are not authorized to update this comment"));

                if (updateComment.UserId != user.Id) return NotFound(new ApiResponse(404, useSeriousMessages: true));

                _mapper.Map(updateCommentDto, updateComment);
                await _commentRepo.Update(updateComment);

                var data = _mapper.Map<CommentDto>(updateComment);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}, Inner exception: {ex.InnerException?.Message}");
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteComment(int id)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse(400, "Invalid Model"));

            try
            {
                var user = await GetAuthenticatedUserAsync();

                if (user == null || string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized(new ApiResponse(401, useSeriousMessages: true));
                }

                var spec = new CommentSpecification(id);
                var updateComment = await _commentRepo.GetEntityWithSpecAsync(spec);

                if (updateComment == null) return Unauthorized(new ApiResponse(403, "You are not authorized to update this comment"));

                if (updateComment.UserId != user.Id) return NotFound(new ApiResponse(404, useSeriousMessages: true));

                await _commentRepo.Delete(updateComment);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}, Inner exception: {ex.InnerException?.Message}");
            }
        }
    }
}