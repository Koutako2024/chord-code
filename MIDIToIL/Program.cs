using NAudio.Midi;
using System.Text;

namespace MIDIToIL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int lagToAllow = 10;

            // load MIDI file
            string filePath = @"C:\Users\kouta\Documents\projects\unfinished\ChordCode\MIDIToIL\お試し.mid";//GetMIDIFilePath();
            MidiFile midiFile = new(filePath, false);

            // filter note on events
            List<NoteOnEvent> noteOnEvents = midiFile.Events[0]
                .OfType<NoteOnEvent>()
                .Where(e => e.Velocity > 0)
                .ToList();

            //noteOnEvents.ForEach(e => Console.WriteLine(e.DeltaTime));

            List<List<string>> groupedNotes = GroupNoteOnEventsByLagTime(noteOnEvents, lagToAllow);

            // write IL to file
            string? inputParent = Directory.GetParent(filePath)?.FullName;
            if (inputParent is null) throw new Exception("Cannot get parent directory of input file path.");
            string outputFilePath = Path.Join(inputParent, "out.il");
            WriteIL(outputFilePath, groupedNotes);
        }

        static string AskMIDIFilePath()
        {
            string? filePath = null;
            while (filePath is null)
            {
                Console.WriteLine("Please input path for MIDI file.");
                filePath = Console.ReadLine();
            }
            return filePath;
        }

        static List<List<string>> GroupNoteOnEventsByLagTime(List<NoteOnEvent> noteOnEvents, int lagToAllow)
        {
            List<List<string>> groupedNotes = new();
            for (int i = 0, k; i < noteOnEvents.Count; i += k)
            {
                List<string> group = new();
                group.Add(noteOnEvents[i].NoteName);
                for (k = 1; i + k < noteOnEvents.Count && noteOnEvents[i + k].DeltaTime < lagToAllow; k++)
                {
                    group.Add(noteOnEvents[i + k].NoteName);
                }
                groupedNotes.Add(group);
            }
            return groupedNotes;
        }

        static void WriteIL(string OutputFilePath, List<List<string>> groupedNotes)
        {
            using (StreamWriter writer = new(OutputFilePath, false, Encoding.UTF8))
            {
                for (int i = 0; i < groupedNotes.Count; i++)
                {
                    string chord = string.Join("", groupedNotes[i][..^1]);
                    writer.WriteLine($"{chord}\t|{groupedNotes[i][^1]}");
                }
            }
        }
    }
}
