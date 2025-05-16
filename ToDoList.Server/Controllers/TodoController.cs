using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Requests.TodoRequest;
using ToDoList.Helpers;
using Microsoft.EntityFrameworkCore;
using ToDoList.Services.Interfaces;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/to-do")]
    public class TodoController : ControllerBase
    {
        private readonly ToDoDbContext _context;
        private readonly ITodoService _service;

        public TodoController(ToDoDbContext context, ITodoService service)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DateTime? date = null)
        {
            var todos = await _service.GetTodosByDateAsync(date);
            return Ok(ResponseHelper.CreateSuccessResponse("Fetched all tasks", todos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var todo = await _service.GetTodoAsync(id);
            if (todo == null)
                return NotFound(ResponseHelper.CreateErrorResponse("Task not found"));

            return Ok(ResponseHelper.CreateSuccessResponse("Fetched task", todo));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.CreateErrorResponse("Validation failed", ModelState));

            var todo = await _service.CreateTodoAsync(request);
            return Ok(ResponseHelper.CreateSuccessResponse("Task created", todo));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditTodoRequest request)
        {
            var todo = await _service.UpdateTodoAsync(id, request);
            if (todo == null)
                return NotFound(ResponseHelper.CreateErrorResponse("Task not found"));

            return Ok(ResponseHelper.CreateSuccessResponse("Task updated", todo));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusTodoRequest request)
        {
            var todo = await _service.UpdateStatusAsync(id, request);
            if (todo == null)
                return NotFound(ResponseHelper.CreateErrorResponse("Task not found"));

            return Ok(ResponseHelper.CreateSuccessResponse("Status updated", todo));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteTodoAsync(id);
            if (!deleted)
                return NotFound(ResponseHelper.CreateErrorResponse("Task not found"));

            return Ok(ResponseHelper.CreateSuccessResponse("Task deleted"));
        }

        [HttpPost("copy")]
        public async Task<IActionResult> CopyTasks([FromBody] CopyTodoTasksRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseHelper.CreateErrorResponse("Validation failed", ModelState));

            var copiedTasks = await _service.CopyTasksAsync(request);
            return Ok(ResponseHelper.CreateSuccessResponse($"Tasks copied from {request.FromDate:yyyy-MM-dd} to {request.ToDate:yyyy-MM-dd}", copiedTasks));
        }

        [HttpPost("update-sequence")]
        public async Task<IActionResult> UpdateSequence([FromBody] ReorderTodoRequest request)
        {
            if (request.Ids == null || !request.Ids.Any())
                return BadRequest(ResponseHelper.CreateErrorResponse("Invalid data"));

            var todo = await _service.ReorderTodosAsync(request);

            return Ok(ResponseHelper.CreateSuccessResponse("Sequence updated"));
        }
    }
}
