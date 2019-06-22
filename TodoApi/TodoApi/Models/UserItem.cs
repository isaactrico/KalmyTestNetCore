using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class UserItem
    {
        [Key]
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
