using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcmeSoft.Data.Models;
using AcmeSoft.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AcmeSoft.Controllers.API
{
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeMaintenanceRepository _repo;

        public EmployeeController(EmployeeMaintenanceRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var resp = await _repo.AllEmployees();
            return Ok(resp);
        }

        [HttpGet("people")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeopleAndEmployees()
        {
            var people = await _repo.AllPeopleAndEmployees();
            return Ok(people.Distinct().ToList());
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int employeeId)
        {
            var resp = await _repo.FindEmployee(employeeId);
            return Ok(resp);
        }

        [HttpPost]
        public async Task<ActionResult> UpsertEmployee([FromBody] Employee employee)
        {
            await _repo.UpsertEmployee(employee);
            return Ok();
        }

        [HttpDelete("{employeeId}")]
        public async Task<ActionResult> DeleteEmployee(int employeeId)
        {
            await _repo.DeleteEmployee(employeeId);
            return Ok();
        }
    }
}
