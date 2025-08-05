using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ef2.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public ICollection<Register> Registers { get; set; }
    }
}
