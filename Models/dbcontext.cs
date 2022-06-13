using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimpleModelsAndRelations.Models
{
    public class Zoo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Animal> Animals { get; set; }
    }
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public int ZooId { get; set; }
        public Zoo Zoo { get; set; }

        public int SpecieId { get; set; }
        public Specie Specie { get; set; }
    }

    public class Specie
    {
        public string Name {get;set;}
        public int Id { get; set; }
        public List<Animal> Animals { get; set; }
    }

    public partial class SimpleModelsAndRelationsContext : DbContext
    {
        public DbSet<Specie> Species { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Zoo> Zoo { get; set; }

        public SimpleModelsAndRelationsContext(DbContextOptions<SimpleModelsAndRelationsContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
