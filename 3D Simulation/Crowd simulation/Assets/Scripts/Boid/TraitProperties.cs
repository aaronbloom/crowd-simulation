using System;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    /// <summary>
    /// Holds the properties of each trait a boid may posess.
    /// </summary>
    public class TraitProperties {

        private TraitType traitType;

        public float MoveSpeed { get; private set; }
        public float ToiletNeedRate { get; private set; }
        public float DrinkNeedRate { get; private set; }
        public float DanceNeedRate { get; private set; }

        public override string ToString() {
            return traitType.ToString();
        }

        /// <summary>
        /// Constructs a TraitProperties object with the specified trait type
        /// </summary>
        /// <param name="type"> the trait type to get properties for</param>
        public TraitProperties(TraitType type) {
            UpdateType(type);
        }

        /// <summary>
        /// Constructs a TraitProperties object with the specified trait type
        /// </summary>
        /// <param name="type"> the trait type to get properties for</param>
        public TraitProperties(int type) {
            UpdateType((TraitType) type);
        }

        /// <summary>
        /// Constructs a TraitProperties object with a random trait type
        /// </summary>
        public TraitProperties() {
            UpdateType((TraitType) Random.Range(0, 5));
        }

        /// <summary>
        /// Updates the current trait type to a specified trait
        /// </summary>
        /// <param name="type"> the trait type to update to</param>
        public void UpdateType(TraitType type) {
            traitType = type;
            setDefaultValues();

            switch (traitType) {
                case TraitType.Default:
                    //Do Nothing
                    break;
                case TraitType.Slow:
                    MoveSpeed = 3f;
                    break;
                case TraitType.Incontenent:
                    MoveSpeed = 11f;
                    ToiletNeedRate = 1.5f;
                    break;
                case TraitType.Alcoholic:
                    MoveSpeed = 8f;
                    DrinkNeedRate = 6;
                    break;
                case TraitType.Dancer:
                    MoveSpeed = 14f;
                    DanceNeedRate = 7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Sets the default values for traits
        /// </summary>
        private void setDefaultValues() {
            MoveSpeed = 10f;
            ToiletNeedRate = 0.4f;
            DrinkNeedRate = 4;
            DanceNeedRate = 5;
        }
    }

    public enum TraitType {
        Default,
        Slow,
        Incontenent,
        Alcoholic,
        Dancer
    }
}
