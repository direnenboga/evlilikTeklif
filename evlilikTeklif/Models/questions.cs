using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evlilikTeklif.Models
{
    public class questions
    {
        public int Id { get; set; }
        public int number { get; set; }
        public string question { get; set; }
        public bool isAnswered { get; set; }
        public string  answer { get; set; }
    }
}
