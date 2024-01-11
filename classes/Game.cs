using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnek.classes
{

    internal class Game
    {

        Board board = new Board();

        private List<string> directions = new List<string>() { "W", "A", "S", "D" };
        private string direction = "D";

        public Game()
        {

        }

        private Boolean notBackwards(string newDirection)
        {
            int currentIndex = this.directions.IndexOf(this.direction);
            int middleIndex = this.directions.Count / 2;

            if (currentIndex >= middleIndex)
            {
                if (this.directions[currentIndex - 2] == newDirection) { return false; }
            }

            if (currentIndex < middleIndex)
            {
                if (this.directions[currentIndex + 2] == newDirection) { return false; }
            }

            return true;
        }

        private void changeDirection(string newDirection)
        {

            // check if valid direction
            if (this.directions.Contains(newDirection))
            {
                // check if opposite direction
                if (notBackwards(newDirection))
                {
                    this.direction = newDirection;

                }
            }
        }

        private void clearBuffer()
        {
            // clear keyboard buffer
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        private void userInput()
        {
            // if key was pressed, checks if valid input; if yes, change direction, else does nothing
            if (Console.KeyAvailable)
            {
                string newDirection = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                changeDirection(newDirection);
            }

            clearBuffer();
        }

        public void run()
        {
            board.printBoard();
            Console.WriteLine("Press Enter to Start...");
            Console.ReadLine();

            while (true)
            {

                // snake speed, increase to slow down, d ecrease to speed up
                Thread.Sleep(1000);

                userInput();

                // move snake in currentDirection
                this.board.moveSnake(this.direction);

                // clear screen and print board again
                Console.Clear();
                this.board.printBoard();
            }
        }


    }
}
