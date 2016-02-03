using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    class BoidProperties {

        private Gender _gender;
        public Gender Gender {
            get { return _gender; }
        }

        public BoidProperties() {
            this._gender = getRandomGender();
        }

        public BoidProperties(Gender gender) {
            this._gender = gender;
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
