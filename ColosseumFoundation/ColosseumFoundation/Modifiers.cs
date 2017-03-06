using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    public abstract class Modifier : IComparable
    {
        public int Priority { get; protected set; }
        public abstract int CompareTo(object obj);

        public delegate double ModifierDelegate(double input);
        public ModifierDelegate Delegate { get; protected set; }
    }

    public class ModifierList
    {
        protected List<Modifier> modifiers;
        public int Count
        {
            get
            {
               return modifiers.Count;
            }
        }

        public void AddModifier(Modifier mod)
        {
            modifiers.Add(mod);
            modifiers.Sort();
        }

        /// <summary>
        /// Calculates an output of all modifiers in sequence given an input
        /// </summary>
        public double Calculate(double input)
        {
            double output = 0;
            foreach(Modifier mod in modifiers)
            {
                output = mod.Delegate(input);
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
            double output = 0;
            foreach(Modifier mod in modifiers)
            {
                if(mod.Priority >= minRange)
                {
                    output = mod.Delegate(input);
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
            double output = 0;
            foreach (Modifier mod in modifiers)
            {
                if (mod.Priority < maxRange)
                {
                    output = mod.Delegate(input);
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
            double output = 0;
            foreach (Modifier mod in modifiers)
            {
                if (mod.Priority < maxRange && mod.Priority >= minRange)
                {
                    output = mod.Delegate(input);
                }
            }
            return output;
        }

        public void Clear()
        {
            modifiers.Clear();
        }
    }

    /// <summary>
    /// Wrapper class for a modifier to modify damage dealt.
    /// Priority takes place in order from lowest to highest.
    /// 
    /// For convention, pre-armor damage modification is before 0,
    ///     post-armor damage modification is after 0
    /// </summary>
    public class DamageModifier : Modifier
    {
        public DamageModifier(ModifierDelegate mod, int priority)
        {
            Delegate = mod;
            Priority = priority;
        }

        public override int CompareTo(object obj)
        {
            return Priority.CompareTo(obj);
        }
    }
}
