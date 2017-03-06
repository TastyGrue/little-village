using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    /// <summary>
    /// Effects affect fighters for a set amount of turns, and perfrom small
    /// actions every turn with Tick().
    /// </summary>
    public abstract class Effect
    {
        /// <summary>
        /// Constructor for Effect, takes the fighter affected by the effect, 
        /// and how many turns the effect will last
        /// </summary>
        public Effect(int lifespan)
        {
            Lifespan = lifespan;
        }

        /// <summary>
        /// A wrapper method that "ticks" the effect, guarantees lifespan
        /// being expended
        /// </summary>
        public void Tick()
        {
            Lifespan--;
        }

        /// <summary>
        /// The actual effect's tick. This is where all adjustments and
        /// actions on affected fighter occur
        /// </summary>
        abstract protected void TickEffect(Fighter affected);

        /// <summary>
        /// The number of turns that an effect lasts.
        /// </summary>
        public int Lifespan;
    }

    /// <summary>
    /// The Poisoned effect damages a victim every turn
    /// </summary>
    public class Poisoned : Effect
    {
        /// <summary>
        /// Damages a victim's health directly every turn by the strength.
        /// </summary>
        public Poisoned(int lifespan, double strength) : base(lifespan)
        {
            Strength = strength;
        }

        /// <summary>
        /// The strength of the poison, how much damage is dealt each turn
        /// </summary>
        public double Strength { get; protected set; }

        protected override void TickEffect(Fighter affected)
        {
            affected.Damage(Strength);
        }
    }


    /// <summary>
    /// Reduces the chance of success of any move of the victim succeeding to 25%.
    /// </summary>
    public class Blinded : Effect
    {
        public Blinded(int lifespan) : base(lifespan)
        {

        }

        protected override void TickEffect(Fighter affected)
        {
            
        }
    }
}
