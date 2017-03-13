using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    public abstract class PassiveEffect : Effect, IComparable
    {
        public PassiveEffect(int priority, int lifespan, bool InitCalcBool) : base(lifespan)
        {
            Priority = priority;
            InitialCalculationBoolean = InitCalcBool;
        }

        /// <summary>
        /// Whether this effect, when it is initially applied to a fighter, should be
        /// applied before (true) or after (false) damage and move success is calculated.
        /// </summary>
        public bool InitialCalculationBoolean { get; }

        public int Priority { get; protected set; }

        public enum ModType { Weakness = 1, Move = 2, Buff = 3 }

        public ModType IOType { get; protected set; }

        public delegate double ModifierDelegate(double input);
        public ModifierDelegate modDelegate { get; protected set; }

        public int CompareTo(object obj)
        {
            return Priority.CompareTo(obj);
        }
    }

    public class QuickProjectile : PassiveEffect
    {
        public QuickProjectile(double strength) : base(1,0,true)
        {
            modDelegate = x => x + strength;
        }
    }

    /// <summary>
    /// A list of passive effects
    /// </summary>
    public class PassiveList : IEnumerable, ICloneable
    {
        protected List<PassiveEffect> modifiers;
        public int Count
        {
            get
            {
                return modifiers.Count;
            }
        }

        public void Add(PassiveEffect PE)
        {
            modifiers.Add(PE);
            modifiers.Sort();
        }

        /// <summary>
        /// Calculates an output of all modifiers in sequence given an input
        /// </summary>
        public double Calculate(double input)
        {
            double output = input;
            foreach (PassiveEffect mod in modifiers)
            {
                output = mod.modDelegate(output);
            }
            return output;
        }

        /// <summary>
        /// Calculates an output for all modifiers in sequence 
        ///     with a priority above minRange, inclusive,
        /// given an input
        /// </summary>
        public double CalculateMin(double input, int minRange)
        {
            double output = input;
            foreach (PassiveEffect mod in modifiers)
            {
                if (mod.Priority >= minRange)
                {
                    output = mod.modDelegate(output);
                }
            }
            return output;
        }

        /// <summary>
        /// Calculates an output for all modifiers in sequence 
        ///     with a priority below maxRange, exclusive,
        /// given an input
        /// </summary>
        public double CalculateMax(double input, int maxRange)
        {
            double output = input;
            foreach (PassiveEffect mod in modifiers)
            {
                if (mod.Priority < maxRange)
                {
                    output = mod.modDelegate(output);
                }
            }
            return output;
        }

        /// <summary>
        /// Calculates an output for all modifiers in sequence 
        ///     with a priority above minRange, inclusive, and
        ///     with a priority below maxRange, exclusive,
        /// given an input
        /// </summary>
        public double CalculateRange(double input, int minRange, int maxRange)
        {
            double output = input;
            foreach (PassiveEffect mod in modifiers)
            {
                if (mod.Priority < maxRange && mod.Priority >= minRange)
                {
                    output = mod.modDelegate(output);
                }
            }
            return output;
        }

        public void Clear()
        {
            modifiers.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)modifiers).GetEnumerator();
        }

        /// <summary>
        /// Shallow clone
        /// </summary>
        public object Clone()
        {
            PassiveList output = new PassiveList();

            foreach(PassiveEffect PE in modifiers)
            {
                output.Add(PE);
            }

            return output;
        }
    }
}