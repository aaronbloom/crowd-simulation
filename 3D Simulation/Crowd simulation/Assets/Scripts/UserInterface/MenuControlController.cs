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

        /// <summary>
        /// Updates value of the number of boids slider
        /// </summary>
        /// <param name="value">new value</param>
        public void NumberOfBoidsTextUpdate(float value) {
            NumberOfBoidsSliderText.text = value.ToString(CultureInfo.InvariantCulture);
            this.NumberOfBoidsValue = (int) value;
        }

        /// <summary>
        /// Updates value of the gender slider
        /// </summary>
        /// <param name="value">new value</param>
        public void SetGenderBias(float value) {
            GenderBiasSliderText.text = value.ToString(CultureInfo.InvariantCulture) + '%';
            this.GenderBiasValue = value;
        }

        /// <summary>
        /// updates environment Size dropdown
        /// </summary>
        /// <param name="value">new value</param>
        public void EnvironmentSizeUpdate(int value) {
            environmentSizeValue = value;
        }
    }
}
