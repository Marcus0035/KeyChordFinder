using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyChordFinder.Data.Model
{
    public class IntervalChord
    {
        public int Id { get; set; }

        public int IntervalId { get; set; }
        public Interval Interval { get; set; } = new();

        public int ChordId { get; set; }
        public Chord Chord { get; set; } = new();
    }
}