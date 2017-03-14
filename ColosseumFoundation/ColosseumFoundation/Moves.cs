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
        public Move(Fighter user, double SpeedCost)
        {
            User = user;
            FlatDamage = 0;
            FlatManaCost = 0;
            FlatSpeedCost = SpeedCost;
            Name = GetType().ToString();
            AdditionalReceiverEffects = new List<Effect>();
            AdditionalUserEffects = new List<Effect>();
        }

        /// <summary>
        /// The user of the move
        /// </summary>
        public Fighter User { get; protected set; }

        /// <summary>
        /// The name of the move
        /// </summary>
        public String Name { get; protected set; }

        /// <summary>
        /// An initial damage value of the move.
        /// </summary>
        public double FlatDamage { get; protected set; }

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
        public Attack(Fighter user, double speedCost) : base(user,speedCost)
        {
            FlatDamage = User.Strength;
            Name = "Basic Attack";
        }
    }
}
