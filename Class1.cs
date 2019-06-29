using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using Robocode.Util;

namespace Fonx
{
    public class Class1 :Robot
    {
        public List<Enemy> enemies;
        public int degress;
        public int direction;
        public float timeAct = float.MaxValue;
        public int sucessiveShoots;
        public int distanceToMoveWhenShooted;
        public override void Run()
        {
            direction = 1;
            sucessiveShoots = 0;
            TurnLeft(Heading - 90);
            TurnGunRight(90);
            distanceToMoveWhenShooted = 500;
            while (true)
            {
                if (timeAct + 5f < Time) { 
                    sucessiveShoots = 0;
                    timeAct = float.MaxValue;
                }
                if (sucessiveShoots >3) {
                    EvasiveManeuver();
                }
                Ahead(300);
                TurnRight(10 * direction);

                //int Counter = 0;
                //for (int i = 0; i < enemies.Count; i++) {//check how many enemies are close
                //    if (enemies[i].distance < 50) {
                //        Counter++;
                //    }
                //    if (Counter > 2) {//go Yollo \o/
                //        EvasiveManeuver();
                //    }
                //}

            }
        }
        //void OnHitWall(HitWallEvent evnt)
        //{
        //    direction *= -1;
        //}
        public void OnHitByBullet(HitByBulletEvent e) {
            sucessiveShoots++;
            timeAct = Time;

            TurnRight(Utils.NormalRelativeAngleDegrees(90-(Heading- e.Heading))); // sempre correr em 90 graus ao inimigo
            Ahead(distanceToMoveWhenShooted);
            distanceToMoveWhenShooted *= -1;   //talvez seja melhor sem isso.
            Scan();
            //continua aqui
        }
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            Enemy bot = new Enemy(e.Distance, e.Energy, e.Name);
            //if (enemies.Count == 0) {
            //    enemies.Add(bot);
            //}
            //for (int i = 0; i < enemies.Count + 1; i++)
            //{ //check if the enemy is already in the List and update his position.
            //    if (enemies[i].name == bot.name)
            //    {
            //        //enemies[i] = bot;
            //        break;
            //    }
            //    else if ((i == enemies.Count-1) && enemies[i].name != bot.name)
            //    { //if the enemy is not in the List -> update 
            //        enemies.Add(bot);
            //    }
            //}
            if (e.Distance < 50 && Energy > 50)
            {
                Fire(5);
            }
            else {
                Fire(3);
            }
            //if (e.Distance > 150) {
            //    double absBearing = e.Bearing + Heading;
            //    TurnGunRight(Utils.NormalRelativeAngle(absBearing - GunHeading + e.Velocity));
            //    Ahead((e.Distance - 140) * direction);

            //} ficou legal n isso...

            if (e.Bearing >= 0) // mudar para  -45> e.Bearing >= 45 ver: http://mark.random-article.com/weber/java/robocode/lesson4.html
            {
                direction = 1;
            }
            else {
                direction = -1;
            }
            Ahead(e.Distance/2);
            int random = RandomNum(1,3);
            for (int i = 0; i < random;i++) {
                Ahead(100);
                TurnRight(45);
            }
            Scan();
        }
        
        private int RandomNum(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private void EvasiveManeuver() {//mudar isso
            if (X > 2500 || Y > 2500)
            {
                TurnRight(Utils.NormalRelativeAngleDegrees(90));
                Back(2500);
            }
            else if (X > -2500 || Y < -2500)
            {
                TurnLeft(Utils.NormalRelativeAngleDegrees(90));
                Ahead(2500);
            }
            else if (X > -2500 || Y > 2500)
            {
                TurnLeft(Utils.NormalRelativeAngleDegrees(45));
                Back(2500);
            }
            else {
                TurnLeft(Utils.NormalRelativeAngleDegrees(0));
                Ahead(2500);
            }
        }
    }
    public class Enemy
    {
        public double distance;
        //public int posY;
        //public int posX;
        public double energy;
        public string name;
        public Enemy(double distance, double energy, string name)
        {
            this.distance = distance; this.energy = energy; this.name = name;
        }
    }
}
