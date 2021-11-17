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
    [AllowAnonymous]
    //[Authorize]
    [ODataRoutePrefix("Message")]
    [Produces("application/json")]
    public class MessageController : ODataController
    {
        private ApiNicepetContext _db;

        public MessageController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }
     
        [HttpGet]
        [ODataRoute]
        [EnableQuery]
        //[Authorize]
        // [EnableQuery(MaxExpansionDepth = 0)]
        public IQueryable<Message> Get()
        {
            return _db.Message.AsQueryable();
        }

        /// <summary>
        /// Gets a single element.
        /// </summary>
        /// <param name="key">The requested element identifier.</param>
        /// <returns>The requested element.</returns>
        /// <response code="200">The element was successfully retrieved.</response>
        /// <response code="404">The element does not exist.</response>
        
        [HttpGet]
        //[ODataRoute("({key})")]
        [EnableQuery(AllowedQueryOptions = Select)]
        public async Task<IActionResult> GetAsync(int key)
        {
            Message item = await _db.Message.FindAsync(key);
            if(item == null) 
            {
                return NotFound();
            }
            else 
            {
                if(item.Readed != true)
                {
                    await PatchReaded(key);
                }
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
        public async  Task<IActionResult> PostAsync([FromBody] Message item) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            ChatHub chathubClass = new ChatHub();
            await _db.Message.AddAsync(item);
            //await chathubClass.SendMessage("hh", "hh", "jj");
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
        public async Task<IActionResult> PatchAsync([FromODataUri] int key, [FromBody] Delta<Message> delta) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var item = await _db.Message.FindAsync(key);

            delta.Patch(item);

            _db.Message.Update(item);
            await _db.SaveChangesAsync();

            return Updated(item);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [ODataRoute("({key})")]
        public async Task<IActionResult> PutAsync([FromODataUri] int key, [FromBody] Message item) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(item.Id)) 
            {
                return BadRequest();
            }

            var updated = _db.Message.Update(item);
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
            var item = await _db.Message.FindAsync(key);

            if(item == null) 
            {
                return NotFound();
            }
            else 
            {
                _db.Message.Remove(item);
                await _db.SaveChangesAsync();

                return NoContent();
            }
        }


        //---------------------------------------------- Patch Password -------------------------------------------
        public async Task<ActionResult> PatchReaded(int Id)
        {
            Delta<Message> deltaMessage = new Delta<Message>();
            MessageController messageController = new MessageController(_db);

            deltaMessage.TrySetPropertyValue("Readed", "true");
            await messageController.PatchAsync(Id, deltaMessage);

            return Ok(Id);
        }
    }
}
