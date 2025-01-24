using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyChordFinder.Data.Model
{
    public class Interval
    {
        [Key]
        public int Id { get; set; }
        public int IntervalValue { get; set; }
        public string Name { get; set; } = "";
    }
}
