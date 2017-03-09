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
            AIDamage = 0;
            FlatManaCost = 0;
            FlatSpeedCost = 0;
        }

        /// <summary>
        /// The user of the move
        /// </summary>
        public Fighter User { get; protected set; }

        /// <summary>
        /// The expected damage of the move, assuming an enemy defense of 0 and no modifiers
        /// Used for Artificial Intelligence
        /// </summary>
        public double AIDamage { get; protected set; }

        /// <summary>
        /// The expected mana cost of the move
        /// </summary>
        public double FlatManaCost { get; protected set; }

        /// <summary>
        /// The expected speed cost of the move
        /// </summary>
        public double FlatSpeedCost { get; protected set; }

        /// <summary>
        /// A list of delegates that determines additional effects on the user of the move.
        /// The boolean determines whether the user triggers the effect instantly (true) or afterwards (false).
        /// </summary>
        public List<Tuple<bool,Effect>> AdditionalUserEffects;

        /// <summary>
        /// A list of delegates that determines additional effects on the receiver of the move
        /// </summary>
        public List<Effect> AdditionalReceiverEffects;
    }

    public class Attack : Move
    {
        public Attack(Fighter user, double speedCost) : base(user)
        {
            AIDamage = User.Strength;
        }
    }

    public class Poison : Move
    {
        public Poison(Fighter user, Poisoned poisonEffect, double baseManaCost, double baseSpeedCost) : base(user)
        {
            AIDamage = (poisonEffect.Lifespan) * poisonEffect.Strength;
            AdditionalReceiverEffects.Add(poisonEffect);
            FlatManaCost = baseManaCost;
            FlatSpeedCost = baseSpeedCost;
        }
    }

    public class Blind : Move
    {
        public Blind(Fighter user, Blinded blindEffect, double baseManaCost, double baseSpeedCost) : base(user)
        {
            AdditionalReceiverEffects.Add(blindEffect);
            FlatManaCost = baseManaCost;
            FlatSpeedCost = baseSpeedCost;
        }
    }

    public class Fireball : Move
    {
        public Fireball(Fighter user, double baseManaCost, double potency) : base(user)
        {
            user.AddModifier(new DamageModifier(x => x + potency, 0),Fighter.Modifications.OutDamage);
            AIDamage = potency;
            FlatManaCost = baseManaCost;
        }
    }
}
