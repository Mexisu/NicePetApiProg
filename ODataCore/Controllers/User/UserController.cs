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
using BC = BCrypt.Net.BCrypt;

namespace Nicepet_API.Controllers
{
    //[Authorize]
    [ODataRoutePrefix("User")]
    [Produces("application/json")]
    public class UserController : ODataController
    {
        private ApiNicepetContext _db;

        
        public UserController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }

        [HttpGet]
        [ODataRoute]
        [EnableQuery]
        //  [EnableQuery(MaxExpansionDepth = 0)]
        [ServiceFilter(typeof(PathFilter))]
        public IQueryable<User> Get()
        {
            return _db.User.AsQueryable();
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
        //[AllowAnonymous]
        [EnableQuery(AllowedQueryOptions = Select)]
        public async Task<IActionResult> GetAsync(int key)
        {
            User item = await _db.User.FindAsync(key);
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
        public async Task<IActionResult> PostAsync([FromBody] User item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(item.Password != null)
            item.Password = BC.HashPassword(item.Password);
            item.CreationDate = DateTimeOffset.UtcNow;
            item.IsValidEmail = false;
            item.UserTypeId = 2;

            await _db.User.AddAsync(item);
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
        public async Task<IActionResult> PatchAsync([FromODataUri] int key, [FromBody] Delta<User> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _db.User.FindAsync(key);

            delta.Patch(item);

            _db.User.Update(item);
            await _db.SaveChangesAsync();

            return Updated(item);
        }

        [HttpPut]
        [ODataRoute("({key})")]
        public async Task<IActionResult> PutAsync([FromODataUri] int key, [FromBody] User item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(item.Id))
            {
                return BadRequest();
            }

            var updated = _db.User.Update(item);
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
            var item = await _db.User.FindAsync(key);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                _db.User.Remove(item);
                await _db.SaveChangesAsync();

                return NoContent();
            }
        }
        //[Route("api/[controller]")]
        //[ApiController]
        //public class UserController : ControllerBase
        //{
        //    private NicepetAPIContext _context;

        //    public UserController(NicepetAPIContext nicepetAPIContext)
        //    {
        //        _context = nicepetAPIContext;
        //    }
        //    // GET: api/Users
        //    [HttpGet]
        //    [EnableQuery()]
        //    public async Task<ActionResult<IEnumerable<User>>> GetAsync()
        //    {
        //        return await _context.User.ToListAsync();
        //    }

        //    // PUT: api/User/5
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //    // more details see https://aka.ms/RazorPagesCRUD.
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutAsync(int id, User user)
        //    {
        //        if (id != user.Id)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(user).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return NoContent();
        //    }

        //    // POST: api/User
        //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //    // more details see https://aka.ms/RazorPagesCRUD.
        //    [HttpPost]
        //    public async Task<ActionResult<User>> PostAsync(User user)
        //    {
        //        _context.User.Add(user);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetClient", new { id = user.Id }, user);
        //    }

        //    // DELETE: api/User/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult<User>> DeleteAsync(int id)
        //    {
        //        var user = await _context.User.FindAsync(id);
        //        if (user == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.User.Remove(user);
        //        await _context.SaveChangesAsync();

        //        return user;
        //    }

        //    private bool UserExists(int id)
        //    {
        //        return _context.User.Any(e => e.Id == id);
        //    }
        //}
    }
}
