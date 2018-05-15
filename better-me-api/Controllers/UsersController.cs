using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BetterMeApi.Models;
using BetterMeApi.Repositories;
using System.Security.Claims;

namespace BetterMeApi.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /**
         * Liefer User mit E-Mail
         */
        [HttpGet]
        public IActionResult Get([FromQuery(), RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")] string email)
        {
            if (email != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                return Ok(_userRepository.Query(email));
            }

            return Ok(_userRepository.All);
        }

        /**
         * Liefert User nach ID
         */
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var item = _userRepository.Find(id);
            if (item == null)
            {
                return NotFound(ErrorCode.ItemNotFound.ToString());
            }
            
            return Ok(item);
        }

        /*
        [HttpPost]
        public IActionResult Post([FromBody]User item)
        {
            try
            {
                if (item == null || !ModelState.IsValid || item.UserId != 0)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                if (_userRepository.DoesItemExist(item.UserId) || _userRepository.DoesItemExist(item.Email))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.ItemAlreadyExists.ToString());
                }
                _userRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            
            return Ok(item);
        }
        */

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] User item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.DataProvidedIsInvalid.ToString());
                }

                var userItem = _userRepository.Find(item.UserId);
                if (userItem == null || userItem.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }

                _userRepository.Update(item);
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
                var item = _userRepository.Find(id);
                if (item == null || item.Email != User.FindFirstValue(ClaimTypes.Email))
                {
                    return NotFound(ErrorCode.ItemNotFound.ToString());
                }
                _userRepository.Delete(id);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
            }
            return NoContent();
        }
    }
}
