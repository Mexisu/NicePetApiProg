using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;

namespace Nicepet_API.Controllers
{
    //[Authorize]
    [ODataRoutePrefix("UserProfile")]
    [Produces("application/json")]
    public class UserProfileController : ODataController
    {
        private ApiNicepetContext _db;

        public UserProfileController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }

        [HttpGet]
        [ODataRoute]
        [EnableQuery]
        //  [EnableQuery(MaxExpansionDepth = 0)]
        [ServiceFilter(typeof(PathFilter))]
        public IQueryable<UserProfile> Get()
        {
            return _db.UserProfile.AsQueryable();
        }

        /// <summary>
        /// Gets a single element.
        /// </summary>
        /// <param name="key">The requested element identifier.</param>
        /// <returns>The requested element.</returns>
        /// <response code="200">The element was successfully retrieved.</response>
        /// <response code="404">The element does not exist.</response>

        [HttpGet]
        [ODataRoute("({key})")]
        //[Authorize(Roles = "Admin")]
        [EnableQuery(AllowedQueryOptions = Select)]
        public async Task<IActionResult> GetAsync(int key)
        {
            UserProfile item = await _db.UserProfile.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(item);
            }
        }

        /// <summary>
        /// Create a new element
        /// </summary>
        /// <param name="element">The element to place.</param>
        /// <returns>The created element.</returns>
        /// <response code="201">The element was successfully placed.</response>
        /// <response code="400">The element is invalid.</response>
        /// 
        [HttpPost]
        [ODataRoute]
        public async Task<IActionResult> PostAsync([FromBody] UserProfile item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _db.UserProfile.AddAsync(item);
            await _db.SaveChangesAsync();

            return Created(item);
        }
        /// <summary>
        /// Updates an existing element.
        /// </summary>
        /// <param name="key">The requested element identifier.</param>
        /// <param name="delta">The partial element to update.</param>
        /// <returns>The modified element.</returns>
        /// <response code="204">The element was successfully updated.</response>
        /// <response code="404">The element does not exist.</response>
        [HttpPatch]
        [ODataRoute("({key})")]
        [AcceptVerbs("PATCH")]
        public async Task<IActionResult> PatchAsync([FromODataUri] int key, [FromBody] Delta<UserProfile> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _db.UserProfile.FindAsync(key);

            delta.Patch(item);

            _db.UserProfile.Update(item);
            await _db.SaveChangesAsync();

            return Updated(item);
        }

        [HttpPut]
        [ODataRoute("({key})")]
        public async Task<IActionResult> PutAsync([FromODataUri] int key, [FromBody] UserProfile item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(item.Id))
            {
                return BadRequest();
            }

            var updated = _db.UserProfile.Update(item);
            await _db.SaveChangesAsync();

            return Updated(updated);
        }

        /// <summary>
        /// Cancels an element
        /// </summary>
        /// <param name="key">The element to cancel.</param>
        /// <returns>None</returns>
        /// <response code="204">The element was successfully canceled.</response>

        [HttpDelete]
        [ODataRoute("({key})")]
        public async Task<IActionResult> DeleteAsync(int key)
        {
            var item = await _db.UserProfile.FindAsync(key);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                _db.UserProfile.Remove(item);
                await _db.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}
