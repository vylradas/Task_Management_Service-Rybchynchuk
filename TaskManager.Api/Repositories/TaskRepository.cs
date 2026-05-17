using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Api.Models;
using ModelsTaskStatus = TaskManager.Api.Models.TaskStatus;

namespace TaskManager.Api.Repositories
{
    public class TaskRepository
    {
        private readonly List<TaskItem> _tasks = new();

        public IEnumerable<TaskItem> GetAll() => _tasks;

        public TaskItem? GetById(Guid id) => _tasks.FirstOrDefault(t => t.Id == id);

        // ✅ Метод створення нового завдання (повертає створений елемент)
        public TaskItem Create(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            task.Status = ModelsTaskStatus.Backlog; // статус за замовчуванням
            _tasks.Add(task);
            return task;
        }

        // ✅ Метод оновлення статусу (із перевіркою допустимих переходів)
        public bool UpdateStatus(Guid id, ModelsTaskStatus newStatus)
        {
            var task = GetById(id);
            if (task == null)
                return false;

            if (!IsValidTransition(task.Status, newStatus))
                throw new InvalidOperationException($"Перехід із {task.Status} до {newStatus} заборонений");

            task.Status = newStatus;
            return true;
        }

        // ✅ Метод загального оновлення (залишаємо як був)
        public void Update(TaskItem task)
        {
            var index = _tasks.FindIndex(t => t.Id == task.Id);
            if (index >= 0)
            {
                _tasks[index] = task;
            }
        }

        // ✅ Внутрішній метод перевірки допустимих переходів статусу
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
