using UnityEngine;

namespace Assets.Scripts.Boid {
    public class Boid {

        private static readonly string[] malePrefab = { "chr_mike", "chr_bro", "chr_beardo2" };
        private static readonly string[] femalePrefab = { "chr_brookie", "chr_bridget", "chr_goth2" };
        public static readonly Vector3 EyeHeight = new Vector3(0, 2, 0);
        public const float ViewingDistance = 20f;
        public const float MinimumDistance = 4.5f;

        public readonly BoidProperties Properties;
        public readonly BoidStatistics Statistics;

        public Transform Transform { get { return gameObject.transform; } }
        public Vector3 Position { get { return gameObject.transform.position; } }
        public string Name { get { return gameObject.name; } }
        public float Thirst { get { return mind.Thirst; } }
        public float ToiletNeed { get { return mind.ToiletNeed; } }
        public float DanceNeed { get { return mind.DanceNeed; } }
        public string CurrentNeed { get { return mind.CurrentNeed.MindState.ToString(); } }
        public Vector3 Velocity { get; private set; }
        public BoidBehaviour Behaviour; //Set as protected, so can namespace Behaviour access.

        private readonly GameObject gameObject;
        private readonly Mind mind;

        private Vector3 acceleration;

        public static Boid Spawn(Vector3 position, float genderBias = 0.5f) {
            position.y = 0.1f;
            return new Boid(position, genderBias);
        }

        public void LookAt(Vector3 position) {
            gameObject.transform.LookAt(position);
        }

        public bool HasGameObject(GameObject other) {
            return this.gameObject.Equals(other);
        }

        private Boid(Vector3 position, float genderBias) {
            Properties = new BoidProperties(genderBias);
            Statistics = new BoidStatistics();

            int index = Random.Range(0, 3);
            string boidPrefab = Properties.Gender == Gender.Male ? malePrefab[index] : femalePrefab[index];
            gameObject = BootStrapper.Initialise("mmmm/" + boidPrefab, position, Quaternion.identity) as GameObject;


            Behaviour = new GoalSeekingBehaviour(this);
            mind = new Mind(this);
            Velocity = Behaviour.InitialVelocity();


        }

        internal void Update() {
            mind.Think();
            if (!BootStrapper.Pause) {
                calculateNewPosition();
            }
            resetAcceleration();
            faceTravelDirection();
        }

        private void calculateNewPosition() {
            this.acceleration = calculateAcceleration(this.acceleration);
            this.Velocity = calculateVelocity(this.Velocity);
            var oldPosition = gameObject.transform.position;
            var newPosition = gameObject.transform.position + (this.Velocity * Time.deltaTime);
            newPosition = new Vector3(newPosition.x, 0, newPosition.z);
            gameObject.transform.position = newPosition;
            this.Statistics.LogDistance(Vector3.Distance(oldPosition, newPosition));
        }

        private Vector3 calculateAcceleration(Vector3 acceleration) {
            return acceleration + this.Behaviour.UpdateAcceleration();
        }

        private Vector3 calculateVelocity(Vector3 velocity) {
            velocity *= this.Behaviour.VelocityDamping;
            velocity += acceleration;
            velocity = Vector3.ClampMagnitude(velocity, Properties.MoveSpeed);
            velocity.y = 0;
            return velocity;
        }

        private void resetAcceleration() {
            this.acceleration = Vector3.zero;
        }

        private void faceTravelDirection() {
            if (this.Velocity != Vector3.zero) {
                gameObject.transform.rotation = Quaternion.LookRotation(this.Velocity);
            }
        }
    }
}
