using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    /// <summary>
    /// Moves are chosen every turn. Each fighter has an array of moves available to them.
    /// </summary>
    public abstract class Move
    {
        /// <summary>
        /// The base constructor for Move
        /// </summary>
        public Move(Fighter user)
        {
            User = user;
            FlatDamage = 0;
            FlatDefense = 0;
            FlatManaCost = 0;
        }

        /// <summary>
        /// The user of the move
        /// </summary>
        public Fighter User { get; protected set; }

        /// <summary>
        /// The expected damage of the move, assuming an enemy defense of 0 and no modifiers
        /// Used for Artificial Intelligence
        /// </summary>
        public double FlatDamage { get; protected set; }

        /// <summary>
        /// The expected maximum defense, i.e. the amount of damage that will be prevented
        /// Used for Artificial Intelligence
        /// </summary>
        public double FlatDefense { get; protected set; }

        /// <summary>
        /// The expected mana cost of the move
        /// </summary>
        public double FlatManaCost { get; protected set; }

        /// <summary>
        /// A list of delegates that determines additional effects on the user of the move
        /// </summary>
        public List<Effect> AdditionalUserEffects;

        /// <summary>
        /// A list of delegates that determines additional effects on the receiver of the move
        /// </summary>
        public List<Effect> AdditionalReceiverEffects;
    }

    public class Block : Move
    {
        public Block(Fighter user) : base(user)
        {
            FlatDefense = user.Armor;
        }
    }

    public class Attack : Move
    {
        public Attack(Fighter user) : base(user)
        {
            FlatDamage = User.Strength;
        }
    }

    public class Poison : Move
    {
        public Poison(Fighter user, Poisoned poisonEffect, double manaCost) : base(user)
        {
            FlatDamage = (poisonEffect.Lifespan) * poisonEffect.Strength;
            AdditionalReceiverEffects.Add(poisonEffect);
            FlatManaCost = manaCost;
        }
    }
}
