using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickeringText : MonoBehaviour
{
    float speed = 1f;
    float alpha = 0f;

    // Update is called once per frame
    void Update()
    {
        alpha = Mathf.Sin(speed * Time.time);
        gameObject.GetComponent<Text>().color = new Color(1, 1, 1, Mathf.Abs(alpha));
    }
}
