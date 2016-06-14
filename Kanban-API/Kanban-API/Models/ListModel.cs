using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kanban_API.Models
{
    public class ListModel
    {

        public int ListId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UserId { get; set; }

        public string CardsUrl
        {
            get
            {
                return $"http://localhost:50075/api/Lists/{ListId}/Cards";
            }

        }


    }
}