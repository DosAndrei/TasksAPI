using MongoDB.Driver;
using TasksAPI.Models;
using TasksAPI.Settings;

namespace TasksAPI.Services
{
    public class TaskNoteCollectionService : ITaskNoteCollectionService
    {
        private readonly IMongoCollection<TaskNote> _announcements;

        public TaskNoteCollectionService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _announcements = database.GetCollection<TaskNote>(settings.TasksCollectionName);
        }

        public async Task<List<TaskNote>> GetAll()
        {
            var result = await _announcements.FindAsync(announcement => true);
            return result.ToList();
        }

        public async Task<bool> Create(TaskNote announcement)
        {
            announcement.Id = Guid.NewGuid();

            await _announcements.InsertOneAsync(announcement);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _announcements.DeleteOneAsync(announcement => announcement.Id == id);
            if (!result.IsAcknowledged && result.DeletedCount == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<TaskNote> Get(Guid id)
        {
            return (await _announcements.FindAsync(announcement => announcement.Id == id)).FirstOrDefault();
        }

        public async Task<bool> Update(Guid id, TaskNote announcement)
        {
            announcement.Id = id;
            var result = await _announcements.ReplaceOneAsync(announcement => announcement.Id == id, announcement);
            if (!result.IsAcknowledged && result.ModifiedCount == 0) {
                await _announcements.InsertOneAsync(announcement);
                return false;
            }
            
            return true;
        }

        public async Task<List<TaskNote>> GetTaskByStatus(string status)
        {
            return (await _announcements.FindAsync(announcement => announcement.Status.ToUpperInvariant() == status.ToUpperInvariant())).ToList();
        }

    }
}
