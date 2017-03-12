using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    /// <summary>
    /// Effects affect fighters for a set amount of ticks, and perfrom small
    /// actions every tick with Tick().
    /// </summary>
    public abstract class Effect
    {
        /// <summary>
        /// Constructor for Effect, takes the lifespan of the effect and defines
        /// InitPreCalcDmg.
        /// </summary>
        public Effect(int lifespan, bool InitialCalculationBoolean)
        {
            Lifespan = lifespan;
            this.InitialCalculationBoolean = InitialCalculationBoolean;
        }

        /// <summary>
        /// Whether this effect, when it is initially applied to a fighter, should be
        /// applied before (true) or after (false) damage and move success is calculated.
        /// </summary>
        public bool InitialCalculationBoolean { get; }

        protected Fighter affected = null;

        /// <summary>
        /// Attaches a fighter to the effect. Returns false if the fighter is not
        /// successfully attached.
        /// </summary>
        public bool AttachFighter(Fighter fighter)
        {
            if (affected == null)
            {
                affected = fighter;
                return true;
            }
            else
            {
                return affected == fighter;
            }
        }

        /// <summary>
        /// A wrapper method that "ticks" the effect, guarantees lifespan
        /// being expended.
        /// </summary>
        public virtual void Tick()
        {
            if (affected != null)
                Lifespan--;
        }

        /// <summary>
        /// The number of turns that an effect lasts.
        /// </summary>
        public int Lifespan;
    }

    public abstract class ActiveEffect : Effect
    {
        public ActiveEffect(int lifespan, bool initCalcBool) : base(lifespan, initCalcBool)
        {
        }

        public override void Tick()
        {
            base.Tick();
            if(affected != null)
                TickEffect();
        }

        /// <summary>
        /// The actual effect's tick. This is where all adjustments and
        /// actions on affected fighter occur.
        /// </summary>
        abstract protected void TickEffect();
    }
}
