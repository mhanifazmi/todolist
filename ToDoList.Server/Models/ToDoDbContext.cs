using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Models
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
