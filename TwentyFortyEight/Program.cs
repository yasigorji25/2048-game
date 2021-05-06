using System;

namespace TwentyFortyEight {
    /// <summary>
    ///   Runs the game 2048
    /// </summary>
    /// <author>Yasaman Gorjinejad</author>
    /// <student_id>n10295674</student_id>
    public static class Program {
        /// <summary>
        /// Specifies possible moves in the game
        /// </summary>
        public enum Move { Up, Left, Down, Right, Restart, Quit };

        /// <summary>
        /// Generates random numbers
        /// </summary>
        static Random numberGenerator = new Random();

        /// <summary>
        /// Number of initial digits on a new 2048 board
        /// </summary>
        const int NUM_STARTING_DIGITS = 2;

        /// <summary>
        /// The chance of a two spawning
        /// </summary>
        const float CHANCE_OF_TWO = 0.9f; // 90% chance of a two; 10% chance of a four

        /// <summary>
        /// The size of the 2048 board
        /// </summary>
        const int BOARD_SIZE = 4; // 4x4

        public static object Row { get; private set; }

        /// <summary>
        /// Runs the game of 2048
        /// </summary>
        static void Main() {

            Move userinput = Move.Left;
            int[,] board = MakeBoard();
            Console.WriteLine("2048 - Join the numbers and get to the 2048 tile!");
            Console.WriteLine();

            DisplayBoard(board);
            Console.WriteLine("w: up     a: left \ns: down   d:right \n\nr:restart \nq: quit");

            while (userinput != Move.Quit) {

                userinput = ChooseMove();
                if (userinput == Move.Restart) {

                    board = MakeBoard();
                    DisplayGame(board);
                } else if (GameOver(board) == true) {
                    DisplayGame(board);
                    Console.WriteLine();
                    Console.WriteLine("Game Over!");
                    Console.WriteLine("press 'r' if you wanna play the game OR press 'q' if you wanna quit. ");
                } else if (MakeMove(userinput, board) == true) {
                    PopulateAnEmptyCell(board);
                    DisplayGame(board);

                } else {
                    DisplayGame(board);
                }
            }

        }
        /// <summary>
        /// clear the console the display the board with the guidence(a : left)
        /// </summary>
        /// <param name="board">2048 game board to display</param>
        public static void DisplayGame(int[,] board) {
            Console.Clear();
            Console.WriteLine("2048 - Join the numbers and get to the 2048 tile!");
            Console.WriteLine();
            DisplayBoard(board);
            Console.WriteLine("w: up     a: left \ns: down   d:right \n\nr:restart \nq: quit");
        }
        /// <summary>
        /// applies the GameOver on 2048 board game.
        /// </summary>
        /// <param name="board">2048 board game</param>
        /// <returns>return true if the board is full and the numbers adjustand to each other are not combinable, false otherwise </returns>
        public static bool GameOver(int[,] board) {
            bool GameOver = true;
            // if the board is full then check if the numbers in the rows and column are combinable
            // if it is combinable then return false 
            if (IsFull(board) == true) {
                for (int row = 0; row < board.GetLength(0); row++) {
                    for (int column = 0; column < board.GetLength(1) - 1; column++) {
                        if (board[row, column] == board[row, column + 1]) {
                            GameOver = false;
                        }
                    }
                }
                for (int column = 0; column < board.GetLength(1); column++) {
                    for (int row = 0; row < board.GetLength(0) - 1; row++) {
                        if (board[row, column] == board[row + 1, column]) {
                            GameOver = false;
                        }
                    }
                }

            }
            // the board is not full 
            else {
                GameOver = false;
            }
            // the board is full and the numbers(next to rach other) are not combinable! Game Over 
            return GameOver;
        }





        /// Generates a new 2048 board
        /// </summary>
        /// <returns>A new 2048 board</returns>
        public static int[,] MakeBoard() {
            // Make a BOARD_SIZExBOARD_SIZE array of integers (filled with zeros)
            int[,] board = new int[BOARD_SIZE, BOARD_SIZE];

            // Populate some random empty cells
            for (int i = 0; i < NUM_STARTING_DIGITS; i++) {
                PopulateAnEmptyCell(board);
            }

            return board;
        }

        /// <summary>
        /// Display the given 2048 board
        /// </summary>
        /// <param name="board">The 2048 board to display</param>
        public static void DisplayBoard(int[,] board) {
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int column = 0; column < board.GetLength(1); column++) {
                    Console.Write("{0,4}", board[row, column] == 0 ? "-" : board[row, column].ToString());
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// If the board is not full, choose a random empty cell and add a two or a four.
        /// There should be a 90% chance of adding a two, and a 10% chance of adding a four.
        /// </summary>
        /// <param name="board">The board to add a new number to</param>
        /// <returns>False if the board is already full; true otherwise</returns>
        public static bool PopulateAnEmptyCell(int[,] board) {


            // check if the board is already full

            if (IsFull(board)) {
                return false;
            }
            int row, column;
            do {
                // 1. choose a random row nad random column 
                row = numberGenerator.Next(0, BOARD_SIZE);
                column = numberGenerator.Next(0, BOARD_SIZE);
                // 2. check the location to see if it's a zero 
            } while (board[row, column] != 0);
            // generate a new number to go in the location 
            int newNum;
            if (numberGenerator.Next(1, 101) <= (int)(CHANCE_OF_TWO * 100)) {
                // 90% of the time 
                newNum = 2;
            } else {
                // 10% of the time 
                newNum = 4;
            }
            // store the numbers and show them in the board 
            board[row, column] = newNum;
            return true;
        }
        /// <summary>
        /// Returns true if the given 2048 board is full (contains no zeros)
        /// </summary>
        /// <param name="board">A 2048 board to check</param>
        /// <returns>True if the board is full; false otherwise</returns>
        public static bool IsFull(int[,] board) {
            // check the rows and columns if the number is equal to zero means it is empty (return false)
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int column = 0; column < board.GetLength(1); column++) {
                    if (board[row, column] == 0) {
                        return false;
                    }

                }

            }
            return true;

        }
        /// <summary>
        /// Get a Move from the user (such as UP, LEFT, DOWN, RIGHT, RESTART or QUIT)
        /// </summary>
        /// <returns>The chosen Move</returns>
        public static Move ChooseMove() {
            bool good_input = false;
            string input = "";
            // if the user input is not good ask the user untill the user enter a good input(wasdqr)
            while (!good_input) {
                input = Console.ReadKey().KeyChar.ToString();
                if ("wasdqr".Contains(input.ToLower()))
                    good_input = true;
            }

            if (input == "w") {
                return Move.Up;
            } else if (input == "a") {
                return Move.Left;
            } else if (input == "s") {
                return Move.Down;
            } else if (input == "d") {
                return Move.Right;
            } else if (input == "r") {
                return Move.Restart;
            } else {
                return Move.Quit;
            }

        }

        /// <summary>
        /// Applies the chosen Move on the given 2048 board
        /// </summary>
        /// <param name="move">A move such as UP, LEFT, RIGHT or DOWN</param>
        /// <param name="board">A 2048 board</param>
        /// <returns>True if the move had an affect on the game; false otherwise</returns>
        public static bool MakeMove(Move move, int[,] board) {
            bool affected = false;
            // move up 
            if (move == Move.Up) {
                for (int i = 0; i < BOARD_SIZE; i++) {

                    int[] column = MatrixExtensions.GetCol(board, i);
                    if (ShiftCombineShift(column, true) == true) {
                        MatrixExtensions.SetCol(board, i, column);
                        affected = true;
                    }
                }
            }
            // move down 
            else if (move == Move.Down) {
                for (int i = 0; i < BOARD_SIZE; i++) {

                    int[] column = MatrixExtensions.GetCol(board, i);
                    if (ShiftCombineShift(column, false) == true) {
                        MatrixExtensions.SetCol(board, i, column);
                        affected = true;
                    }
                }
            }
            // move right 
            else if (move == Move.Right) {
                for (int i = 0; i < BOARD_SIZE; i++) {

                    int[] row = MatrixExtensions.GetRow(board, i);
                    if (ShiftCombineShift(row, false) == true) {
                        MatrixExtensions.SetRow(board, i, row);
                        affected = true;
                    }
                }
            }
            // move left
            else {
                for (int i = 0; i < BOARD_SIZE; i++) {

                    int[] row = MatrixExtensions.GetRow(board, i);
                    if (ShiftCombineShift(row, true) == true) {
                        MatrixExtensions.SetRow(board, i, row);
                        affected = true;
                    }
                }
            }
            return affected;

        }


        /// <summary>
        /// Shifts the non-zero integers in the given 1D array to the left
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if shifting had an effect; false otherwise</returns>
        public static bool ShiftLeft(int[] nums) {
            bool affected = false;
            // creat an array call Shifted.
            int[] shifted = new int[nums.Length]; //{0,0,0,0}
            int counter = 0;
            for (int i = 0; i < nums.Length; i++) {
                if (nums[i] != 0) {
                    shifted[counter] = nums[i];
                    counter++;
                }
            }
            // has the array changed?
            for (int i = 0; i < nums.Length; i++) {
                // it has change (return true)
                if (nums[i] != shifted[i]) {
                    nums[i] = shifted[i];

                    affected = true;
                }
            }
            // it dosent change 
            return affected;

        }

        /// <summary>
        /// Combines identical, non-zero integers that are adjacent to one another by summing 
        /// them in the left integer, and replacing the right-most integer with a zero
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3  }
        ///   It will be modified to:
        ///       { 0, 4, 0, 8, 0, 0, 0, 16, 0, 5, 3  }
        /// </example>
        public static bool CombineLeft(int[] nums) {
            bool affected = false;
            for (int i = 0; i < nums.Length - 1; i++) {
                // summing the non-zero integers that are adjacent to one another and put it in the left integers 
                if (nums[i] == nums[i + 1] && nums[i] != 0) {
                    // sum the numbers and store it in the left one.
                    nums[i] += nums[i + 1];
                    // then make the other one zero 
                    nums[i + 1] = 0;
                    affected = true;
                }

            }
            // cant be combine 
            return affected;

        }

        /// <summary>
        /// Shifts the numbers in the array in the specified direction, then combines them, then 
        /// shifts them again.
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <param name="left">True if numbers should be shifted to the left; false otherwise</param>
        /// <returns>True if shifting and combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values below, and shiftLeft is true:
        ///       { 0, 2, 2,  4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 4, 8, 16, 5, 3, 0, 0, 0, 0, 0, 0  }
        ///       
        ///   If nums has the values below, and shiftLeft is false:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 0, 0, 0, 0, 0, 0, 2, 8, 16, 5, 3 }
        /// </example>
        public static bool ShiftCombineShift(int[] nums, bool shiftLeft) {
            // reverse the array if we wanna shiftRight 
            if (shiftLeft == false) {
                Array.Reverse(nums);
            }
            bool affected_1 = ShiftLeft(nums);
            bool affected_2 = CombineLeft(nums);
            bool affected_3 = ShiftLeft(nums);
            // reverse the array if we wanna shiftRight 
            if (shiftLeft == false) {
                Array.Reverse(nums);
            }
            // if one of them affect means ShiftCombineShif affect. 
            return affected_1 || affected_2 || affected_3;
        }

    }

}