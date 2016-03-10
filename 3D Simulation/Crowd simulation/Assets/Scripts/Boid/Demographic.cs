using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.Scripts.Boid {
    class DemographicAttr : Attribute {

        public float movespeed { get; private set; }
        public float bladderSize { get; private set; }
        public float thirstiness { get; private set; }
        public float danciness { get; private set; }

        internal DemographicAttr(float movespeed, float bladderSize, float thirstiness, float danciness) {
            this.movespeed = movespeed;
            this.bladderSize = bladderSize;
            this.thirstiness = thirstiness;
            this.danciness = danciness;
        }
    }

    public static class Demographics {

        public static float GetMovespeed(this Demographic d) {
            return GetAttr(d).movespeed;
        }

        public static float GetBladderSize(this Demographic d) {
            return GetAttr(d).bladderSize;
        }

        public static float GetThirstiness(this Demographic d) {
            return GetAttr(d).bladderSize;
        }

        public static float GetDanciness(this Demographic d) {
            return GetAttr(d).bladderSize;
        }

        private static DemographicAttr GetAttr(Demographic d) {
            return (DemographicAttr) Attribute.GetCustomAttribute(ForValue(d), typeof(DemographicAttr));
        }

        private static MemberInfo ForValue(Demographic d) {
            return typeof(Demographic).GetField(Enum.GetName(typeof(Demographic), d));
        }

    }

    public enum Demographic {
        [DemographicAttr(10f, 0.5f, 4, 5)]DEFAULT,
        [DemographicAttr(5f, 0.5f, 4, 5)]SLOW,
        [DemographicAttr(13f, 2.5f, 4, 5)]INCONTENENT,
        [DemographicAttr(13f, 0.2f, 6, 3)]ALCOHOLIC,
        [DemographicAttr(13f, 0.5f, 3, 7)]DANCER,

        
    }
}
