using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonCrud.Entites;

namespace PersonCrud.Models
{
    public class PersonsViewModel
    {
        public IEnumerable<Person> Persons { get; set; } 
    }
}