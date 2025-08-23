using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIToIL
{
    internal class ILGenerator
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

            List<List<string>> groupedNotes = GroupNoteOnEvents(noteOnEvents);

            WriteIL(outputFilePath, groupedNotes);
        }

        private List<List<string>> GroupNoteOnEvents(List<NoteOnEvent> noteOnEvents)
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
