using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetterMeApi.Models;
using BetterMeApi.Repositories;

namespace BetterMeApi.Controllers
{
    [Authorize]
    [Route("api/goals")]
    public class GoalsController : Controller
    {
        private readonly GoalRepository _goalRepository;
        private readonly UserRepository _userRepository;

        public GoalsController(GoalRepository goalRepository, UserRepository userRepository)
        {
            _goalRepository = goalRepository;
            _userRepository = userRepository;
        }
        
        // GET api/values
        [HttpGet]
        public IActionResult GetByUserId([FromQuery]long? userId)
        {
            if (userId != null)
            {
                return Ok(_goalRepository.All.Where(goal => goal.User.UserId == userId));
            }

            return Ok(_goalRepository.All);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var goalItem = _goalRepository.Find(id);
            if (goalItem == null)
            {
                return NotFound("RecordNotFound");
            }
            
            return Ok(goalItem);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Goal item)
        {
            try
            {
                if (item == null || !ModelState.IsValid || item.GoalId != 0 || item.User != null)
                {
                    return BadRequest(ErrorCode.ItemNameAndNotesRequired.ToString());
                }
                

                var email = User.FindFirst(ClaimTypes.Email).Value;
                var userItem = _userRepository.All.Where(user => user.Email == email).SingleOrDefault();
                if (userItem == null)
                {
                    return BadRequest(ErrorCode.ItemNameAndNotesRequired.ToString());
                }

                item.UserId = userItem.UserId;

                _goalRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            
            return Ok(item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody]Goal item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.ItemNameAndNotesRequired.ToString());
                }
                
                var existingItem = _goalRepository.Find(item.GoalId);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _goalRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var item = _goalRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _goalRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
    }
}
