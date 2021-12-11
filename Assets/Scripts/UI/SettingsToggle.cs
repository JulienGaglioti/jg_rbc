using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    public Image image;
    public Color toggledColor;
    public List<GameObject> objects;
    private bool _isToggled;

    public void Toggle()
    {
        if (_isToggled)
        {
            _isToggled = false;
            image.color = Color.white;
            foreach (var obj in objects)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            _isToggled = true;
            image.color = toggledColor;
            foreach (var obj in objects)
            {
                obj.SetActive(true);
            }
        }
    }
}
