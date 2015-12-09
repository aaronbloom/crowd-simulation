using UnityEngine;
using System.Globalization;
using UnityEngine.UI;

public class MenuControlController : MonoBehaviour {
    public Text NumberOfBoidsSliderText;
    public int NumberOfBoidsValue;
    private int _environmentSizeValue = 1;
    public int EnvironmentSizeValue {
        get { return (_environmentSizeValue + 1)*40; }
    }

    public void NumberOfBoidsTextUpdate(float value) {
        NumberOfBoidsSliderText.text = value.ToString(CultureInfo.InvariantCulture);
        this.NumberOfBoidsValue = (int) value;
    }

    public void EnvironmentSizeUpdate(int value) {
        _environmentSizeValue = value;
    }

    void Update() {
    }
}
