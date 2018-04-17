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
    [Route("api/achievements")]
    public class AchievementsController : Controller
    {
        private readonly GoalRepository _goalRepository;

        private readonly AchievementRepository _achievementRepository;

        public AchievementsController(GoalRepository goalRepository, AchievementRepository achievementRepository)
        {
            _goalRepository = goalRepository;
            _achievementRepository = achievementRepository;
        }
        
        [HttpGet]
        public IActionResult GetAll([FromQuery]long? userId, [FromQuery]long? goalId)
        {
            return Ok(_achievementRepository.AllWithQuery(userId, goalId));
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var achievementItem = _achievementRepository.Find(id);
            if (achievementItem == null)
            {
                return NotFound(ErrorCode.ItemNotFound.ToString());
            }
            
            return Ok(achievementItem);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Achievement item)
        {
            try
            {
                if (item == null
                 || !ModelState.IsValid
                 || item.Goal != null
                 || item.AchievementId != 0
                 || item.GoalId == 0
                 || item.Achieved < 0)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }
                
                var goalItem = _goalRepository.Find(item.GoalId);
                if (goalItem ==  null || goalItem.User.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                _achievementRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            
            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Achievement item)
        {
            try
            {
                var achievementItem = _achievementRepository.Find(id);
                if (achievementItem ==  null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                if (item == null
                 || !ModelState.IsValid
                 || item.Goal != null
                 || item.AchievementId != achievementItem.AchievementId
                 || item.GoalId != achievementItem.GoalId
                 || item.Achieved < 0)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var goalItem = _goalRepository.Find(item.GoalId);
                if (goalItem ==  null || goalItem.User.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                _achievementRepository.Update(item);
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
                var achievementItem = _achievementRepository.Find(id);
                if (achievementItem ==  null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                var goalItem = _goalRepository.Find(achievementItem.GoalId);
                if (goalItem ==  null || goalItem.User.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }
                
                _achievementRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            
            return NoContent();
        }
    }
}
