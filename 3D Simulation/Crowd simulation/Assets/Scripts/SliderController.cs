using UnityEngine;
using System.Globalization;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {
    public Text SliderText;
    public int Value;

    public void TextUpdate(float value) {
        SliderText.text = value.ToString(CultureInfo.InvariantCulture);
        this.Value = (int) value;
    }

    void Update() {
    }
}
