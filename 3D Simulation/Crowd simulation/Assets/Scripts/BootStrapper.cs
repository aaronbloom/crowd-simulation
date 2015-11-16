using UnityEngine;

    class Bootstrapper : MonoBehaviour {

        //System properties
        int boidPopulation = 100;

        //System Fields
        private BoidManager boidManager;

        void Awake() {

        }

        void Start() {
            boidManager = new BoidManager(boidPopulation);
            boidManager.SpawnBoids();
        }

    }

