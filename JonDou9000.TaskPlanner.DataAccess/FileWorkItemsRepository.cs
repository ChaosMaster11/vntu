using Newtonsoft.Json;
using JonDou9000.TaskPlanner.Domain.Models;
using JonDou9000.TaskPlanner.DataAccess.Abstractions;

namespace JonDou9000.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FilePath = "work-items.json";
        private readonly Dictionary<Guid, WorkItem> _workItems = new Dictionary<Guid, WorkItem>();

        public FileWorkItemsRepository()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var items = JsonConvert.DeserializeObject<WorkItem[]>(json);
                foreach (var item in items)
                {
                    _workItems[item.Id] = item;
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            var newItem = workItem.Clone();
            newItem.Id = Guid.NewGuid();
            _workItems[newItem.Id] = newItem;
            SaveChanges();
            return newItem.Id;
        }

        public WorkItem Get(Guid id)
        {
            return _workItems.ContainsKey(id) ? _workItems[id].Clone() : null;
        }

        public WorkItem[] GetAll()
        {
            var items = new WorkItem[_workItems.Count];
            _workItems.Values.CopyTo(items, 0);
            return items;
        }

        public bool Update(WorkItem workItem)
        {
            if (_workItems.ContainsKey(workItem.Id))
            {
                _workItems[workItem.Id] = workItem.Clone();
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            if (_workItems.ContainsKey(id))
            {
                _workItems.Remove(id);
                SaveChanges();
                return true;
            }
            return false;
        }

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_workItems.Values);
            File.WriteAllText(FilePath, json);
        }
    }
}
