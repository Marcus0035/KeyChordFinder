using System.Globalization;
using KeyChordFinder.Data;
using KeyChordFinder.Data.Model;

namespace KeyChordFinder.Services
{
    public static class NotesToChords
    {
        private static readonly List<Interval> AllIntervals = KeyChordFinderDbContext.GetIntervals();
        private static readonly List<Chord> AllChords = KeyChordFinderDbContext.GetChords();
        public static (Note?, Chord?) GetChordsFromNotes(List<int> notesIds)
        {
            var selectedNotes = KeyChordFinderDbContext.GetNotes(notesIds).OrderBy(x => x.Pitch).ToList();
            
            List<int> intervalsInt = [];
            intervalsInt.AddRange(selectedNotes.Select(note => CalculateIntervalFromRoot(selectedNotes[0], note)));
            
            var selectedIntervals = AllIntervals.Where(x => intervalsInt.Contains(x.IntervalValue)).ToList();

            foreach (var interval in selectedIntervals)
            {
                var rootIndex = selectedIntervals.IndexOf(interval);
                var intervalsFromZero = MoveIntervalsToZero(selectedIntervals, rootIndex);

                foreach (var chord in AllChords.Where(chord => AreAllIntervalsExactlyTheSame(chord, intervalsFromZero)))
                {
                    return (selectedNotes[rootIndex], chord);
                }
            }
            return (null, null);
        }

        private static int CalculateIntervalFromRoot(Note root, Note interval)
        {
            //culture because 42.22 != 4222
            var rootPitch = double.Parse(root.Pitch, CultureInfo.InvariantCulture);
            var intervalPitch = double.Parse(interval.Pitch, CultureInfo.InvariantCulture);

            return (int)Math.Round(12 * Math.Log(intervalPitch / rootPitch, 2));
        }


        private static bool AreAllIntervalsExactlyTheSame(Chord chord, List<Interval> intervals)
        {
            var chordIntervalValues = chord.Intervals
                .Select(ic => ic.Interval.IntervalValue)
                .ToList();

            var providedIntervalValues = intervals
                .Select(i => i.IntervalValue)
                .ToList();

            chordIntervalValues.Sort();
            providedIntervalValues.Sort();

            return chordIntervalValues.SequenceEqual(providedIntervalValues);
        }




        private static List<Interval> MoveIntervalsToZero(List<Interval> intervals, int fromIndex)
        {
            var result = new List<Interval>();
            var min = intervals[fromIndex].IntervalValue;


            foreach (var interval in intervals)
            {
                var newIndex = (interval.IntervalValue - min);

                if (newIndex < 0)
                {
                    newIndex = 12 + newIndex;
                }

                result.Add(AllIntervals[newIndex]);
            }

            return result.OrderBy(x => x.IntervalValue).ToList();
        }
    }
}

