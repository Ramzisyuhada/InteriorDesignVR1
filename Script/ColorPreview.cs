using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPreview : MonoBehaviour
{
    public Graphic previewGraphic;

    public ColorPicker colorPicker;



    public void OnColorChanged(Color c)
    {
        previewGraphic.color = c;
    }
    private void Update()
    {
        previewGraphic.color = colorPicker.color;
        colorPicker.onColorChanged += OnColorChanged;
    }
    private void OnDestroy()
    {
        if (colorPicker != null)
            colorPicker.onColorChanged -= OnColorChanged;
    }
}
