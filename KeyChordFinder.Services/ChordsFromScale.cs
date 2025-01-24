using KeyChordFinder.Data;
using KeyChordFinder.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace KeyChordFinder.Services
{
    public static class ChordsFromScalerr
    {

        static List<Chord> allChords = KeyChordFinderDbContext.GetChords();
        static List<List<IntervalChord>> intervalsOfChords = KeyChordFinderDbContext.GetIntervalChord();


        public static Dictionary<Note, List<Chord>> GetAllChordsInScale(Note root, int scaleId)
        {
            List<Note> notesInScale = GetNotesInScale(root, scaleId);
            List<IntervalScale> intervalsInScale = KeyChordFinderDbContext.GetIntervalScale(scaleId);

            Dictionary<Note, List<Chord>> noteChordsDictionary = new Dictionary<Note, List<Chord>>();
            for (int i = 0; i < notesInScale.Count; i++)
            {
                noteChordsDictionary.Add(notesInScale[i], AllChordsOnNote(intervalsInScale, intervalsInScale[i].Interval.IntervalValue));
            }

            return noteChordsDictionary;


        }


        static List<Chord> AllChordsOnNote(List<IntervalScale> scale, int indexOfStartNote)
        {
            List<Chord> chordsOnNote = new List<Chord>();

            foreach (List<IntervalChord> chord in intervalsOfChords)
            {
                if (IsChordOnNote(chord, scale, indexOfStartNote))
                {
                    chordsOnNote.Add(allChords.First(x => x.Id == chord[0].ChordId));
                }
            }
            return chordsOnNote;
        }

        static bool IsChordOnNote(List<IntervalChord> chord, List<IntervalScale> scale, int indexOfStartNote)
        {
            foreach (var interval in chord)
            {
                int targetValue = (interval.Interval.IntervalValue + indexOfStartNote) % 12;
                if (!scale.Any(s => s.Interval.IntervalValue == targetValue))
                {
                    return false;
                }
            }
            return true;
        }


        static List<Note> NotesFromIntervals(Note root, List<IntervalScale> intervalScales)
        {
            var octave = KeyChordFinderDbContext.GetOctave(root);
            
            var scale = new List<Note>();

            int index = octave.FindIndex(x => x.Id == root.Id);

            foreach (var interval in intervalScales)
            {
                scale.Add(octave[(index + interval.Interval.IntervalValue) % 12]);
            }

            return scale;
        }

        public static List<Note> GetNotesInScale(Note root, int scaleId)
        {
            List<IntervalScale> intervalScales = KeyChordFinderDbContext.GetIntervalScale(scaleId);
            return NotesFromIntervals(root, intervalScales);
        }

    }
}

