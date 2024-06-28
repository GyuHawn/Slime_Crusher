using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    // 화면 비율 조절
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheigt = ((float)Screen.width / Screen.height) / ((float) 16 / 9);
        float scalewidth = 1f / scaleheigt;
        if(scaleheigt < 1)
        {
            rect.height = scaleheigt;
            rect.y = (1f - scaleheigt) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
}
