using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StarChart.Data;

namespace StarChart.Controllers
{

    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id); //na ovj nacin proverue dali u databazu ima element ss takvo id
            if(celestialObject == null) //ako ne postoi takv objekt u nasu database
            {
                return NotFound();
            }
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList(); // Ovoj Where desan del od liniju e sigurno SQL Query
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name);
            if (!celestialObjects.Any())
            {
                return NotFound();
            }
            foreach(var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList(); // Ovoj Where desan del od liniju e sigurno SQL Query
            }
            
            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList(); // Ovoj Where desan del od liniju e sigurno SQL Query
            }
            return Ok(celestialObjects);
        }
    }
}
