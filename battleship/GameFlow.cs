using System;
using System.Media;
using BattleShip.Requests;
using BattleShip.Responses;

namespace BattleShip
{
    class GameFlow
    {
        GameState gm;

        public GameFlow()
        {
            gm = new GameState() { IsPlayer1 = false, Player1 = new Player(), Player2 = new Player() };
        }
        
        public void Start()
        {

            GameSetup GameSetup = new GameSetup(gm);
            GameSetup.Music();
            GameSetup.Setup();


            do
            {
                GameSetup.SetBoard();
                FireShotResponse shotresponse;
                do
                {
                    ControlOutput.ResetScreen(new Player[] { gm.Player1, gm.Player2 }) ;
                    ControlOutput.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player1 : gm.Player2);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1, false);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player1 : gm.Player2, true);
                    Coordinate ShotPoint = new Coordinate(1,1);
                    shotresponse = Shot(gm.IsPlayer1 ? gm.Player2 : gm.Player1, gm.IsPlayer1 ? gm.Player1 : gm.Player2, out ShotPoint);

                    ControlOutput.ResetScreen(new Player[] { gm.Player1, gm.Player2 });
                    ControlOutput.ShowWhoseTurn(gm.IsPlayer1 ? gm.Player1 : gm.Player2);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player2 : gm.Player1, false);
                    ControlOutput.DrawHistory(gm.IsPlayer1 ? gm.Player1 : gm.Player2, true);
                    ControlOutput.ShowShotResult(shotresponse, ShotPoint, gm.IsPlayer1 ? gm.Player1.Name : gm.Player2.Name);
                    if (shotresponse.ShotStatus != ShotStatus.Victory)
                    {
                        Console.WriteLine("Press any key to continue to switch to " + (gm.IsPlayer1 ? gm.Player2.Name : gm.Player1.Name));
                        gm.IsPlayer1 = !gm.IsPlayer1;
                        Console.ReadKey();
                    }
                } while (shotresponse.ShotStatus != ShotStatus.Victory);

            } while (ControlInput.CheckQuit());
        }


        private FireShotResponse Shot(Player victim, Player Shoter, out Coordinate ShotPoint)
        {
            FireShotResponse fire; Coordinate WhereToShot;
            do
            {

                    WhereToShot = ControlInput.GetShotLocationFromUser();
                    fire = victim.PlayerBoard.FireShot(WhereToShot);
                    if (fire.ShotStatus == ShotStatus.Invalid || fire.ShotStatus == ShotStatus.Duplicate)
                        ControlOutput.ShowShotResult(fire, WhereToShot, "");                    
             

                if (fire.ShotStatus == ShotStatus.Victory)
                {
                    if (gm.IsPlayer1) gm.Player1.Win += 1;
                    else gm.Player2.Win += 1;
                }
            } while (fire.ShotStatus == ShotStatus.Duplicate || fire.ShotStatus == ShotStatus.Invalid);
            ShotPoint = WhereToShot;
            return fire;
        }
    }
}
