﻿using System;
using System.Collections.Generic;
using BattleShip.GameLogic;
using BattleShip.Requests;
using BattleShip.Responses;
using BattleShip.Ships;

namespace BattleShip
{
    public class ControlInput
    {
        public static bool IsMusicOn()
        {
            string strResult = "";
            do
            {
                Console.Write("Enable Music? Y/N : ");
                strResult = Console.ReadLine(); strResult = strResult.Trim().ToLower();
            } while (strResult != "y" && strResult != "n");
            return strResult == "y";
        }

        public static string[] GetNameFromUser()
        {
            string strLevel = "", player1 = "", player2 = "";
            

            do
            {
                Console.Write("Enter player 1 name: ");
                player1 = Console.ReadLine();
            } while (player1.Trim() == "");
          
            
                do
                {
                    Console.Write("Enter player 2 name: ");
                    player2 = Console.ReadLine();
                } while (player2.Trim() == "");
            


            return new string[] { player1, player2, strLevel };
        }

        public static bool IsPlaceBoardAuto()
        {
            string strResult = "";
            do
            {
                Console.Write("Let the game places the ship on the board? Y/N : ");
                strResult = Console.ReadLine(); strResult = strResult.Trim().ToLower();
            } while (strResult != "y" && strResult != "n");
            return strResult == "y";
        }

        public static ShipDirection getDirection(string direction)
        {
            switch (direction.ToLower())
            {
                case "l":
                    return ShipDirection.Left;

                case "r":
                    return ShipDirection.Right;

                case "u":
                    return ShipDirection.Up;

                default:
                    return ShipDirection.Down;

            }

        }

        public static PlaceShipRequest GetLocationFromUser(string ShipType)
        {
            PlaceShipRequest result = null;
            do
            {
                Console.Write("- " + ShipType + ": ");
                result = GetLocation(Console.ReadLine());
                if (result != null) 
                    return result;
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please input location and direiction. Ex:) a2, r");
                Console.ForegroundColor = ConsoleColor.White;
            }while(result is null);
            return result;
        }

        public static PlaceShipRequest GetLocation(string location)
        {
            string strX, strY, strDirection; int x, y;

            if (location.Split(',').Length == 2)
            {
                if (location.Split(',')[0].Trim().Length > 1)
                {
                    strX = location.Split(',')[0].Trim().Substring(0, 1);
                    strY = location.Split(',')[0].Trim().Substring(1);
                    strDirection = location.Split(',')[1].ToLower().Trim();

                    x = GetNumberFromLetter(strX);
                    if (x > 0 && x < 11 && int.TryParse(strY, out y) && y > 0 && y < 11
                        && (strDirection == "l"
                        || strDirection == "r"
                        || strDirection == "u"
                        || strDirection == "d"))
                    {
                        PlaceShipRequest ShipToPlace = new PlaceShipRequest();
                        ShipToPlace.Direction = getDirection(strDirection);
                        ShipToPlace.Coordinate = new Coordinate(x, y);
                        return ShipToPlace;
                    }
                }
            }
            return null;
        }
        public static PlaceShipRequest GetLocationFromComputer()
        {
            PlaceShipRequest ShipToPlace = new PlaceShipRequest();
            ShipToPlace.Direction = getDirection(GetRandom.GetDirection());
            ShipToPlace.Coordinate = new Coordinate(GetRandom.GetLocation(), GetRandom.GetLocation());
            return ShipToPlace;
        }

        public static Coordinate GetShotLocationFromUser()
        {
            string result = ""; int x, y;
            while (true)
            {
                Console.Write("Enter location you want to shot  ");
                result = Console.ReadLine();
                if (result.Trim().Length > 1)
                {
                    x = GetNumberFromLetter(result.Substring(0, 1));
                    if (x > 0 && int.TryParse(result.Substring(1), out y))
                    {
                        return new Coordinate(x, y);
                    }
                }
            }
        }



        static Coordinate GetRightLocationToShot(Board victimboard)
        {
            List<Coordinate> tmpList = new List<Coordinate> { };
            for (int i = 0; i < victimboard.Ships.Length; i++)
            {
                Ship tmpShip = victimboard.Ships[i];
                for (int j = 0; j < tmpShip.BoardPositions.Length; j++)
                {
                    if (victimboard.CheckCoordinate(tmpShip.BoardPositions[j]) == ShotHistory.Unknown)
                        tmpList.Add(tmpShip.BoardPositions[j]);
                }
            }

           return tmpList[GetRandom.r.Next(0, tmpList.Count - 1)];

        }

        static int GetNumberFromLetter(string letter)
        {
            int result = -1;
            switch (letter.ToLower())
            {
                case "a":
                    result = 1;
                    break;
                case "b":
                    result = 2;
                    break;
                case "c":
                    result = 3;
                    break;
                case "d":
                    result = 4;
                    break;
                case "e":
                    result = 5;
                    break;
                case "f":
                    result = 6;
                    break;
                case "g":
                    result = 7;
                    break;
                case "h":
                    result = 8;
                    break;
                case "i":
                    result = 9;
                    break;
                case "j":
                    result = 10;
                    break;
                default:
                    break;
            }
            return result;
        }

        public static bool CheckQuit()
        {
            Console.WriteLine("Press F5 to replay or any key to quit...");
            return Console.ReadKey().Key == ConsoleKey.F5;
        }
    }
}
