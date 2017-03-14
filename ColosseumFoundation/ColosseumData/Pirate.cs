using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColosseumFoundation;

namespace ColosseumData
{
    public class Pirate
    {
        public class PirateFighter : Fighter
        {
            public PirateFighter(double totalHealth, double totalMana, double totalSpeed, double strength) : base(totalHealth, totalMana, totalSpeed, strength)
            {
                AvailableMoves = new List<Move> { new Attack(this, totalSpeed / 6),
                                                  new Shoot(this, totalSpeed / 3, strength * 2.5),
                                                  new Blind(this, totalSpeed / 4, totalMana / 4, 0.125, 5) };
            }
        }

        public class Shoot : Move
        {
            public Shoot(Fighter user, double speedcost, double strength) : base(user, speedcost)
            {
                Name = "Flintlock Shot";
                AdditionalReceiverEffects.Add(new QuickProjectile(strength));
            }
        }

        public class Blind : Move
        {
            public Blind(Fighter user, double speedCost, double manaCost, double reductionRatio, int lifespan) : base(user,speedCost)
            {
                Name = "Blinding Parrot";
                FlatSpeedCost = speedCost;
                FlatManaCost = manaCost;
                AdditionalReceiverEffects.Add(new Blinded(0, lifespan, false, reductionRatio));
            }
        }

        public class Blinded : PassiveEffect
        {
            public Blinded(int priority, int lifespan, bool InitCalcBool, double reductionRatio) : base(priority, lifespan, InitCalcBool)
            {
                modDelegate = x => x - (x * reductionRatio);
                IOType = ModType.Move;
            }
        }
    }
}
