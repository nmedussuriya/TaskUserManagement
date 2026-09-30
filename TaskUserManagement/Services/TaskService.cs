using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskUserManagement.Data;
using TaskUserManagement.DTOs;
using TaskUserManagement.Models;
using TaskUserManagement.Services;

namespace TaskUserManagement.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TaskDto>> GetAllTasks()
        {
            var tasks = await _context.Tasks
                .ToListAsync();

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Completed = task.Completed,
                UserId = task.UserId
            }).ToList();
        }
        public async Task<TaskDto?> GetTaskById(int id)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return null;

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Completed = task.Completed,
                UserId = task.UserId
            };
        }

        public async Task<TaskItem> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskItem?> UpdateTask(int id, TaskItem updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return null;

            task.Title = updatedTask.Title;
            task.Completed = updatedTask.Completed;

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }


    }
}
