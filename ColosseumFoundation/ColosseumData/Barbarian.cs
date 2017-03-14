using ColosseumFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumData
{
    public class Barbarian
    {
        public class BarbarianFighter : Fighter
        {
            public BarbarianFighter(double totalHealth, double totalMana, double totalSpeed, double strength) : base(totalHealth, totalMana, totalSpeed, strength)
            {
                AvailableMoves = new List<Move> { new Attack(this, totalSpeed / 8),
                                                  new Stun(this, totalSpeed / 3, totalMana / 2.5, (int)Math.Floor(totalSpeed / 4), 0.25),
                                                  new Berserk(this,totalSpeed / 6, totalMana / 3, (int)Math.Floor(totalSpeed / 3), 1.33, (strength * 0.33) / (totalSpeed / 8) ) };
            }
        }

        public class Stun : Move
        {
            public Stun(Fighter user, double SpeedCost, double manaCost, int lifespan, double multiplier) : base(user, SpeedCost)
            {
                AdditionalReceiverEffects.Add(new Stunned(lifespan, multiplier));
            }
        }

        public class Stunned : PassiveEffect
        {
            public Stunned(int lifespan, double multiplier) : base(-4, lifespan, false)
            {
                modDelegate = x => x * (1 + multiplier);
                IOType = ModType.Weakness;
            }
        }

        public class Berserk : Move
        {
            public Berserk(Fighter user, double SpeedCost, double ManaCost, int lifespan, double strengthRatio, double healthTick) : base(user, SpeedCost)
            {
                AdditionalUserEffects.Add(new Uncontrolled(lifespan, healthTick));
                AdditionalUserEffects.Add(new Enraged(lifespan, (User.Strength * strengthRatio) - User.Strength));
            }
        }

        public class Uncontrolled : ActiveEffect
        {
            private double healthLoss;
            public Uncontrolled(int lifespan, double healthTick) : base(lifespan)
            {
               healthLoss = healthTick;
            }

            protected override void TickEffect()
            {
                affected.HealthDamage(healthLoss);
            }
        }

        public class Enraged : PassiveEffect
        {
            public Enraged(int lifespan, double strengthAmount) : base(-4, lifespan, true)
            {
                IOType = ModType.Buff;
                modDelegate = x => x + strengthAmount;
            }
        }
    }
}
