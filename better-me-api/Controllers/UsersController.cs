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

        // GET api/values
        [HttpGet]
        public IActionResult Get([FromQuery(), RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")] string email)
        {
            if (email != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(_userRepository.All.Where(user => user.Email == email));
            }

            return Ok(_userRepository.All);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var item = _userRepository.Find(id);
            if (item == null)
            {
                return NotFound(ErrorCode.RecordNotFound.ToString());
            }
            
            return Ok(item);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]User item)
        {
            try
            {
                if (item == null || !ModelState.IsValid || item.UserId != 0)
                {
                    return BadRequest(ErrorCode.ItemNameAndNotesRequired.ToString());
                }

                if (_userRepository.DoesItemExist(item.UserId) || _userRepository.DoesItemExist(item.Email))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ErrorCode.ItemIDInUse.ToString());
                }
                _userRepository.Insert(item);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            
            return Ok(item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] User item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest(ErrorCode.ItemNameAndNotesRequired.ToString());
                }
                var existingItem = _userRepository.Find(item.UserId);
                if (existingItem == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
                }
                _userRepository.Update(item);
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
                var item = _userRepository.Find(id);
                if (item == null)
                {
                    return NotFound(ErrorCode.RecordNotFound.ToString());
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
