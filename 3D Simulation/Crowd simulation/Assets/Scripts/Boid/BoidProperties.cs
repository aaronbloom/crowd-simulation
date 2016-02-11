using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class BoidProperties {

        public string HumanName { get; private set; }
        public Gender Gender { get; private set; }

        public BoidProperties() {
            Gender = getRandomGender();
            HumanName = NameGenerator.GenerateFairlyUniqueName(Gender);
        }

        public BoidProperties(Gender gender) {
            Gender = gender;
        }

        private Gender getRandomGender() {
            int value = Random.Range(0, 2);
            switch (value) {
                case 0: return Gender.MALE;
                case 1: return Gender.FEMALE;
                default: return Gender.UNDEFINED;
            }
        }

    }
}
