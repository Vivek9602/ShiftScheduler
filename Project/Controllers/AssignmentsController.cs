using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly ShiftDbContext _context;

        public AssignmentsController(ShiftDbContext context)
        {
            _context = context;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
          if (_context.Assignments == null)
          {
              return NotFound();
          }

          List<Assignment> list = await _context.Assignments.Where(e => !((e.End <= start) || (e.Start >= end))).ToListAsync();

          List<ShiftAssignment> result = new List<ShiftAssignment>();
          foreach (var assignment in list)
          {
              var a1 = new ShiftAssignment
              {
                  Id = assignment.Id,
                  Start = assignment.Start,
                  End = assignment.End,
                  EmployeeId = assignment.EmployeeId,
                  LocationId = assignment.LocationId
              };
              result.Add(a1);
          }

          return Ok(result);

        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
            if (_context.Assignments == null)
            {
              return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        // PUT: api/Assignments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(int id, AssignmentUpdateParams p)
        {

            Assignment? assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return Problem("Assignment not found " + id);
            }

            if (p.EmployeeId != null)
            {
                Employee? person = await _context.Employees.FindAsync(p.EmployeeId);
                if (person == null)
                {
                    return Problem("Person not found " + p.EmployeeId);
                }

                assignment.Employee = person;
            }

            if (p.Start != null && p.End != null)
            {
                assignment.Start = (DateTime) p.Start;
                assignment.End = (DateTime) p.End;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Assignments
        [HttpPost]
        public async Task<ActionResult<Assignment>> PostAssignment(NewAssignmentParams na)
        {
            if (_context.Assignments == null)
            {
              return Problem("Entity set 'ShiftDbContext.Assignments'  is null.");
            }

            var locationEntity = await _context.Locations.FindAsync(na.LocationId);
            if (locationEntity == null)
            {
                return Problem("Location not found " + na.LocationId);
            }

            var personEntity = await _context.Employees.FindAsync(na.EmployeeId);
            if (personEntity == null)
            {
                return Problem("Person not found " + na.EmployeeId);
            }

            var assignment = new Assignment
            {
              Start = na.Start,
              End = na.End,
              Location = locationEntity,
              Employee = personEntity
            };

            _context.Assignments.Add(assignment);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssignment", new { id = assignment.Id }, assignment);
        }

        // DELETE: api/Assignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            if (_context.Assignments == null)
            {
                return NotFound();
            }
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssignmentExists(int id)
        {
            return (_context.Assignments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }



    public class NewAssignmentParams
    {
        public int EmployeeId { get; set; }
        public int LocationId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }


    public class ShiftAssignment
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int EmployeeId { get; set; }
        public int LocationId { get; set; }
    }


    public class AssignmentUpdateParams
    {
        public int? EmployeeId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
