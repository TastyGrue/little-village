using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColosseumFoundation
{
    /// <summary>
    /// Moves are chosen every turn.
    /// Each fighter has an array of moves available to them.
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
        /// A predicted damage value of the move.
        /// Used for Artificial Intelligence.
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
        /// A list of delegates that describes additional effects
        /// applied to the user of the move.
        /// </summary>
        public List<Effect> AdditionalUserEffects;

        /// <summary>
        /// A list of delegates that describes additional effects
        /// applied to the receiver of the move.
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
}
