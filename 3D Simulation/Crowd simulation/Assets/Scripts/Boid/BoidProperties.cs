using UnityEngine;

namespace Assets.Scripts.Boid {
    public class BoidProperties {
        private readonly float _genderBias;

        public string HumanName { get; private set; }
        public Gender Gender { get; private set; }
        public TraitProperties TraitProperties { get; private set; }
        public float MoveSpeed { get { return TraitProperties.MoveSpeed; } }
        public float ToiletNeedRate { get { return TraitProperties.ToiletNeedRate; } }
        public float DrinkNeedRate { get { return TraitProperties.DrinkNeedRate; } }
        public float DanceNeedRate { get { return TraitProperties.DanceNeedRate; } }

        public BoidProperties(float genderBias) {
            _genderBias = genderBias;
            Gender = getRandomGender();
            TraitProperties = new TraitProperties();
            HumanName = NameGenerator.GenerateFairlyUniqueName(Gender);
        }

        public BoidProperties(Gender gender) {
            Gender = gender;
        }

        private Gender getRandomGender() {
            float value = Random.Range(0, 101);
            return value < _genderBias ? Gender.Male : Gender.Female;
        }

    }
}
