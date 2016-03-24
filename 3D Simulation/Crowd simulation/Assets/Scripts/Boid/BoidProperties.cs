using UnityEngine;

namespace Assets.Scripts.Boid {
    public class BoidProperties {
        private readonly float _genderBias;

        public string HumanName { get; private set; }
        public Gender Gender { get; private set; }
        public DemographicProperties DemographicProperties { get; private set; }

        public float MoveSpeed { get { return DemographicProperties.MoveSpeed; } }
        public float ToiletNeedRate { get { return DemographicProperties.ToiletNeedRate; } }
        public float DrinkNeedRate { get { return DemographicProperties.DrinkNeedRate; } }
        public float DanceNeedRate { get { return DemographicProperties.DanceNeedRate; } }

        public BoidProperties(float genderBias) {
            _genderBias = genderBias;
            Gender = getRandomGender();
            DemographicProperties = new DemographicProperties();
            HumanName = NameGenerator.GenerateFairlyUniqueName(Gender);
        }

        public BoidProperties(Gender gender) {
            Gender = gender;
        }

        private Gender getRandomGender() {
            float value = Random.Range(0, 101);
            return value < _genderBias ? Gender.MALE : Gender.FEMALE;
        }

    }
}
