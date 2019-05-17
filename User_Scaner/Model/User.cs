using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Scaner.Model
{
    public class User
    {
        public string hostName { get; set; }
        public string nazwa { get; set; }

        public override string ToString()
        {
            return $"{hostName}" + " - " + $"{nazwa}";
        }
    }
}
