using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Button btnJump;

    // Start is called before the first frame update
    void Start()
    {
        btnJump.onClick.AddListener(BtnJumpClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnJumpClicked()
    {
        player.GetComponent<Player>().Jump();
    }
}
