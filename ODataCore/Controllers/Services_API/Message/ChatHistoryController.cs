using Microsoft.AspNetCore.Mvc;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Nicepet_API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatHistoryController : ControllerBase
    {
        private ApiNicepetContext _db;

        public ChatHistoryController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }

      
        [AllowAnonymous]
        [HttpPost]
        public async Task <List<OutList>> PostAsync([FromBody] InList item)
        {
            // item contains Ids of ControllerPlanning and new time slots associated for DUPLICATION

            List<OutList> OutList = new List<OutList>();

            if (!ModelState.IsValid)
            {

                OutList outList = new OutList
                {
                    Error = "Ce compte n'existe pas !   "
                };
                OutList.Add(outList);
                return OutList;
            }


            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    //----------------------------Message----------------------------------------
                    OutList outList = new OutList();
                    List<Contacts> contactList = await _db.Contacts
                        //.Include(a => a.UserContact)
                        .Where(a => a.UserId == item.UserId)
                        .ToListAsync();

                     List<OutList> OutListIntermediate = new List<OutList>();

                    foreach (Contacts contectid in contactList)
                    {
                        Message LastMessage = await _db.Message.Include(a=>a.SenderUser).Include(a=>a.RecipientUser)
                        .Where(a => (a.SenderUserId == contectid.ContactId && a.RecipientUserId == item.UserId) ||
                         (a.RecipientUserId == contectid.ContactId && a.SenderUserId == item.UserId))
                        .OrderByDescending(a => a.Time).FirstOrDefaultAsync();


                        if (LastMessage != null)
                        {
                            int interlocutorId=0;
                            string interlocutorAvatar="";
                            string interlocutorFullName = "";
                            string since = "";
                            if (LastMessage.Time.AddHours(24) < DateTimeOffset.UtcNow)
                            {
                                since = LastMessage.Time.ToString().Substring(0, 10); 
                            }
                            else
                            {
                                int Hours = Convert.ToInt32( (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - LastMessage.Time.ToUnixTimeSeconds() )/3600 );
                                int Munits = Convert.ToInt32(((DateTimeOffset.UtcNow.ToUnixTimeSeconds() - LastMessage.Time.ToUnixTimeSeconds() ) % 3600 )/60);
                                since = Hours + "h " + Munits + "m Ago";
                            }

                            if (LastMessage.SenderUserId != item.UserId)
                            {
                                interlocutorId = LastMessage.SenderUserId;
                                interlocutorAvatar = LastMessage.SenderUser.Avatar;
                                interlocutorFullName = LastMessage.SenderUser.FirstName + " " + LastMessage.SenderUser.LastName;
                            }
                            else if (LastMessage.SenderUserId == item.UserId)
                            {
                                interlocutorId = LastMessage.RecipientUserId;
                                interlocutorAvatar = LastMessage.RecipientUser.Avatar;
                                interlocutorFullName = LastMessage.RecipientUser.FirstName + " " + LastMessage.RecipientUser.LastName;
                            }

                            outList = new OutList
                            {
                                Body = LastMessage.Body,
                                InterlocutorId = interlocutorId,
                                InterlocutorImage = interlocutorAvatar,
                                InterlocutorFullName = interlocutorFullName,
                                Created = LastMessage.Time,
                                Readed=LastMessage.Readed,
                                Succes="ok",
                                Since= since
                            };
                            OutListIntermediate.Add(outList);
                        }

                    }

                    if (OutListIntermediate.Count > 0)
                    {
                        OutList = OutListIntermediate.OrderByDescending(q => q.Created.Ticks).ToList();
                       // OutList.OrderByDescending(a => a.Time.Ticks);
                    }



                    transaction.Complete();

                    return OutList;
                }
            }
            catch (Exception ex)
            {
                 OutList outList = new OutList
                {
                    Error = ex.Message.ToString()
                };
                OutList.Add(outList);
                return OutList;
            }

        }


        public class InList
        {
            public int UserId  { get; set; }
        }


        public class OutList
        {
            public string Body { get; set; }
            public int InterlocutorId { get; set; }
            public string InterlocutorFullName { get; set; }
            public string InterlocutorImage { get; set; }
            public DateTimeOffset Created { get; set; }
            public bool? Readed { get; set; }
            public string Succes { get; set; }
            public string Error { get; set; }
            public string Since { get; set; }
        }



    }
}
