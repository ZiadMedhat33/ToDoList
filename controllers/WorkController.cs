using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ToDoList.Services;
using ToDoList.Entites;

namespace ToDoList.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkController : ControllerBase
    {
        private readonly WorkService _workService;

        public WorkController(WorkService workService)
        {
            _workService = workService;
        }

        [HttpPost("add/{content}/{Private}")]
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AddWork(string content, bool Private)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            long userId = long.Parse(userIdStr);
            try
            {
                _workService.AddWork(userId, content, Private);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("user already reached maximum work count");
            }
            return Ok("Work has been added.");
        }

        [ProducesResponseType(typeof(List<Work>), StatusCodes.Status200OK)]
        [HttpGet("Works/{userId}")]
        public IActionResult GetUserWorks(long userId)
        {
            long CallerId;
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) CallerId = 0;
            else CallerId = long.Parse(userIdStr);
            return Ok(_workService.GetUserWorks(CallerId, userId));
        }
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("DeleteWork/{WorkId}")]
        public IActionResult DeleteWork(long WorkId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            long userId = long.Parse(userIdStr);
            try
            {
                _workService.DeleteWork(userId, WorkId);
                return Ok("work has been deleted");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("user dont own this work");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("there is no such work to delete");
            }
        }
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("WorkComplete/{WorkId}")]
        public IActionResult WorkComplete(long WorkId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            long userId = long.Parse(userIdStr);
            try
            {
                _workService.Done(userId, WorkId);
                return Ok("work has been completed");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("user dont own this work");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("there is no such work to complete");
            }
        }

    }
}
