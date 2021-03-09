using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FindProperty.Models;

namespace FindProperty.Data
{
    public class FindProperty1Context : DbContext
    {
        public FindProperty1Context (DbContextOptions<FindProperty1Context> options)
            : base(options)
        {
        }

        public DbSet<FindProperty.Models.Property> Property { get; set; }
        public DbSet<FindProperty.Models.Appointment> Appointment { get; set; }
    }
}
