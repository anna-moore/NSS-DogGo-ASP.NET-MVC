using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//could have the total walk time in this view model as a computed propertys
namespace DogGo.Models.ViewModels
{
    public class WalkerFormViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set;  }

        //public Neighborhood neighborhood { get; set; }
    }
}
