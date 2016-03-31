using UnityEngine;

namespace Assets.Scripts.Boid {
    /// <summary>
    /// Holds the properties of a boid.
    /// </summary>
    public class BoidProperties {

        private readonly float genderBias;

        public string HumanName { get; private set; }
        public Gender Gender { get; private set; }
        public TraitProperties TraitProperties { get; private set; }
        public float MoveSpeed { get { return TraitProperties.MoveSpeed; } }
        public float ToiletNeedRate { get { return TraitProperties.ToiletNeedRate; } }
        public float DrinkNeedRate { get { return TraitProperties.DrinkNeedRate; } }
        public float DanceNeedRate { get { return TraitProperties.DanceNeedRate; } }

        /// <summary>
        /// Creates properties for a boid, using a gender bias
        /// </summary>
        /// <param name="genderBias">the % chance that boids will be female</param>
        public BoidProperties(float genderBias) {
            this.genderBias = genderBias;
            Gender = getRandomGender();
            TraitProperties = new TraitProperties();
            HumanName = NameGenerator.GenerateFairlyUniqueName(Gender);
        }

        /// <summary>
        /// Returns a random gender
        /// </summary>
        private Gender getRandomGender() {
            float value = Random.Range(0, 101);
            return value < genderBias ? Gender.Male : Gender.Female;
        }

    }
}
