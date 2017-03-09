using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    /// <summary>
    /// An abstract modifier class. Modifiers run algorithm x: input -> x -> output
    /// </summary>
    public abstract class Modifier : IComparable
    {
        public int Priority { get; protected set; }
        public abstract int CompareTo(object obj);

        public delegate double ModifierDelegate(double input);
        public ModifierDelegate Delegate { get; protected set; }
    }

    /// <summary>
    /// A list of modifiers
    /// </summary>
    public class ModifierList : IEnumerable
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
            double output = input;
            foreach(Modifier mod in modifiers)
            {
                output = mod.Delegate(output);
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
            foreach(Modifier mod in modifiers)
            {
                if(mod.Priority >= minRange)
                {
                    output = mod.Delegate(output);
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
            foreach (Modifier mod in modifiers)
            {
                if (mod.Priority < maxRange)
                {
                    output = mod.Delegate(output);
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
            foreach (Modifier mod in modifiers)
            {
                if (mod.Priority < maxRange && mod.Priority >= minRange)
                {
                    output = mod.Delegate(output);
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
    }

    /// <summary>
    /// Wrapper class for a modifier to modify damage dealt.
    /// Priority takes place in order from lowest to highest.
    /// 
    /// For convention, pre-evade damage modification is before 0 (physical, projectile),
    ///     post-evade damage modification is after 0 (armor-piercing, unavoidable)
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

    /// <summary>
    /// Modifies the percent chance of a success, as represented by a double between 0 and 1,
    /// with 1 as success and 0 as failure.
    /// </summary>
    public class MoveModifier : Modifier
    {
        public MoveModifier(ModifierDelegate mod, int priority)
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
