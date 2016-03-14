using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.Scripts.Boid {
    class DemographicAttr : Attribute {

        public float movespeed { get; private set; }
        public float ToiletNeedRate { get; private set; }
        public float DrinkNeedRate { get; private set; }
        public float DanceNeedRate { get; private set; }

        internal DemographicAttr(float movespeed, float toiletNeedRate, float drinkNeedRate, float danceNeedRate) {
            this.movespeed = movespeed;
            this.ToiletNeedRate = toiletNeedRate;
            this.DrinkNeedRate = drinkNeedRate;
            this.DanceNeedRate = danceNeedRate;
        }
    }

    public static class Demographics {

        public static float GetMovespeed(this Demographic d) {
            return GetAttr(d).movespeed;
        }

        public static float GetToiletNeedRate(this Demographic d) {
            return GetAttr(d).ToiletNeedRate;
        }

        public static float GetDrinkNeedRate(this Demographic d) {
            return GetAttr(d).ToiletNeedRate;
        }

        public static float GetDanceNeedRate(this Demographic d) {
            return GetAttr(d).ToiletNeedRate;
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
