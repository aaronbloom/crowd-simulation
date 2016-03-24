using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface
{
    public class MenuControlController : MonoBehaviour {

        public Text NumberOfBoidsSliderText;
        public Text GenderBiasSliderText;
        public int NumberOfBoidsValue;
        public float GenderBiasValue;
        private int environmentSizeValue = 1;
        public int EnvironmentSizeValue {
            get { return (environmentSizeValue + 1)*40; }
        }

        public void NumberOfBoidsTextUpdate(float value) {
            NumberOfBoidsSliderText.text = value.ToString(CultureInfo.InvariantCulture);
            this.NumberOfBoidsValue = (int) value;
        }

        public void SetGenderBias(float value) {
            GenderBiasSliderText.text = value.ToString(CultureInfo.InvariantCulture) + '%';
            this.GenderBiasValue = value;
        }

        public void EnvironmentSizeUpdate(int value) {
            environmentSizeValue = value;
        }
    }
}
