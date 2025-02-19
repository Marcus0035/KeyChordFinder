using KeyChordFinder.Data;
using KeyChordFinder.Data.Model;

namespace KeyChordFinder.Services
{
    public static class ChordsInScale
    {
        private static readonly List<Chord> AllChords = KeyChordFinderDbContext.GetChords();
        private static readonly List<List<IntervalChord>> IntervalsOfChords = KeyChordFinderDbContext.GetIntervalChord();


        public static Dictionary<Note, List<Chord>> GetAllChordsInScale(Note root, int scaleId)
        {
            var notesInScale = GetNotesInScale(root, scaleId);
            var intervalsInScale = KeyChordFinderDbContext.GetIntervalScale(scaleId);

            var noteChordsDictionary = new Dictionary<Note, List<Chord>>();
            for (var i = 0; i < notesInScale.Count; i++)
            {
                noteChordsDictionary.Add(notesInScale[i], AllChordsOnNote(intervalsInScale, intervalsInScale[i].Interval.IntervalValue));
            }

            return noteChordsDictionary;
        }


        private static List<Chord> AllChordsOnNote(List<IntervalScale> scale, int indexOfStartNote)
        {
            return (
                from chord in IntervalsOfChords 
                where IsChordOnNote(chord, scale, indexOfStartNote) 
                select AllChords.First(x => x.Id == chord[0].ChordId)).ToList();
        }

        private static bool IsChordOnNote(List<IntervalChord> chord, List<IntervalScale> scale, int indexOfStartNote)
        {
            return chord.Select(interval => (interval.Interval.IntervalValue + indexOfStartNote) % 12)
                        .All(targetValue => scale.Any(s => s.Interval.IntervalValue == targetValue));
        }


        private static List<Note> NotesFromIntervals(Note root, List<IntervalScale> intervalScales)
        {
            var octave = KeyChordFinderDbContext.GetOctave(root);
            
            var scale = new List<Note>();

            var index = octave.FindIndex(x => x.Id == root.Id);

            foreach (var interval in intervalScales)
            {
                scale.Add(octave[(index + interval.Interval.IntervalValue) % 12]);
            }

            return scale;
        }

        private static List<Note> GetNotesInScale(Note root, int scaleId)
        {
            var intervalScales = KeyChordFinderDbContext.GetIntervalScale(scaleId);
            return NotesFromIntervals(root, intervalScales);
        }

    }
}

