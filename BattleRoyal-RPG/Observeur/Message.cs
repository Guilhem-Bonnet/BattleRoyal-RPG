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
        }

        public Message AddSegment(string content, ConsoleColor color = ConsoleColor.White)
        {
            Segments.Add(new TextSegment(content, color));
            return this; // renvoie une référence à l'objet actuel
        }
    }

    public enum TypeMessage
    {
        Info,
        Log
    }
}
