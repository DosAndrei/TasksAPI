using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        ITaskNoteCollectionService _tasksCollectionService;

        public TasksController(ITaskNoteCollectionService tasksCollectionService)
        {
            _tasksCollectionService = tasksCollectionService ?? throw new ArgumentNullException(nameof(TaskNoteCollectionService));
        }


        /// <summary>
        /// GET Method summary
        /// </summary>
        /// <returns>OK</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await _tasksCollectionService.GetAll();

            return Ok(tasks);
        }

        /// <summary>
        /// GET Method summary
        /// </summary>
        /// <returns>OK</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var taskNote = await _tasksCollectionService.Get(id);

            return taskNote != null ? Ok(taskNote) : NotFound();
        }

        /// <summary>
        /// POST Method summary
        /// </summary>
        /// <returns>OK</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskNote taskNoteToAdd)
        {
            if (taskNoteToAdd == null)
            {
                return BadRequest("Empty model");
            }

            var created = await _tasksCollectionService.Create(taskNoteToAdd);
            return created ? Ok("Task created") : BadRequest("Could not create Task!");
        }

        /// <summary>
        /// PUT Method summary
        /// </summary>
        /// <returns>OK</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] TaskNote taskToUpdate)
        {
            var updated = await _tasksCollectionService.Update(id, taskToUpdate);

            if (updated)
            {
                return Ok("Updated");
            }

            return BadRequest("Could not update");
        }

        /// <summary>
        /// DELETE Method summary
        /// </summary>
        /// <returns>OK</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var removed = await _tasksCollectionService.Delete(id);

            return removed ? Ok("Task removed!") : NotFound("Could not find element to remove.");
        }

    }
}
