using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnek.classes
{
    internal class Snake
    {


        private List<string> heads = new List<string>() { "\u25B2", "\u25C4", "\u25BC", "\u25BA" };
        string bodypart = "\u25A0";

        public Snake() { }

        public string getBodypart() { return this.bodypart; } 

        /// <summary>
        /// Returns default snake as list for initial placement on game board.
        /// Default snake shape is: "——▶"
        public List<string> getSnake()
        {
            return new List<string>() { this.bodypart, this.bodypart, this.heads[3] };
        }

        public string getHead(string newDirection)
        {
            return newDirection switch
            {
                "W" => this.heads[0],
                "A" => this.heads[1],
                "S" => this.heads[2],
                "D" => this.heads[3],
                _ => "This code should never be reached.",
            };
        }



    }
}
