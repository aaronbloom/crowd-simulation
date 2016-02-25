using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class BoidProperties {
        private readonly float _genderBias;

        public string HumanName { get; private set; }
        public Gender Gender { get; private set; }

        public BoidProperties(float genderBias) {
            _genderBias = genderBias;
            Gender = getRandomGender();
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
