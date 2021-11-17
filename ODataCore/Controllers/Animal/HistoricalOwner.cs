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
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Nicepet_API.Controllers
{
    //[AllowAnonymous]
    //[Authorize]
    [ODataRoutePrefix("HistoricalOwner")]
    [Produces("application/json")]
    public class HistoricalOwnerController : ODataController
    {
        private ApiNicepetContext _db;

        public HistoricalOwnerController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }
     
        [HttpGet]
        [ODataRoute]
        [EnableQuery]
       // [Authorize]
      //  [EnableQuery(MaxExpansionDepth = 0)]
        [ServiceFilter(typeof(PathFilter))]
        public IQueryable<HistoricalOwner> Get()
        {
            return _db.HistoricalOwner.AsQueryable();
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
        [EnableQuery(AllowedQueryOptions = Select)]
        public async Task<IActionResult> GetAsync(int key)
        {
            HistoricalOwner item = await _db.HistoricalOwner.FindAsync(key);
            if(item == null) 
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
        //[Authorize(Roles = "Admin")]
        [ODataRoute]
        public async  Task<IActionResult> PostAsync([FromBody] HistoricalOwner item) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            await _db.HistoricalOwner.AddAsync(item);
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
        //[Authorize(Roles = "Admin")]
        [ODataRoute("({key})")]
        [AcceptVerbs("PATCH")]
        public async Task<IActionResult> PatchAsync([FromODataUri] int key, [FromBody] Delta<HistoricalOwner> delta) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var item = await _db.HistoricalOwner.FindAsync(key);

            delta.Patch(item);

            _db.HistoricalOwner.Update(item);
            await _db.SaveChangesAsync();

            return Updated(item);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [ODataRoute("({key})")]
        public async Task<IActionResult> PutAsync([FromODataUri] int key, [FromBody] HistoricalOwner item) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(item.Id)) 
            {
                return BadRequest();
            }

            var updated = _db.HistoricalOwner.Update(item);
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
        //[Authorize(Roles = "Admin")]
        [ODataRoute("({key})")]
        public async Task<IActionResult> DeleteAsync(int key) 
        {
            var item = await _db.HistoricalOwner.FindAsync(key);

            if(item == null) 
            {
                return NotFound();
            }
            else 
            {
                _db.HistoricalOwner.Remove(item);
                await _db.SaveChangesAsync();

                return NoContent();
            }
        }
       
    }
}
