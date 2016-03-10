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
        public Demographic Demographic { get; private set; }

        public float speed { get { return Demographic.GetMovespeed(); } }
        public float BladderSize { get { return Demographic.GetBladderSize(); } }
        public float Thirstiness { get { return Demographic.GetThirstiness(); } }
        public float Danciness { get { return Demographic.GetDanciness(); } }

        public BoidProperties(float genderBias) {
            _genderBias = genderBias;
            Gender = getRandomGender();
            Demographic = (Demographic) Random.Range(0, 5);
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
