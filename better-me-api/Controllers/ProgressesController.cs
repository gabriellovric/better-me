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
    [Route("api/progresses")]
    public class ProgressesController : Controller
    {
        private readonly ProgressRepository _progressRepository;
        private readonly AssignmentRepository _assignmentRepository;
        private readonly UserRepository _userRepository;

        public ProgressesController(ProgressRepository progressRepository, AssignmentRepository assignmentRepository, UserRepository userRepository)
        {
            _progressRepository = progressRepository;
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
        }
        
        [HttpGet]
        public IActionResult GetAll([FromQuery]long? userId, [FromQuery]long? goalId, [FromQuery]long? assignmentId)
        {
            return Ok(_progressRepository.AllWithQuery(userId, goalId, assignmentId));
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var progressItem = _progressRepository.Find(id);
            if (progressItem == null)
            {
                return NotFound(ErrorCode.ItemNotFound);
            }
            
            return Ok(progressItem);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Progress item)
        {
            try
            {
                if (item == null
                 || !ModelState.IsValid
                 || item.Assignment != null
                 || item.ProgressId != 0
                 || item.AssignmentId == 0)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var assignmentItem = _assignmentRepository.Find(item.AssignmentId);
                if (assignmentItem == null)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var userItem = _userRepository.Find(assignmentItem.UserId);
                if(userItem == null || userItem.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                _progressRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            
            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Progress item)
        {
            try
            {
                var progressItem = _progressRepository.Find(id);
                if (progressItem == null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                if (item == null
                 || !ModelState.IsValid
                 || item.Assignment != null
                 || item.Assignment != null
                 || item.ProgressId != progressItem.ProgressId
                 || item.AssignmentId != progressItem.AssignmentId)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }
                
                var assignmentItem = _assignmentRepository.Find(item.AssignmentId);
                if (assignmentItem == null)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var userItem = _userRepository.Find(assignmentItem.UserId);
                if(userItem == null || userItem.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                _progressRepository.Update(item);
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
                var progressItem = _progressRepository.Find(id);
                if(progressItem == null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                var assignmentItem = _assignmentRepository.Find(progressItem.AssignmentId);
                if(assignmentItem == null)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var userItem = _userRepository.Find(assignmentItem.UserId);
                if(userItem == null || userItem.Email != User.FindFirstValue(ClaimTypes.Email) )
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                _progressRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
    }
}
