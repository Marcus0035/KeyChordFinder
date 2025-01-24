using KeyChordFinder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KeyChordFinder.Data
{
    public class KeyChordFinderDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Scale> Scales { get; set; }
        public DbSet<Interval> Intervals { get; set; }
        public DbSet<IntervalScale> IntervalScale { get; set; }
        public DbSet<Chord> Chords { get; set; }
        public DbSet<IntervalChord> IntervalChord { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\Marek\source\repos\MudBlazorKeyChordFinder\KeyChordFinder.Data\KeyChordFinder.db");
        }

        public static List<Note> GetOctave()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Notes.Take(12).ToList();
            }
        }
        public static List<Note> GetOctave(Note root)
        {
            using (var context = new KeyChordFinderDbContext())
            {
                List<Note> octave = new List<Note>();
                List<Note> notes = context.Notes.ToList();
                int index = notes.FindIndex(x => x.Id == root.Id);

                for (int i = 0; i < 12; i++)
                {
                    octave.Add(notes[(index + i) % notes.Count]);
                }

                return octave;
            }
        }

        public static List<int> GetOctaveNumbers()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Notes.Select(x => x.Octave).Distinct().ToList();
            }
        }



        public static List<Note> GetNotes()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Notes.ToList();
            }
        }

        public static List<Note> GetNotes(List<int> ids)
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Notes.Where(x => ids.Contains(x.Id)).ToList();
            }
        }

        public static List<Scale> GetScales()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Scales.ToList();
            }
        }

        public static List<Interval> GetIntervals()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Intervals.ToList();
            }
        }

        public static List<List<IntervalChord>> GetIntervalChord()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.IntervalChord.Include(x => x.Chord).Include(x => x.Interval).GroupBy(x => x.ChordId).Select(g => g.ToList()).ToList();
            }
        }

        public static List<Chord> GetChords()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.Chords.Include(x => x.Intervals).ThenInclude(x => x.Interval).ToList();
            }
        }

        public static Dictionary<Chord, List<IntervalChord>> GetChordsWithIntervals()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.IntervalChord.Include(x => x.Chord).Include(x => x.Interval).GroupBy(x => x.ChordId).ToDictionary(x => x.First().Chord, x => x.ToList());
            }
        }

        public static List<IntervalScale> GetIntervalScale()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.IntervalScale.Include(x => x.Scale).Include(x => x.Interval).ToList();
            }
        }

        public static List<IntervalScale> GetIntervalScale(int scaleId)
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.IntervalScale.Include(x => x.Scale).Include(x => x.Interval).Where(x => x.ScaleId == scaleId).ToList();
            }
        }

        public static List<List<IntervalChord>> GetIntervalChords()
        {
            using (var context = new KeyChordFinderDbContext())
            {
                return context.IntervalChord
                    .Include(x => x.Chord)
                    .Include(x => x.Interval)
                    .GroupBy(x => x.ChordId)
                    .Select(g => g.ToList())
                    .ToList();
            }
        }

    }
}
