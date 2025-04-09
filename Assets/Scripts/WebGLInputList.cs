using TMPro;
using UnityEngine;

public class WebGLInputList : MonoBehaviour
{
    public TMP_InputField[] inputFields;
    private TouchScreenKeyboard[] _keyboards;
    void Start()
    {
        _keyboards = new TouchScreenKeyboard[inputFields.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].isFocused && (_keyboards[i] == null || !_keyboards[i].active))
            {
                _keyboards[i] = TouchScreenKeyboard.Open(inputFields[i].text, TouchScreenKeyboardType.Default);
            }

            if (_keyboards[i] != null && _keyboards[i].active)
            {
                inputFields[i].text = _keyboards[i].text;
            }
        }
    }
}
