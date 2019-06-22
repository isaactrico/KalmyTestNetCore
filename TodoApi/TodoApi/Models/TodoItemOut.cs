using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class TodoItemOut
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }
    }
}
