using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColosseumData;
using ColosseumFoundation;


namespace ColosseumGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Fighter player1 = new Pirate.PirateFighter(500,300,400,20);
            Fighter player2 = new Barbarian.BarbarianFighter(700, 250, 300, 15);

            Console.WriteLine("Health: " + player1.Health.ToString() + '\n' + "Speed: " + player1.Speed.ToString());

            Moveset moveset = new Moveset(player1.Speed);
            moveset.Push(new Tuple<Move, Fighter>(player1.AvailableMoves[0], player2));
            moveset.Push(new Tuple<Move, Fighter>(player1.AvailableMoves[1], player2));
            player1.ProcessMoveset(moveset);
            Console.WriteLine('\n');
            Console.WriteLine("Health: " + player1.Health.ToString() + '\n' + "Speed: " + player1.Speed.ToString());
            Console.WriteLine("Health: " + player2.Health.ToString() + '\n' + "Speed: " + player2.Speed.ToString());
            Console.Read();
        }
    }
}
