using System;
using System.Text;

namespace Matching_Game
{
    public class MessageOrganizer
    {
        public static string EnterName()
        {
            Console.WriteLine("Please enter player name:");
            return Console.ReadLine(); // return the input player name
        }

        public static int ChooseOpponent()
        {
            int opponentType;

            Console.WriteLine("Please choose your opponent: 1.player    2.computer");

            while (!(int.TryParse(Console.ReadLine(), out opponentType)) || (opponentType != 1 && opponentType != 2))
            {
                Console.WriteLine("Invalid input, please choose your opponent: 1.player    2.computer");
            }

            return opponentType;
        }

        public static void HandleBoardSize(ref int io_Height, ref int io_Width)
        {
            bool checkInput = true;
            StringBuilder heightStr = new StringBuilder();
            StringBuilder widthStr = new StringBuilder();

            Console.WriteLine("Please enter board size, so the size (height * width) will be even, minimum size(4x4) and maximum size(6x6).");

            while (checkInput)
            {
                Console.WriteLine("Enter height:");
                heightStr.Append(" "); // avoid empty stringBuilder error(if the user enter an empty string)
                heightStr.Replace(heightStr.ToString(), Console.ReadLine());
                Console.WriteLine("Enter width:");
                widthStr.Append(" "); // avoid empty stringBuilder error(if the user enter an empty string)
                widthStr.Replace(widthStr.ToString(), Console.ReadLine());

                if (!(int.TryParse(heightStr.ToString(), out io_Height)) || !(int.TryParse(widthStr.ToString(), out io_Width))) // syntax check and sizes allocation
                {
                    Console.WriteLine("Invalide input, please try again.");
                }
                else if (!(io_Height >= 4 && io_Height <= 6) || !(io_Width >= 4 && io_Width <= 6) || (io_Width * io_Height) % 2 != 0) // sizes check
                {
                    Console.WriteLine("Your choosen dimensions of the board are not fit for the requirements, please try again.");
                }
                else
                {
                    checkInput = false;
                }
            }
        }

        public static string ChooseSquareToReveal(int i_BoardHeight, int i_BoardWidth)
        {
            bool checkInput = true;
            StringBuilder SquareStr = new StringBuilder();

            Console.WriteLine("choose a square to reveal:(B3 for example)");

            while (checkInput)
            {
                SquareStr.Append(" "); // avoid empty stringBuilder error(if the user enter an empty string)
                SquareStr.Replace(SquareStr.ToString(), Console.ReadLine());

                if (SquareStr.ToString().Equals("Q"))
                {
                    exitGame();
                }
                else if (SquareStr.Length != 2 || !(Char.IsUpper(SquareStr[0])) || !(Char.IsDigit(SquareStr[1])))  // syntax check
                {
                    Console.WriteLine("Invalid input, please try again.");

                }
                else if (!(SquareStr[0] >= 'A' && SquareStr[0] < 'A' + i_BoardWidth) || !(SquareStr[1] >= '1' && SquareStr[1] < '1' + i_BoardHeight)) // limits check
                {
                    Console.WriteLine("Your choosen square is out of the board limits, please try again.");
                }
                else
                {
                    checkInput = false;
                }
            }

            return SquareStr.ToString();
        }

        public static void TurnMsg(string i_PlayerName = "Computer") // default for computer
        {
            ClearScreen();
            Console.WriteLine(string.Format(" Its {0} turn.", i_PlayerName));
            System.Threading.Thread.Sleep(1000);
        }

        public static void WinMessage(string i_WinnerName, MatchingGameUi.eEndGameWinner i_EndGameState)
        {
            string WinTypeMsg = string.Empty;

            switch (i_EndGameState)
            {
                case MatchingGameUi.eEndGameWinner.PlayerWon:
                    WinTypeMsg = string.Format("Congratulation {0}!, you won the game.", i_WinnerName);
                    break;
                case MatchingGameUi.eEndGameWinner.ComputerWon:
                    WinTypeMsg = "You lost the game.";
                    break;
                case MatchingGameUi.eEndGameWinner.Tie:
                    WinTypeMsg = "Its a tie.";
                    break;
            }

            Console.WriteLine(WinTypeMsg);
            System.Threading.Thread.Sleep(1000); // wait 1 second before exit
        }

        public static bool ReplayGame()
        {
            Console.WriteLine("Do you want to play another game? Y|N ");
            StringBuilder inputReplay = new StringBuilder(Console.ReadLine());

            while (!(inputReplay.ToString().Equals("Y")) && !(inputReplay.ToString().Equals("N")) && !(inputReplay.ToString().Equals("Q"))) // invalid replay check
            {
                Console.WriteLine("Invalid input, do you want to play again? Y|N");
                inputReplay.Append(" "); // avoid empty stringBuilder error(if the user enter an empty string)
                inputReplay.Replace(inputReplay.ToString(), Console.ReadLine());
            }

            return (inputReplay.ToString().Equals("Y"));
        }

        public static void TakenSquareMsg()
        {
            Console.WriteLine("The choosen squere is taken, please choose again:");
        }

        public static void ShowBoard(char[,] i_CurrentBoardToDisplay)
        {
            char colSign = 'A'; // the col sign
            StringBuilder separatorLine = new StringBuilder();
            StringBuilder currentLineToPrint = new StringBuilder(" ");

            ClearScreen();

            separatorLine.Append("  ").Append('=', 4 * i_CurrentBoardToDisplay.GetLength(1) + 1); // build the separator line

            // build the Col signs line
            for (int i = 0; i < i_CurrentBoardToDisplay.GetLength(1); i++, colSign++)
            {
                currentLineToPrint.Append(string.Format("   {0}", colSign));
            }

            Console.WriteLine(currentLineToPrint);
            Console.WriteLine(separatorLine);

            // print the board:
            for (int i = 0; i < i_CurrentBoardToDisplay.GetLength(0); i++)
            {
                currentLineToPrint.Replace(currentLineToPrint.ToString(), string.Format("{0} |", i + 1));

                for (int j = 0; j < i_CurrentBoardToDisplay.GetLength(1); j++)
                {
                    if (i_CurrentBoardToDisplay[i, j] != default(Char))
                    {
                        currentLineToPrint.Append(string.Format(" {0} |", i_CurrentBoardToDisplay[i, j]));
                    }
                    else
                    {
                        currentLineToPrint.Append("   |");
                    }
                }

                Console.WriteLine(currentLineToPrint);
                Console.WriteLine(separatorLine);
            }

            Console.WriteLine();
            System.Threading.Thread.Sleep(500);
        }

        public static void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public static void exitGame()
        {
            ClearScreen();
            Console.WriteLine("Bye Bye!");
            System.Threading.Thread.Sleep(1000); // wait 1 second before exit
            Environment.Exit(1);
        }
    }
}
