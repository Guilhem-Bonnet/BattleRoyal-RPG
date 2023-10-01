using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Observeur
{
    public class TextSegment
    {
        public string Content { get; }
        public ConsoleColor Color { get; }

        public TextSegment(string content, ConsoleColor color = ConsoleColor.White)
        {
            Content = content;
            Color = color;
        }
    }

    public class Message
    {
        public List<TextSegment> Segments { get; } = new List<TextSegment>();
        public TypeMessage Type { get; }

        public Message(TypeMessage type = TypeMessage.Info)
        {
            Type = type;
            // Obtenez le temps actuel et formatez-le
            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            AddSegment($"[{currentTime}] "); // Affichez le temps préfixé avant le message
        }


        public Message AddSegment(string content, ConsoleColor color = ConsoleColor.White)
        {
            Segments.Add(new TextSegment(content, color));
            return this; // renvoie une référence à l'objet actuel
        }
        public void Clear()
        {
            Segments.Clear(); // Vide la liste de segments
        }

        public void AddSegmentsFrom(Message otherMessage)
        {
            Segments.AddRange(otherMessage.Segments);
        }
    }

    public enum TypeMessage
    {
        Info,
        Log
    }
}
