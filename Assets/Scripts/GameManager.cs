using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /*[HideInInspector]*/ public Button btnJump;
    /*[HideInInspector]*/ public Button btnAttack;
    /*[HideInInspector]*/ public Button btnGuard;
    /*[HideInInspector]*/ public Button btnPause;
    /*[HideInInspector]*/ public Button btnResume;
    /*[HideInInspector]*/ public Button btnExit;
    /*[HideInInspector]*/ public Button btnTitle;
    /*[HideInInspector]*/ public Button btnExit2;

    /*[HideInInspector]*/ public GameObject panelPause;
    /*[HideInInspector]*/ public GameObject panelTitle;
    /*[HideInInspector]*/ public GameObject panelPlay;
    /*[HideInInspector]*/ public GameObject panelGameOver;

    public Transform comboPosition;
    GameObject comboFactory;

    int _currentScore = 0;
    int _combo = 0;
    int maxCombo = 0;
    int _hp = 3;
    int currentWave = 0;

    public Text txtWave;

    public Text txtTitleHighScore;
    public Text txtCurrentScore;
    public int CurrentScore
    {
        get
        {
            return _currentScore;
        }
        set
        {
            _currentScore = value;
            txtCurrentScore.text = value +"";
            if(value > HighScore)
            {
                HighScore = value;
            }
        }
    }

    public Text txtPlayerHp;
    public int playerHP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            txtPlayerHp.text = "X " + value;
            if(value < 1)
            {
                ChangeState(GameState.GameOver);
            }
        }
    }

    public Text txtHighScore;
    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt("HighScore");
        }
        set
        {
            PlayerPrefs.SetInt("HighScore", value);
            txtHighScore.text = value + "";
        }
    }

    public Text txtGold;
    public int Gold
    {
        get
        {
            return PlayerPrefs.GetInt("Gold");
        }
        set
        {
            PlayerPrefs.SetInt("Gold", value);
            txtGold.text = value + "";
        }
    }


    public int Combo
    {
        get
        {
            return _combo;
        }
        set
        {
            _combo = value;
            if(_combo > maxCombo) maxCombo = _combo;
        }
    }

    public void ComboText()
    {
        Instantiate(comboFactory, comboPosition.position, Quaternion.identity, comboPosition.transform);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public enum GameState
    {
        Ready,
        Play,
        GameOver
    }

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        #region BtnListener
        btnJump.onClick.AddListener(BtnJumpClicked);
        btnAttack.onClick.AddListener(BtnAttackClicked);
        btnPause.onClick.AddListener(BtnPauseClicked);
        btnResume.onClick.AddListener(BtnResumeClicked);
        btnExit.onClick.AddListener(BtnExitClicked);
        btnExit2.onClick.AddListener(BtnExitClicked);
        btnTitle.onClick.AddListener(BtnTitleClicked);
        #endregion
        panelPause.SetActive(false);
        panelPlay.SetActive(false);
        panelTitle.SetActive(true);
        panelGameOver.SetActive(false);

        enemyFactory = (GameObject)Resources.Load("Enemy");
        comboFactory = (GameObject)Resources.Load("TxtCombo");
        CurrentScore = 0;
        HighScore = PlayerPrefs.GetInt("HighScore");
        Gold = PlayerPrefs.GetInt("Gold");
        ChangeState(GameState.Ready);

    }

    #region ButtonListener
    void BtnJumpClicked()
    {
        Player.instance.Jump();
    }

    void BtnAttackClicked()
    {
        Player.instance.Attack();
    }

    public void BtnGuardDown()
    {
        Player.instance.Guard();
    }

    public void BtnGuardUp()
    {
        StartCoroutine(Player.instance.GuardCoolDown());
    }

    public void BtnPauseClicked()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void BtnResumeClicked()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void BtnExitClicked()
    {
        Application.Quit();
    }

    public void BtnTitleClicked()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    void ChangeState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.Ready:
                StartCoroutine(Ready());
                break;
            case GameState.Play:
                Play();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    }

    IEnumerator Ready()
    {
        txtTitleHighScore.text = PlayerPrefs.GetInt("HighScore") + "";
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ChangeState(GameState.Play);
                break;
            }
            yield return null;
        }
    }

    void Play()
    {
        panelTitle.SetActive(false);
        panelPlay.SetActive(true);
        StartCoroutine(Wave());
    }

    IEnumerator Wave()
    {
        while(true)
        {
            if (gameState != GameState.Play) break;
            if(wave.Count == 0)
            {
                yield return new WaitForSeconds(2f);
                MakeWave();
            }
            yield return null;
        }
    }

    /*[HideInInspector]*/ public Text txtGameOverScore;
    /*[HideInInspector]*/ public Text txtGameOverWave;
    /*[HideInInspector]*/ public Text txtGameOverMaxCombo;


    void GameOver()
    {
        panelGameOver.SetActive(true);
        txtGameOverScore.text = CurrentScore + "";
        txtGameOverWave.text = currentWave + "";
        txtGameOverMaxCombo.text = maxCombo + "";
    }

    GameObject enemyFactory;
    public List<GameObject> wave;

    void MakeWave()
    {
        currentWave++;
        txtWave.text = "wave " + currentWave;
        wave = new List<GameObject>();
        int rand = Random.Range(3, 6);
        for(int i = 0; i < rand; i++)
        {
            wave.Add(Instantiate(enemyFactory));
        }
        SetWave();
    }

    public Transform wavePoint;
    void SetWave()
    {
        foreach(GameObject enemy in wave)
        {
            enemy.transform.position = wavePoint.position;
        }
    }
}
