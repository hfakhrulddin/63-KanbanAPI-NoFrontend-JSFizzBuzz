using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanban_API.Models
{
    public class CardModel
    {
        public int CardId { get; set; }
        public int ListID { get; set; }
        public System.DateTime CreatDate { get; set; }
        public string Text { get; set; }
        
    }
}