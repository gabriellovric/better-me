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
        
        [HttpGet]
        public IActionResult Get([FromQuery]long? userId)
        {
            if (userId != null)
            {
                return Ok(_goalRepository.AllByUser(userId.Value));
            }

            return Ok(_goalRepository.All);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var goalItem = _goalRepository.Find(id);
            if (goalItem == null)
            {
                return NotFound(ErrorCode.ItemNotFound.ToString());
            }
            
            return Ok(goalItem);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Goal item)
        {
            try
            {
                if (item == null || !ModelState.IsValid || item.GoalId != 0 || item.User != null)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var email = User.FindFirstValue(ClaimTypes.Email);
                var userItem = _userRepository.Find(email);
                if (userItem == null)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
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

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]Goal item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }
                
                var existingItem = _goalRepository.Find(item.GoalId);
                if (existingItem == null || existingItem.User.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                _goalRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var item = _goalRepository.Find(id);
                if (item == null || item.User.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return NotFound(ErrorCode.DataProvidedIsInvalid.ToString());
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
