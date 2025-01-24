using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyChordFinder.Data.Model
{
    public class IntervalScale
    {
        [Key]
        public int Id { get; set; }

        public int IntervalId { get; set; }
        public Interval Interval { get; set; } = new();

        public int ScaleId { get; set; }
        public Scale Scale { get; set; } = new();
    }
}