using cwchat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cwchat.Server.Controllers
{
    public class ValuesController : ApiController
    {
        DB db = new DB();

        // GET api/values
        public IEnumerable<MessageModel> Get()
        {
            return db.Messages;
        }

        // POST api/values
        public void Post([FromBody]MessageModel value)
        {
            if(ModelState.IsValid)
            {
                value.Date = DateTime.Now;

                db.Messages.Add(value);
                db.SaveChanges();
            }
        }
    }
}
