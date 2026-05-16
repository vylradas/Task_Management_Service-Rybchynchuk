using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;
using ModelsTaskStatus = TaskManager.Api.Models.TaskStatus;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskRepository _repository;

        public TasksController(TaskRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll() => Ok(_repository.GetAll());

        [HttpGet("{id}")]
        public ActionResult<TaskItem> Get(Guid id)
        {
            var task = _repository.GetById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItem> Create(TaskItem task)
        {
            var created = _repository.Create(task);
            Console.WriteLine("Creating task (MASTER VERSION)");
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPatch("{id}/status")]
        public ActionResult<TaskItem> UpdateStatus(Guid id, [FromQuery] ModelsTaskStatus newStatus)
        {
            var task = _repository.GetById(id);
            if (task == null) return NotFound();

            if (!IsValidTransition(task.Status, newStatus))
                return BadRequest($"Invalid status transition from {task.Status} to {newStatus}");

            task.Status = (TaskManager.Api.Models.TaskStatus)newStatus;
            _repository.Update(task);

            return Ok(task);
        }

        private bool IsValidTransition(ModelsTaskStatus current, ModelsTaskStatus next)
        {
            return (current, next) switch
            {
                (ModelsTaskStatus.Backlog, ModelsTaskStatus.InWork) => true,
                (ModelsTaskStatus.InWork, ModelsTaskStatus.Testing) => true,
                (ModelsTaskStatus.Testing, ModelsTaskStatus.Done) => true,
                _ => false
            };
        }

    }
}
