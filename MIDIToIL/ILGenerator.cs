using NAudio.Midi;
using System.Text;

namespace MIDIToIL
{
    public class ILGenerator
    {
        private int lagTickToAllow;

        public ILGenerator(int lagTickToAllow = 10)
        {
            this.lagTickToAllow = lagTickToAllow;
        }

        public void GenerateIL(MidiFile midiFile, string outputFilePath)
        {
            // filter note on events
            List<NoteOnEvent> noteOnEvents = midiFile.Events[0]
                .OfType<NoteOnEvent>()
                .Where(e => e.Velocity > 0)
                .ToList();

            List<List<string>> groupedNotes = GroupSameMomentEvents(noteOnEvents);

            List<List<List<string>>> structedNotes = MakeList(groupedNotes);

            WriteIL(outputFilePath, groupedNotes);
        }

        private List<List<string>> GroupSameMomentEvents(List<NoteOnEvent> noteOnEvents)
        {
            List<List<string>> groupedNotes = new();
            for (int i = 0, k; i < noteOnEvents.Count; i += k)
            {
                List<string> group = new();
                group.Add(noteOnEvents[i].NoteName);
                for (k = 1; i + k < noteOnEvents.Count && IsSameGroup(noteOnEvents[i + k - 1], noteOnEvents[i + k]); k++)
                {
                    group.Add(noteOnEvents[i + k].NoteName);
                }
                groupedNotes.Add(group);
            }
            return groupedNotes;
        }

        private List<List<List<string>>> MakeList(List<List<string>> groupedNotes)
        {
            List<List<List<string>>> structedNotes = new();

            int j;
            for (int i = 0; i < groupedNotes.Count; i += j)
            {
                //List<List<string>> currentStruct = new();


                for (j = 1; i + j < groupedNotes.Count; j++)
                {
                    if (groupedNotes[i + j].Count > 1)
                    {
                        break;
                    }
                }

                structedNotes.Add(groupedNotes[i..(i + j)]);
            }

            return structedNotes;
        }

        private bool IsSameGroup(NoteOnEvent a, NoteOnEvent b)
        {
            return b.AbsoluteTime - a.AbsoluteTime < lagTickToAllow;
        }

        private static void WriteIL(string OutputFilePath, List<List<string>> groupedNotes)
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
