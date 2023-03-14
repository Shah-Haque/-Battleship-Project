using BattleShipLiteLibrary.Logic;
using BattleShipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInformationModel ActivePlayer = CreatePlayer("Player 1");
            PlayerInformationModel Opponent = CreatePlayer("Player 2");
            PlayerInformationModel Winner = null;

            do
            {
                DisplayShotGrid(ActivePlayer);
             
                RecordPlayerShot(ActivePlayer, Opponent);

                bool DoesGameContinue = BattleShipLiteGameLogic.PlayerStillActive(Opponent);

                if (DoesGameContinue == true)
                {
                    //  Swap using temp variable
                    PlayerInformationModel tempholder = Opponent;
                    Opponent = ActivePlayer;
                    ActivePlayer = tempholder;

                    //use tuple - swap positions
                   //(ActivePlayer, Opponent) = (Opponent, ActivePlayer);
                }
                else
                {
                    Winner = ActivePlayer;
                }

            } while (Winner == null);

            // Else, swap positions (active player to opponent)
            IdentifyWinner(Winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInformationModel winner)
        {
            Console.WriteLine($"Congrats to {winner.UsersName} for winning");
            Console.WriteLine($"{winner.UsersName} took {BattleShipLiteGameLogic.GetShotCount(winner)} shots");
        }

        private static void RecordPlayerShot(PlayerInformationModel ActivePlayer, PlayerInformationModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot(ActivePlayer);

                try
                {
                    (row, column) = BattleShipLiteGameLogic.SplitShotintoRowAndColumn(shot);

                    isValidShot = BattleShipLiteGameLogic.ValidateShot(ActivePlayer, row, column);
                }
                catch (Exception ex)
                {

                   Console.WriteLine("Error: " + ex.Message);
                 
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot please try again");
                }

            } while (isValidShot == false);

            bool IsAHit = BattleShipLiteGameLogic.IdentifyShotResult(opponent, row, column);
   
            BattleShipLiteGameLogic.MarkShotResult(ActivePlayer,row,column, IsAHit);

            DisplayShotResults(row,column,IsAHit);
        }

        private static void DisplayShotResults(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} is a Hit!");
            }
            else 
            {
                Console.WriteLine($"{row}{column} is a Miss.");
            }
            Console.WriteLine();
        }

        private static string AskForShot(PlayerInformationModel player)
        {
            Console.Write($"{player.UsersName}, Please enter your shot selection:  ");
            string output = Console.ReadLine();

            return output;
        }

        private static void DisplayShotGrid(PlayerInformationModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].GridSpotletter;

            foreach (var gridspot in activePlayer.ShotGrid)
            {
                if (gridspot.GridSpotletter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridspot.GridSpotletter;
                }
               
                //Move IF Statements to a method of their own 
                if (gridspot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"{gridspot.GridSpotletter}{gridspot.GridSpotNumber} "); 
                }
                else if (gridspot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridspot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();

        }

        private static PlayerInformationModel CreatePlayer(string PlayerTitle)
        {
            PlayerInformationModel output = new PlayerInformationModel();

            Console.WriteLine($"player information for {PlayerTitle}");

            //  Ask the user for their name
            output.UsersName =  AskForUserName();

            //  Load up the shot grid
            BattleShipLiteGameLogic.InitializeGrid(output);

            //  Ask the user for five placements
            PlaceShips(output);

            //  Clear 
            Console.Clear();

            return output;
        }

        private static void PlaceShips(PlayerInformationModel model)
        {
            do
            {
                Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}:  ");

                string Location = Console.ReadLine();

                bool IsValidLocation = false;

                try
                {
                    IsValidLocation = BattleShipLiteGameLogic.PlaceShip(model, Location);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("error: " + ex.Message);
                }

                if (IsValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location please try again..");
                }

            } while (model.ShipLocations.Count < 5);
        }

        private static string AskForUserName()
        {
            Console.Write("What is your Name: ");
            string output = Console.ReadLine();

            return output;
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("welcome to BattleShip Lite");
            Console.WriteLine("Created by Shah Haque");
            Console.WriteLine();
        }
    }
}