using NAudio.Midi;

namespace MIDIToIL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // input
            string filePath = @"C:\Users\kouta\Documents\projects\unfinished\ChordCode\MIDIToIL\お試し.mid";//AskMIDIFilePath();
            MidiFile midiFile = new(filePath, false);

            // output
            string? inputParent = Directory.GetParent(filePath)?.FullName;
            if (inputParent is null) throw new Exception("Cannot get parent directory of input file path.");
            string outputFilePath = Path.Join(inputParent, "out.il");

            //generate
            ILGenerator generator = new();
            generator.GenerateIL(midiFile, outputFilePath);
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
    }
}
