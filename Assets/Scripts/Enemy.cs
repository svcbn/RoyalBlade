using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int _hp = 1;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if(_hp < 1)
            {
                KillEnemy();
            }
        }
    }

    [SerializeField] int goldGain = 7;
    [SerializeField] int scoreGain = 5;

    void KillEnemy()
    {
        GameManager.instance.CurrentScore += scoreGain;
        GameManager.instance.Gold += goldGain;
        GameManager.instance.Combo++;
        GameManager.instance.wave.Remove(gameObject);
        Player.instance.AttackSkillCount++;
        GameManager.instance.ComboText();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "GuardArea")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
