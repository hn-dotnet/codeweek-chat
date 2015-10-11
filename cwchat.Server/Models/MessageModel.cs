using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwchat.Server.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public string Sender { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public decimal Longitute { get; set; }

        public decimal Latitute { get; set; }
    }
}
