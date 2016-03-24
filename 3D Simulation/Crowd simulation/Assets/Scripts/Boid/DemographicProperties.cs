using System;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {

    public class DemographicProperties {

        private DemographicType demographicType;

        public float MoveSpeed { get; private set; }
        public float ToiletNeedRate { get; private set; }
        public float DrinkNeedRate { get; private set; }
        public float DanceNeedRate { get; private set; }

        public override string ToString() {
            return demographicType.ToString();
        }

        public DemographicProperties(DemographicType type) {
            updateType(type);
        }

        public DemographicProperties(int type) {
            updateType((DemographicType) type);
        }

        public DemographicProperties() {
            updateType((DemographicType) Random.Range(0, 5));
        }

        public void updateType(DemographicType type) {
            demographicType = type;
            setDefaultValues();

            switch (demographicType) {
                case DemographicType.DEFAULT:
                    //Do Nothing
                    break;
                case DemographicType.SLOW:
                    MoveSpeed = 3f;
                    break;
                case DemographicType.INCONTENENT:
                    MoveSpeed = 11f;
                    ToiletNeedRate = 1.5f;
                    break;
                case DemographicType.ALCOHOLIC:
                    MoveSpeed = 8f;
                    DrinkNeedRate = 6;
                    break;
                case DemographicType.DANCER:
                    MoveSpeed = 14f;
                    DanceNeedRate = 7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void setDefaultValues() {
            MoveSpeed = 10f;
            ToiletNeedRate = 0.4f;
            DrinkNeedRate = 4;
            DanceNeedRate = 5;
        }
    }

    public enum DemographicType {
        DEFAULT,
        SLOW,
        INCONTENENT,
        ALCOHOLIC,
        DANCER
    }
}
