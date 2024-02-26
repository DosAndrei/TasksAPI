using TasksAPI.Models;

namespace TasksAPI.Services
{
    public interface ITaskNoteCollectionService : ICollectionService<TaskNote>
    {
        Task<List<TaskNote>> GetTaskByStatus(string status);
    }
}
