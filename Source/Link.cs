using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    public class Link
    {
        public int Id { get; init; }
        public string Text { get; set; }
        
        public Link(int id, string link) {
            Id = id;
            Text = link;
        }
    }
}
