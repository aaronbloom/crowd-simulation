using UnityEngine;

namespace Assets.Scripts.Boid {
    public class Boid {

        private static readonly string[] MalePrefab = { "chr_mike", "chr_bro", "chr_beardo2" };
        private static readonly string[] FemalePrefab = { "chr_brookie", "chr_bridget", "chr_goth2" };

        private readonly GameObject gameObject;
        private readonly Mind mind;
        public Transform Transform { get { return gameObject.transform; } }
        public Vector3 Position { get { return gameObject.transform.position; } }
        public string Name { get { return gameObject.name; } }
        public float Thirst { get { return mind.Thirst; } }
        public float ToiletNeed { get { return mind.ToiletNeed; } }
        public float DanceNeed { get { return mind.DanceNeed; } }
        public string CurrentNeed { get { return mind.CurrentNeed.MindState.ToString(); } }
        private Vector3 _velocity;
        public Vector3 Velocity {
            get { return _velocity; }
        }

        public float ViewingDistance = 20f;
        public float MinimumDistance = 4.5f;
        public readonly Vector3 EyeHeight = new Vector3(0, 2, 0);


        public BoidBehaviour behaviour; //Set as protected, so can namespace behaviour access.
        public BoidProperties Properties { get; private set; }
        public BoidStatistics Statistics { get; private set; }
        private Vector3 acceleration;

        public static Boid Spawn(Vector3 position, float genderBias = 0.5f) {
            position.y = 0.1f;
            return new Boid(position, genderBias);
        }

        public void LookAt(Vector3 position) {
            gameObject.transform.LookAt(position);
        }


        private Boid(Vector3 position, float genderBias) {
            Properties = new BoidProperties(genderBias);
            Statistics = new BoidStatistics();

            int index = Random.Range(0, 3);
            string boidPrefab = Properties.Gender == Gender.MALE ? MalePrefab[index] : FemalePrefab[index];
            gameObject = BootStrapper.Initialise("mmmm/" + boidPrefab, position, Quaternion.identity) as GameObject;


            behaviour = new GoalSeekingBehaviour(this);
            mind = new Mind(this);
            _velocity = behaviour.InitialVelocity();


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
            this._velocity = calculateVelocity(this._velocity);
            var oldPosition = gameObject.transform.position;
            var newPosition = gameObject.transform.position + (this._velocity * Time.deltaTime);
            newPosition = new Vector3(newPosition.x, 0, newPosition.z);
            gameObject.transform.position = newPosition;
            this.Statistics.LogDistance(Vector3.Distance(oldPosition, newPosition));
        }

        private Vector3 calculateAcceleration(Vector3 acceleration) {
            return acceleration += this.behaviour.updateAcceleration();
        }

        private Vector3 calculateVelocity(Vector3 velocity) {
            velocity *= this.behaviour.VelocityDamping;
            velocity += acceleration;
            velocity = Vector3.ClampMagnitude(velocity, Properties.MoveSpeed);
            velocity.y = 0;
            return velocity;
        }

        private void resetAcceleration() {
            this.acceleration = Vector3.zero;
        }

        private void faceTravelDirection() {
            if (this._velocity != Vector3.zero) {
                gameObject.transform.rotation = Quaternion.LookRotation(this._velocity);
            }
        }

        public bool HasGameObject(GameObject other) {
            return this.gameObject.Equals(other);
        }
    }
}
