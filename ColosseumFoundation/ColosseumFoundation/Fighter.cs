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
        public Fighter(double totalHealth, double totalMana, double totalSpeed)
        {
            Health = (MaxHealth = totalHealth);
            Mana = (MaxMana = totalMana);
            Speed = (MaxSpeed = totalSpeed);
            AvailableMoves = new List<Move> { new Attack(this, 10) };
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
        /// The maximum health of the fighter
        /// </summary>
        public double MaxHealth { get; protected set; }

        /// <summary>
        /// The mana of the fighter; spent when doing special moves. Regenerates.
        /// </summary>
        public double Mana { get; protected set; }

        /// <summary>
        /// The maximum mana of the fighter
        /// </summary>
        public double MaxMana { get; protected set; }

        /// <summary>
        /// The speed of the fighter.
        /// </summary>
        public double Speed { get; protected set; }

        /// <summary>
        /// Maximum speed of the fighter
        /// </summary>
        public double MaxSpeed { get; protected set; }

        /// <summary>
        /// How much mana the fighter regenerates per turn.
        /// </summary>
        public double RegenerationRate { get; protected set; }

        /// <summary>
        /// The amount of damage dealt to an enemy by the fighter by a regular strike.
        /// </summary>
        public double Strength { get; protected set; }

        /// <summary>
        /// Calculates damage to the fighter based on the fighter's available speed.
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
        protected List<ActiveEffect> FighterEffects = new List<ActiveEffect>();

        /// <summary>
        /// A list of a fighter's available moves
        /// </summary>
        public List<Move> AvailableMoves = new List<Move>();

        /// <summary>
        /// A list of modifiers this fighter applies to damage directed
        /// at them
        /// </summary>
        protected PassiveList SelfDamageModifiers = new PassiveList();

        /// <summary>
        /// A list of modifiers this fighter applies to any move performed by them
        /// </summary>
        protected PassiveList SelfMoveModifiers = new PassiveList();

        /// <summary>
        /// A list of damage modifiers this fighter applies to damage dealt
        /// </summary>
        protected PassiveList OutDamageModifiers = new PassiveList();

        /// <summary>
        /// Deal flat damage to the fighter, health -= damage
        /// </summary>
        public void HealthDamage(double damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Damage the fighter applying damage modifiers (on both fighter and attacker)
        /// to the damage. 
        /// </summary>
        public void Damage(double damage, PassiveList DamageModifiers)
        {
            PassiveList DamageModifierClone = (PassiveList)DamageModifiers.Clone();

            foreach (PassiveEffect mod in SelfDamageModifiers)
            {
                DamageModifierClone.Add(mod);
            }
            damage = DamageModifierClone.CalculateMax(damage, 0);
            Tuple<double, double> pair = SpeedDamage(damage);
            Speed -= pair.Item2;
            damage = DamageModifierClone.CalculateMin(pair.Item1, 0);
            HealthDamage(damage);
        }

        /// <summary>
        /// Damage the fighter directly, using only the fighter's own damage modifiers
        /// </summary>
        public void Damage(double damage)
        {
            damage = SelfDamageModifiers.CalculateMax(damage, 0);
            Tuple<double, double> pair = SpeedDamage(damage);
            Speed -= pair.Item2;
            damage = SelfDamageModifiers.CalculateMin(pair.Item1, 0);
            HealthDamage(damage);
        }

        /// <summary>
        /// Runs once per turn, after damage is resolved, to replenish mana
        /// </summary>
        virtual public void ManaUpdate()
        {
            Mana = Mana + RegenerationRate;
        }

        /// <summary>
        /// Refills speed
        /// </summary>
        virtual public void SpeedUpdate()
        {
            Speed = MaxSpeed;
        }

        /// <summary>
        /// Ticks all effects on the fighter, and then removes any expired effects.
        /// Also clears all the fighter's modifier lists beforehand. Any modifiers left
        /// afterwards are directly from the effects.
        /// </summary>
        virtual public void EffectUpdate()
        {
            List<ActiveEffect> ExpiredEffects = new List<ActiveEffect>();
            foreach (ActiveEffect e in FighterEffects)
            {
                e.Tick();

                if (e.Lifespan <= 0)
                {
                    ExpiredEffects.Add(e);
                    continue;
                }
            }

            foreach (ActiveEffect e in ExpiredEffects)
            {
                FighterEffects.Remove(e);
            }
        }

        /// <summary>
        /// Processes a moveset performed by this fighter
        /// </summary>
        public void ProcessMoveset(Moveset Actions)
        {
            Tuple<Move,Fighter>[] actions = Actions.Moves;
            
            foreach(Tuple<Move,Fighter> act in actions)
            {
                // If the move is successful
                if(PerformMove(act.Item1))
                {
                    // Add the effects of the move to the user
                    foreach(Effect e in act.Item1.AdditionalUserEffects)
                    {
                        this.AddEffect(e);
                    }

                    // Execute the move on target
                    act.Item2.ReceiveMove(act.Item1.AdditionalReceiverEffects, OutDamageModifiers, act.Item1.FlatDamage);
                }
            }
        }

        /// <summary>
        /// Process a move this fighter is the target of.
        /// </summary>
        protected void ReceiveMove(List<Effect> ReceivedEffects, PassiveList DamagingMods, double initDmg)
        {
            // Add pertinent pre-damage modifiers
            foreach(Effect e in ReceivedEffects)
            {
                if(e.InitialCalculationBoolean == true)
                {
                    if(e is PassiveEffect)
                    {
                        switch(((PassiveEffect)e).IOType)
                        {
                            case PassiveEffect.ModType.Weakness:
                                SelfDamageModifiers.Add((PassiveEffect)e);
                                break;
                            case PassiveEffect.ModType.Buff:
                                SelfDamageModifiers.Add((PassiveEffect)e);
                                break;
                            case PassiveEffect.ModType.Move:
                                SelfMoveModifiers.Add((PassiveEffect)e);
                                break;
                        }
                    }
                }
            }

            Damage(initDmg, DamagingMods);

            foreach(Effect e in ReceivedEffects)
            {
                AddEffect(e);
            }
        }

        /// <summary>
        /// Expend the mana cost for the move, and applies user effects
        /// </summary>
        protected bool PerformMove(Move m)
        {
            if (m.AdditionalUserEffects.Count != 0)
            {
                foreach (Effect e in m.AdditionalUserEffects)
                {
                    if (e.InitialCalculationBoolean)
                    {
                        if (e is PassiveEffect && ((PassiveEffect)e).IOType == PassiveEffect.ModType.Move)
                        {
                            SelfMoveModifiers.Add((PassiveEffect)e);
                        }
                    }
                }
            }

            Mana -= m.FlatManaCost;
            Speed -= m.FlatSpeedCost;

            double chance = 1;

            chance = SelfMoveModifiers.Calculate(chance);

            if (FighterRNG.NextDouble() > chance)
                return false;

            return true;
        }

        /// <summary>
        /// Adds an effect to the fighter, if its InitialCalculationBoolean is false or if it is an active effect
        /// </summary>
        public bool AddEffect(Effect e)
        {
            if(e is ActiveEffect)
            {
                FighterEffects.Add((ActiveEffect)e);
                return true;
            }
            else if(e is PassiveEffect && !e.InitialCalculationBoolean)
            {
                if(((PassiveEffect)e).IOType == PassiveEffect.ModType.Weakness)
                {
                    SelfDamageModifiers.Add((PassiveEffect)e);
                    return true;
                }
                else if (((PassiveEffect)e).IOType == PassiveEffect.ModType.Move)
                {
                    SelfMoveModifiers.Add((PassiveEffect)e);
                    return true;
                }
                else if (((PassiveEffect)e).IOType == PassiveEffect.ModType.Buff)
                {
                    OutDamageModifiers.Add((PassiveEffect)e);
                    return true;
                }
            }
            return false;
        }
    }

    public class Moveset
    {
        public Tuple<Move,Fighter>[] Moves
        {
            get
            {
                Tuple<Move,Fighter>[] output = new Tuple<Move,Fighter>[moves.Count];
                moves.CopyTo(output, 0);
                return output;
            }
        }

        Stack<Tuple<Move,Fighter>> moves;

        double Speed;

        public Moveset(double AvailableSpeed)
        {
            Speed = AvailableSpeed;
        }

        public bool Push(Tuple<Move,Fighter> moveTuple)
        {
            double usedSpeed = 0;
            foreach (Tuple<Move,Fighter> tup in moves)
            {
                usedSpeed += tup.Item1.FlatSpeedCost;
            }

            if (moveTuple.Item1.FlatSpeedCost + usedSpeed > Speed)
            {
                return false;
            }
            else
            {
                moves.Push(moveTuple);
                return true;
            }
        }

        public bool Pop()
        {
            if (moves.Count != 0)
            {
                moves.Pop();
                return true;
            }
            else
            {
                return false;
            }
        }
    }




}
