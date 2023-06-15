using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        gameObject.GetComponent<Text>().text = GameManager.instance.Combo + " Combo!";
        float currentTime = 0;
        while (true)
        {
            currentTime += Time.deltaTime;
            transform.localPosition = transform.localPosition + new Vector3(0, 2, 0);
            gameObject.GetComponent<Text>().color = new Color(1, 1, 1, 1 - currentTime);

            if (currentTime > 2f)
            {
                break;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
