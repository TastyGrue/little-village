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
        public enum Modifications { SelfDamage = 1, SelfMove, OutDamage }

        public Fighter(double totalHealth, double totalMana, double totalSpeed, List<Move> moves)
        {
            Health = totalHealth;
            Mana = totalMana;
            Speed = totalSpeed;
            moves = new List<Move> { new Attack(this) };
        }

        /// <summary>
        /// The fighter's personal RNG
        /// </summary>
        protected Random FighterRNG = new Random();

        /// <summary>
        /// The health of the fighter. Normally considered dead when it hits 0.
        /// </summary>
        public double Health { get; protected set; }

        /// <summary>
        /// The mana of the fighter; spent when doing special moves. Regenerates.
        /// </summary>
        public double Mana { get; protected set; }

        /// <summary>
        /// The speed of the fighter.
        /// </summary>
        public double Speed { get; protected set; }

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
        /// Returns a tuple (Health Damage, Speed Damage)
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>HealthDMG,ArmorDMG</returns>
        virtual public Tuple<double, double> SpeedDamage(double damage)
        {
            double speedDamage = 1.5 * damage;
            double overflow = 0;
            if (speedDamage > Speed)
            {
                overflow = (speedDamage - Speed) / 1.5;
                speedDamage = Speed;
            }
            return new Tuple<double, double>(overflow, speedDamage);
        }

        /// <summary>
        /// A list of effects that are applied to the fighter when the fighter updates.
        /// </summary>
        protected List<Effect> FighterEffects = new List<Effect>();

        /// <summary>
        /// A list of a fighter's available moves
        /// </summary>
        public List<Move> AvailableMoves = new List<Move>();

        /// <summary>
        /// A list of modifiers this fighter applies to damage directed
        /// at them
        /// </summary>
        protected ModifierList SelfDamageModifiers = new ModifierList();

        /// <summary>
        /// A list of modifiers this fighter applies to any move performed by them
        /// </summary>
        protected ModifierList SelfMoveModifiers = new ModifierList();

        /// <summary>
        /// A list of damage modifiers this fighter applies to damage dealt
        /// </summary>
        protected ModifierList OutDamageModifiers = new ModifierList();

        /// <summary>
        /// Deal flat damage to the fighter, health -= damage
        /// </summary>
        public void HealthDamage(double damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Damage the fighter applying damage modifiers (on both fighter and attacker)
        /// to the damage. CLEARS DamageModifiers
        /// </summary>
        public void Damage(double damage, ModifierList DamageModifiers)
        {
            foreach (Modifier mod in SelfDamageModifiers)
            {
                DamageModifiers.AddModifier(mod);
            }
            damage = DamageModifiers.CalculateMax(damage, 0);
            Tuple<double, double> pair = SpeedDamage(damage);
            Speed -= pair.Item2;
            HealthDamage(pair.Item1);
            damage = DamageModifiers.CalculateMin(damage, 0);
            HealthDamage(damage);
            DamageModifiers.Clear();
        }

        /// <summary>
        /// Runs once per turn, after damage is resolved, to replenish mana
        /// </summary>
        virtual public void ManaUpdate()
        {
            Mana = Mana + RegenerationRate;
        }

        /// <summary>
        /// Ticks all effects on the fighter, and then removes any expired effects.
        /// Also clears all the fighter's modifier lists beforehand. Any modifiers left
        /// afterwards are directly from the effects.
        /// </summary>
        virtual public void EffectUpdate()
        {
            OutDamageModifiers.Clear();
            SelfDamageModifiers.Clear();
            SelfMoveModifiers.Clear();

            List<Effect> ExpiredEffects = new List<Effect>();
            foreach (Effect e in FighterEffects)
            {
                e.Tick(this);

                if (e.Lifespan <= 0)
                {
                    ExpiredEffects.Add(e);
                    continue;
                }
            }

            foreach (Effect e in ExpiredEffects)
            {
                FighterEffects.Remove(e);
            }
        }

        /// <summary>
        /// Expend the mana cost for the move, and applies user effects
        /// </summary>
        public bool PerformMove(Move m)
        {
            Mana -= m.FlatManaCost;

            double chance = 1;

            chance = SelfMoveModifiers.Calculate(chance);

            if (FighterRNG.NextDouble() > chance)
                return false;

            if (m.AdditionalUserEffects.Count != 0)
            {
                foreach (Effect e in m.AdditionalUserEffects)
                {
                    AddEffect(e);
                }
            }

            return true;
        }

        /// <summary>
        /// Adds an effect to the fighter
        /// </summary>
        public void AddEffect(Effect e)
        {
            FighterEffects.Add(e);
        }

        /// <summary>
        /// Adds a modifier to the fighter
        /// </summary>
        public void AddModifier(Modifier mod, Modifications m)
        {
            switch (m)
            {
                case Modifications.SelfDamage:
                    SelfDamageModifiers.AddModifier(mod);
                    break;

                case Modifications.SelfMove:
                    SelfMoveModifiers.AddModifier(mod);
                    break;

                case Modifications.OutDamage:
                    OutDamageModifiers.AddModifier(mod);
                    break;
            }
        }
    }




}
