using ColosseumFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumData
{
    public class Mage
    {
        public class MageFighter : Fighter
        {
            public MageFighter(double totalHealth, double totalMana, double totalSpeed, double strength) : base(totalHealth, totalMana, totalSpeed, strength)
            {
                AvailableMoves = new List<Move> { new Attack(this, totalSpeed / 6),
                                                  new Fireball(this, totalSpeed / 4, totalMana / 3, Strength * 2, Strength / 4, 5),
                                                  new Meditate(this,totalSpeed / 5, totalMana / 4, totalHealth / 8) };
            }
        }

        public class Fireball : Move
        {
            public Fireball(Fighter user, double SpeedCost, double manaCost, double hitStrength, double burnStrength, int burnTime) : base(user, SpeedCost)
            {
                FlatDamage = hitStrength;
                FlatManaCost = manaCost;
                AdditionalReceiverEffects.Add(new Burning(burnTime, burnStrength));
            }
        }

        public class Burning : ActiveEffect
        {
            private double burn;

            public Burning(int lifespan, double strength) : base(lifespan)
            {
                burn = strength;
            }

            protected override void TickEffect()
            {
                affected.Damage(burn);
            }
        }

        public class Meditate : Move
        {
            public Meditate(Fighter user, double SpeedCost, double manaCost, double healFactor) : base(user, SpeedCost)
            {
                FlatManaCost = manaCost;
                user.HealthDamage(-healFactor);
            }
        }
    }
}
