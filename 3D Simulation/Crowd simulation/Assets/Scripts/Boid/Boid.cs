using Assets.Scripts.Environment.Navigation;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class Boid : MonoBehaviour {

        private Vector3 _velocity;
        public Vector3 Velocity {
            get { return _velocity; }
        }

        public BoidBehaviour behaviour; //Set as protected, so can namespace behaviour access.
        private BoidProperties properties; 
        private Vector3 acceleration;

        void Awake() {
            this.behaviour = new GoalSeekingBehaviour(this, 10f, 2.5f);
            this.properties = new BoidProperties();

            MeshRenderer boidRenderer = this.GetComponent<MeshRenderer>();
            boidRenderer.material.mainTexture = Resources.Load("Texture/Boid/" + properties.Gender.ToString()) as Texture;
        }

        void Start() {
            this._velocity = this.behaviour.InitialVelocity();
            Graph graph = BootStrapper.EnvironmentManager.CurrentEnvironment.Graph;
            ((GoalSeekingBehaviour)this.behaviour).chooseNewGoal();
        }

        void Update() {
            calculateNewPosition();
            resetAcceleration();
            faceTravelDirection();
        }

        void OnDrawGizmos() {
            this.behaviour.DrawGraphGizmo();
        }

        private void calculateNewPosition() {
            this.acceleration = calculateAcceleration(this.acceleration);
            this._velocity = calculateVelocity(this._velocity);
            this.transform.position += (this._velocity * Time.deltaTime);
        }

        private Vector3 calculateAcceleration(Vector3 acceleration) {
            return acceleration += this.behaviour.updateAcceleration();
        }

        private Vector3 calculateVelocity(Vector3 velocity) {
            velocity *= this.behaviour.VelocityDamping;
            velocity += acceleration;
            velocity = Vector3.ClampMagnitude(velocity, this.behaviour.MaxSpeed);
            velocity.y = 0;
            return velocity;
        }

        private void resetAcceleration() {
            this.acceleration = Vector3.zero;
        }

        private void faceTravelDirection() {
            this.transform.rotation = Quaternion.LookRotation(this._velocity);
        }
    }
}
