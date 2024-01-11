using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnek.classes
{
    internal class Board
    {


        public Dictionary<string, List<string>> board = new Dictionary<string, List<string>>();
        private List<string> letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        // defining board size, should take all values up to 24 and infinite, respectively
        private int colNum = 16;
        private int rowNum = 16;

        // variables for correct printing
        private int rowSizeOffset = 12;
        private string border = " ";

        // snake-relevant variables
        Snake snake = new Snake();
        private List<Tuple<string, int>> snakeCoordinates = new List<Tuple<string, int>>();

        string snail = "@";


        public Board()
        {
            // fill dictionary acording to rows-List
            foreach (int i in Enumerable.Range(0, this.colNum - 1))
            {
                this.board.Add(this.letters[i], new List<string>());
            }

            // fill lists in dictionary with empty spaces according to rowSize-List
            foreach (var row in this.board)
            {
                foreach (int i in Enumerable.Range(1, this.rowNum))
                {
                    board[row.Key].Add(" ");
                }

            }

            calculateBorder();

            placeSnake();
            placeSnail();
        }

        private void calculateBorder()
        {
            foreach (int i in Enumerable.Range(1, this.rowNum * 2 + this.rowSizeOffset))
            {
                this.border += "#";
            }
        }

        public void printBoard()
        {
            Console.WriteLine();
            Console.WriteLine(this.border);

            foreach (var row in this.board)
            {
                Console.WriteLine(" ### {0} ## {1} ###", row.Key, string.Join(" ", row.Value));
            }

            Console.WriteLine(this.border);
        }

        public void placeSnail()
        {
            Random rng = new Random();

            string randomRow;
            int randomCol;

            do
            {
                randomRow = letters[rng.Next(this.rowNum) - 1];
                randomCol = rng.Next(this.colNum) - 1;

            } while (this.board[randomRow][randomCol] != " ");

            board[randomRow][randomCol] = this.snail;

        }

        private void placeSnake()
        {
            var defaultSnake = this.snake.getSnake();

            // determine middle of board
            string middleRow = this.letters[this.rowNum / 2];
            int middleCol = this.colNum / 2;

            // place snake at middle of board
            this.board[middleRow][middleCol] = defaultSnake[2];
            this.board[middleRow][middleCol - 1] = defaultSnake[1];
            this.board[middleRow][middleCol - 2] = defaultSnake[0];

            // update snakeCoordinates, insert head last
            this.snakeCoordinates.Add(Tuple.Create(middleRow, middleCol - 2));
            this.snakeCoordinates.Add(Tuple.Create(middleRow, middleCol - 1));
            this.snakeCoordinates.Add(Tuple.Create(middleRow, middleCol));
        }

        public void moveSnake(string newDirection)
        {
            // move head + update snakeCoordinates
            Boolean snailEaten = moveHead(newDirection);

            // move tail (skip if snail is eaten) + update Snake Coordinates
            if (!snailEaten)
            {
                moveTail();
            }

            // check win conditions (no empty fields = win)
            checkWinCondition();

            // place new snail (only if snail was eaten)
            if (snailEaten)
            {
                placeSnail();
            }
        }

        private void moveTail()
        {
            var tailLocation = this.snakeCoordinates[0];

            this.board[tailLocation.Item1][tailLocation.Item2] = " ";

            this.snakeCoordinates.RemoveAt(0);
        }

        private Boolean moveHead(string newDirection)
        {
            var oldLocation = this.snakeCoordinates.Last();
            string oldRow = oldLocation.Item1;
            int oldCol = oldLocation.Item2;

            string newRow = oldRow;
            int newCol = oldCol;

            try
            {
                switch (newDirection)
                {
                    case "W":
                        newRow = this.letters[this.letters.IndexOf(oldRow) - 1];
                        break;

                    case "A":
                        newCol = oldCol - 1;
                        break;

                    case "S":
                        newRow = this.letters[this.letters.IndexOf(oldRow) + 1];
                        break;

                    case "D":
                        newCol = oldCol + 1;
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You bump your head and fall unconcious. Have a nice sleepy time.");
                throw;
            }

            // overwrite old head location with body
            this.board[oldRow][oldCol] = this.snake.getBodypart();

            // determine new location
            var newLocation = Tuple.Create(newRow, newCol);

            // check if new location has snail
            Boolean snailEaten = this.board[newLocation.Item1][newLocation.Item2] == this.snail;

            // move head to new location
            this.board[newLocation.Item1][newLocation.Item2] = this.snake.getHead(newDirection);

            // add new location to list
            this.snakeCoordinates.Add(newLocation);

            return snailEaten;


        }

        private Boolean checkWinCondition()
        {
            foreach (var item in this.board)
            {
                if (item.Value.Contains(" "))
                {
                    return false;
                }

            }

            return true;
        }
    }
}
