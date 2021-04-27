using System;
using System.Media;
using BattleShip.GameLogic;
using BattleShip.Requests;
using BattleShip.Responses;
using BattleShip.Ships;

namespace BattleShip
{
    public class GameSetup
    {
        GameState _gm;
        public GameSetup(GameState gm)
        {
            _gm = gm;
        }

        public void Music()
        {
            Console.ForegroundColor = ConsoleColor.White;
            ControlOutput.ShowHeader();

            bool IsMusicOn = false;
            IsMusicOn = ControlInput.IsMusicOn();
            if (IsMusicOn)
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\1.wav";
                player.Play();
            }
        }


        public void Setup()
        {
            Console.ForegroundColor = ConsoleColor.White;
            ControlOutput.ShowHeader();


            string[] userSetUp = ControlInput.GetNameFromUser();


            _gm.Player1.Name = userSetUp[0];
            _gm.Player1.Win = 0;

            _gm.Player2.Name = userSetUp[1];
            _gm.Player2.Win = 0;



        }

        public void SetBoard()
        {
            ControlOutput.ResetScreen(new Player[] { _gm.Player1, _gm.Player2 });

            _gm.IsPlayer1 = Responses.GetRandom.WhoseFirst();

            _gm.Player1.PlayerBoard = new Board();
            PlaceShipOnBoard(_gm.Player1);
            ControlOutput.ResetScreen(new Player[] { _gm.Player1, _gm.Player2 });

            _gm.Player2.PlayerBoard = new Board();
            PlaceShipOnBoard(_gm.Player2);
            Console.WriteLine("All ship were placed successfull! Press any key to continue...");
            Console.ReadKey();
        }

        public void PlaceShipOnBoard(Player player)
        {
            bool IsPlaceBoardAuto = false;
        
                ControlOutput.ShowWhoseTurn(player);
                IsPlaceBoardAuto = ControlInput.IsPlaceBoardAuto();
                if (!IsPlaceBoardAuto)
                    Console.WriteLine("Input the location and direction(l, r, u, d) of the ships. Ex:) a2, r:");
            for (ShipType s = ShipType.Destroyer; s <= ShipType.Carrier; s++)
            {
                PlaceShipRequest ShipToPlace = new PlaceShipRequest();
                ShipPlacement result;
                do
                {
                    if (!IsPlaceBoardAuto)
                    {
                        ShipToPlace = ControlInput.GetLocationFromUser(s.ToString());
                        ShipToPlace.ShipType = s;
                        result = player.PlayerBoard.PlaceShip(ShipToPlace);
                        if (result == ShipPlacement.NotEnoughSpace)
                            Console.WriteLine("Not Enough Space!");
                        else if (result == ShipPlacement.Overlap)
                            Console.WriteLine("Overlap placement!");
                    }
                    else
                    {
                        ShipToPlace = ControlInput.GetLocationFromComputer();
                        ShipToPlace.ShipType = s;
                        result = player.PlayerBoard.PlaceShip(ShipToPlace);
                    }

                } while (result != ShipPlacement.Ok);
            }
        }
    }
}
