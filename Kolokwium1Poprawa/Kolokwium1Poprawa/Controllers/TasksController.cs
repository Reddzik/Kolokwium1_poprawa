using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kolokwium1Poprawa.Exceptions;
using Kolokwium1Poprawa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1Poprawa.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TasksController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{int}")]
        public IActionResult GetMemberBy(int id)
        {
            try { 
            var response = _dbService.GetTeamMemberBy(id);
                return Ok();
            }
            catch(MemberDoesntExistsException exc)
            {
                return BadRequest(exc.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                _dbService.RemoveProjectBy(id);
                return Ok();
            }catch(ProjectDoesntExistsException exc)
            {
                return NotFound(exc.Message);
            }
        }
    }
}