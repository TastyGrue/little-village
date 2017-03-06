using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{

    /// <summary>
    /// Fighters are the characters of Colosseum.
    /// </summary>
    public abstract class Fighter
    {
        public Fighter(double health, double mana, double armor, List<Move> moves)
        {
            Health = health;
            Mana = mana;
            Armor = armor;
            moves = new List<Move> { new Attack(this), new Block(this) };
        }

        /// <summary>
        /// The health of the fighter. Normally considered dead when it hits 0.
        /// </summary>
        public double Health { get; protected set; }

        /// <summary>
        /// The mana of the fighter; spent when doing special moves. Regenerates.
        /// </summary>
        public double Mana { get; protected set; }

        /// <summary>
        /// The armor of the fighter. Expended when the fighter uses block, and blocks an attack,
        /// and when the fighter receives physical damage.
        /// </summary>
        public double Armor { get; protected set; }

        /// <summary>
        /// How much mana the fighter regenerates per turn.
        /// </summary>
        public double RegenerationRate { get; protected set; }

        /// <summary>
        /// The amount of damage dealt to an enemy by the fighter by a regular strike.
        /// </summary>
        public double Strength { get; protected set; }

        /// <summary>
        /// Calculates damage to the fighter based on the fighter's armor.
        /// Returns a tuple (HealthDamage, Armor Damage)
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>HealthDMG,ArmorDMG</returns>
        virtual public Tuple<double,double> ArmoredDamage(double damage)
        {
            double halfDamage = 0.5 * damage;
            double armorDamage =  0.375 * halfDamage;
            if(armorDamage > Armor)
            {
                double overflow = (armorDamage - Armor) / 0.375;
                damage = halfDamage + armorDamage;
                armorDamage = Armor;
            }
            return new Tuple<double, double>(damage,armorDamage);
        }

        /// <summary>
        /// A list of effects that are applied to the fighter when the fighter updates.
        /// </summary>
        protected List<Effect> FighterEffects;

        /// <summary>
        /// A list of a fighter's available moves
        /// </summary>
        public List<Move> AvailableMoves;

        /// <summary>
        /// Deal flat damage to the fighter, health -= damage
        /// </summary>
        virtual public void Damage(double damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Runs once per turn, after damage is resolved, to replenish mana
        /// </summary>
        virtual public void ManaUpdate()
        {
            Mana = Mana + RegenerationRate;
        }

        /// <summary>
        /// Ticks all effects on the fighter, and removes any expired effects
        /// </summary>
        virtual public void EffectUpdate()
        {
            List<Effect> ExpiredEffects = new List<Effect>();
            foreach(Effect e in FighterEffects)
            {
                e.Tick();

                if (e.Lifespan <= 0)
                {
                    ExpiredEffects.Add(e);
                    continue;
                }
            }

            foreach(Effect e in ExpiredEffects)
            {
                FighterEffects.Remove(e);
            }
        }

        /// <summary>
        /// Expend the mana cost for the move, and applies user effects
        /// </summary>
        public void PerformMove(Move m)
        {
            Mana -= m.FlatManaCost;
            if(m.AdditionalUserEffects.Count != 0)
            {
                foreach(Effect e in m.AdditionalUserEffects)
                {
                    AddEffect(e);
                }
            }
        }
        
        /// <summary>
        /// Adds an effect to the fighter
        /// </summary>
        public void AddEffect(Effect e)
        {
            FighterEffects.Add(e);
        }
    }

   

   
}
