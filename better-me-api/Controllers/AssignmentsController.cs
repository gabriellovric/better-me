﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetterMeApi.Models;
using BetterMeApi.Repositories;
using System.ComponentModel.DataAnnotations;

namespace BetterMeApi.Controllers
{
    [Authorize]
    [Route("api/assignments")]
    public class AssignmentsController : Controller
    {
        private readonly AssignmentRepository _assignmentRepository;

        private readonly GoalRepository _goalRepository;

        private readonly UserRepository _userRepository;

        public AssignmentsController(AssignmentRepository assignmentRepository, GoalRepository goalRepository, UserRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _goalRepository = goalRepository;
            _userRepository = userRepository;
        }
        
        /**
         * Liefert Assignments für User und Goal
         */
        [HttpGet]
        public IActionResult Get([FromQuery]long? userId, [FromQuery]long? goalId)
        {
            if (userId != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                return Ok(_assignmentRepository.AllByUser(userId.Value));
            }

            if (goalId != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                return Ok(_assignmentRepository.AllByGoal(goalId.Value));
            }

            return Ok(_assignmentRepository.All);
        }

        /**
         * Liefer ein spezifisches Assignment
         */
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var assignmentItem = _assignmentRepository.Find(id);
            if (assignmentItem == null)
            {
                return NotFound(ErrorCode.ItemNotFound.ToString());
            }
            
            return Ok(assignmentItem);
        }

        /**
         * Speichert Assignment
         */
        [HttpPost]
        public IActionResult Post([FromBody]Assignment item)
        {
            try
            {
                if (item == null
                 || !ModelState.IsValid 
                 || item.AssignmentId != 0
                 || item.Goal != null
                 || item.GoalId == 0
                 || item.User != null
                 || item.UserId != 0)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }
                
                if(!_goalRepository.DoesItemExist(item.GoalId))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var userItem = _userRepository.Find(User.FindFirstValue(ClaimTypes.Email));
                if(userItem == null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                item.UserId = userItem.UserId;

                _assignmentRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            
            return Ok(item);
        }

        /**
         * Speichert Assignment
         */
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Assignment item)
        {
            try
            {
                var assignmentItem = _assignmentRepository.Find(id);
                if(assignmentItem == null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                if (item == null
                 || !ModelState.IsValid
                 || item.User != null
                 || item.Goal != null
                 || item.UserId != assignmentItem.UserId
                 || item.GoalId != assignmentItem.GoalId)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var userItem = _userRepository.Find(item.UserId);
                if(userItem == null || userItem.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                _assignmentRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
            }
            return NoContent();
        }

        /**
         * Löscht Assignment
         */
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var assignmentItem = _assignmentRepository.Find(id);
                if(assignmentItem == null)
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                var userItem = _userRepository.Find(assignmentItem.UserId);
                if(userItem == null || userItem.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                _assignmentRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            
            return NoContent();
        }
    }
}
