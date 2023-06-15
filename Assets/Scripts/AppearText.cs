using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearText : MonoBehaviour
{
    float alpha = 0;
    
    void Update()
    {
        alpha = Mathf.Lerp(alpha, 1, Time.time);
        gameObject.GetComponent<Text>().color = new Color(1, 1, 1, alpha);
    }
}
