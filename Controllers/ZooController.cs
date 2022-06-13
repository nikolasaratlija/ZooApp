using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleModelsAndRelations.Models;



namespace SimpleModelsAndRelations.Controllers
{
    [Route("[controller]")]
    public partial class ZooController : Controller
    {
        private readonly SimpleModelsAndRelationsContext _context;
        private readonly ProjectNameOptions _projectNameOptions;


        public ZooController(SimpleModelsAndRelationsContext context, IOptions<ProjectNameOptions> projectNameOptions)
        {
            _context = context;
            _projectNameOptions = projectNameOptions.Value;
            if (_context.Zoo.Count() == 0)
            {
                var mammal = new Specie() { Name = "mammal" };
                var bird = new Specie() { Name = "bird" };

                _context.Zoo.AddRange(new Zoo[]{
                    new Zoo(){Name="Blijdorp", Animals=new List<Animal>(){
                        new Animal(){Name="Armadillo", Age=5, Specie=mammal},
                        new Animal(){Name="Baboon", Age=15, Specie=mammal},
                        new Animal(){Name="Fox", Age=10, Specie=mammal},
                        new Animal(){Name="Tapir", Age=5, Specie=mammal},
                        new Animal(){Name="Gru", Age=2, Specie=bird},
                        new Animal(){Name="Black drongo", Age=5, Specie=bird},
                        new Animal(){Name="Blue-bellied roller", Age=9, Specie=bird},
                        new Animal(){Name="Bonin petrel", Age=5, Specie=bird}
                    }},
                    new Zoo(){Name="Artis", Animals=new List<Animal>(){
                        new Animal(){Name="Wolf", Age=7, Specie=mammal},
                        new Animal(){Name="Zebra", Age=4, Specie=mammal},
                        new Animal(){Name="Koala", Age=2, Specie=mammal},
                        new Animal(){Name="Jaguar", Age=6, Specie=mammal},
                        new Animal(){Name="Chukar partridge", Age=12, Specie=bird},
                        new Animal(){Name="Citrine wagtail", Age=14, Specie=bird},
                        new Animal(){Name="Grey wren", Age=9, Specie=bird},
                        new Animal(){Name="Green peafowl", Age=11, Specie=bird}
                    }}
                });
                _context.SaveChanges();
            }
        }

        [HttpGet("GetAllAnimals")]
        public IActionResult GetAllAnimals([FromQuery] string specie)
        {
            if (specie == null) specie = ""; 

            var animals_grouped = (
                from z in _context.Zoo
                from a in _context.Animals
                from s in _context.Species
                where z.Id == a.ZooId && a.SpecieId == s.Id
                select new { Zoo = z, Animal = a, Specie = s });

            if (specie != "") 
            {
                animals_grouped = (
                    from z in _context.Zoo
                    from a in _context.Animals
                    from s in _context.Species
                    where z.Id == a.ZooId && a.SpecieId == s.Id && s.Name == specie
                    select new { Zoo = z, Animal = a, Specie = s });
            }

            var animals_grouped_array = animals_grouped.ToArray();

            var animals_grouped_transformed = new Dictionary<int, Tuple<Zoo, List<Animal>>>();

            foreach (var item in animals_grouped_array)
            {
                item.Zoo.Animals = null;
                item.Animal.Zoo = null;
                item.Animal.Specie = null;
                item.Specie.Animals = null;
                if (!animals_grouped_transformed.ContainsKey(item.Zoo.Id))
                {
                    animals_grouped_transformed.Add(item.Zoo.Id, Tuple.Create(item.Zoo, new List<Animal>()));
                }
                animals_grouped_transformed[item.Zoo.Id].Item2.Add(item.Animal);
                item.Animal.Specie = item.Specie;
            }

            var res = animals_grouped_transformed.Select(x => new { Zoo = x.Value.Item1, Animals = x.Value.Item2.Select(a => new { Id = a.Id, Name = a.Name, Age = a.Age, Specie = a.Specie.Name }) });
            return Ok(res);
        }
    }
}
