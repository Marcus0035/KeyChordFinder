using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyChordFinder.Data.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Octave { get; set; }
        public string Pitch { get; set; } = "";
    }
}
