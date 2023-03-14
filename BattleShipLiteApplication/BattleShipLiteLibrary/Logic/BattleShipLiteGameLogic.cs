using BattleShipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLiteLibrary.Logic
{
    public static class BattleShipLiteGameLogic
    {

        public static void InitializeGrid(PlayerInformationModel model)
        {
            List<string> Letters = new List<string>()
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> Numbers = new List<int>()
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in Letters)
            {
                foreach (int number in Numbers)
                {
                    AddGridSlot(model, letter, number);
                }
            }
        }


        private static void AddGridSlot(PlayerInformationModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel()
            {
                GridSpotletter = letter,
                GridSpotNumber = number,
                Status = GridSpotStatus.Empty

            };
            model.ShotGrid.Add(spot);
        }


        public static bool PlaceShip(PlayerInformationModel model, string location)
        {
            bool output = false;

            (string row, int column) = SplitShotintoRowAndColumn(location);

            bool IsValidLocation = ValidateGridLocation(model, row, column);

            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if (IsValidLocation && isSpotOpen)
            {
                model.ShipLocations.Add(new GridSpotModel
                {
                    GridSpotletter = row.ToUpper(),
                    GridSpotNumber = column,
                    Status = GridSpotStatus.Ship
                });

                output = true;
            }
            return output;
        }

        private static bool ValidateShipLocation(PlayerInformationModel model, string row, int column)
        {
            bool isValidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.GridSpotletter == row.ToUpper()
                    && ship.GridSpotNumber == column)
                {
                    isValidLocation = false;
                }
            }
            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInformationModel model, string row, int column)
        {
            bool isValidGridLocation = false;

            foreach (var ship in model.ShotGrid)
            {
                if (ship.GridSpotletter == row.ToUpper()
                    && ship.GridSpotNumber == column)
                {
                    isValidGridLocation = true;
                }
            }
            return isValidGridLocation;
        }

        public static bool PlayerStillActive(PlayerInformationModel player)
        {
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static (string row, int column) SplitShotintoRowAndColumn(string shot)
        {
            string row;
            int column;

            if (shot.Length != 2)
            {
                throw new ArgumentException("This was an invalid shot type", "shot");
            }

            char[] shotArray = shot.ToArray();

            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);
        }

        public static bool ValidateShot(PlayerInformationModel player, string row, int column)
        {
            bool IsValidShot = false;

            foreach (var gridspot in player.ShotGrid)
            {
                if (gridspot.GridSpotletter == row.ToUpper() && gridspot.GridSpotNumber == column)
                {
                    if (gridspot.Status == GridSpotStatus.Empty)
                    {
                        IsValidShot = true;
                    }
                }
            }
            return IsValidShot;
        }

        public static int GetShotCount(PlayerInformationModel player)
        {
            int shotcount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    shotcount += 1;
                }
            }

            return shotcount;
        }

        public static bool IdentifyShotResult(PlayerInformationModel opponent, string row, int column)
        {
            bool IsaHit = false;

            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.GridSpotletter == row.ToUpper()&& ship.GridSpotNumber == column)
                {
                    IsaHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }
            return IsaHit;
        }

        public static void MarkShotResult(PlayerInformationModel player, string row, int column, bool isAHit)
        {
            foreach (var gridspot in player.ShotGrid)
            {
                if (gridspot.GridSpotletter == row.ToUpper() && gridspot.GridSpotNumber == column)
                {
                    if (isAHit)
                    {
                        gridspot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridspot.Status = GridSpotStatus.Miss;
                    }
                }
            }

        }
    }
}