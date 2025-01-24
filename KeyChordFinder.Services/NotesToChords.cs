using KeyChordFinder.Data;
using KeyChordFinder.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyChordFinder.Services
{
    public class NotesToChords
    {
        //Dont Change Them
        static List<Interval> allIntervals = KeyChordFinderDbContext.GetIntervals();
        static List<Chord> allChords = KeyChordFinderDbContext.GetChords();
        public static (Note?, Chord?) GetChordsFromNotes(List<int> notesIds)
        {
            List<Note> selectedNotes = KeyChordFinderDbContext.GetNotes(notesIds).OrderBy(x => x.Pitch).ToList();


            List<int> intervalsInt = new();

            foreach (Note note in selectedNotes)
            {
                intervalsInt.Add(CalculateIntervalFromRoot(selectedNotes[0], note));
            }

            List<Interval> selectedIntervals = allIntervals.Where(x => intervalsInt.Contains(x.IntervalValue)).ToList();

            foreach (Interval interval in selectedIntervals)
            {
                int rootIndex = selectedIntervals.IndexOf(interval);
                List<Interval> pomocnyListInterval = MoveIntervalsToZero(selectedIntervals, rootIndex);

                foreach (Chord chord in allChords)
                {
                    if (AreAllIntervalsExactlyTheSame(chord, pomocnyListInterval))
                    {
                        //problem
                        //vraci to vzdy tu nejnizsi v aktualnim scale nevim jak to nazvat
                        //nejak for loop a pomoci i tam dat tu realnou
                        int intervalOfLowestNote = pomocnyListInterval.Min(x => x.IntervalValue);
                        return (selectedNotes[rootIndex], chord);
                    }
                }
            }

            return (null, null);
        }

        private static int CalculateIntervalFromRoot(Note root, Note interval)
        {
            double factor = 12 / Math.Log(2);
            double rootPitch = double.Parse(root.Pitch);
            double intervalPitch = double.Parse(interval.Pitch);
            return (int)Math.Round(factor * Math.Log(intervalPitch / rootPitch));
        }

        public static bool AreAllIntervalsExactlyTheSame(Chord chord, List<Interval> intervals)
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
            List<Interval> result = new List<Interval>();
            int min = intervals[fromIndex].IntervalValue;


            foreach (Interval interval in intervals)
            {
                int pomocnaValue = (interval.IntervalValue - min);

                if (pomocnaValue < 0)
                {
                    pomocnaValue = 12 + pomocnaValue;
                }

                result.Add(allIntervals[pomocnaValue]);
            }

            return result.OrderBy(x => x.IntervalValue).ToList();
        }
    }
}

