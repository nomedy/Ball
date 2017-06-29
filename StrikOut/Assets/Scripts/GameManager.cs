using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    public Transform leftWall;
    public Transform rightWall;
    public Transform topWall;
    public Transform bottomWall;
    public Text text;
    public Text scoreTxt;

    public GameObject[] brickPrefab;
    private List<GameObject> bricks;

    public static GameManager Instance;

    public Bullet ballScript;
    public Paddle paddle;

    public int brickCount; //剩余砖块数量
    public int lives=3; //生命值
    private bool canNext;
    public int score = 0;
	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        lives = 3;
        score = 0;
        scoreTxt.text = "Score: " + score.ToString();
        Create();
    }
	

    public void Create()
    {
        text.gameObject.SetActive(true);
        text.text = "touch to start game";
        Camera mainCam = Camera.main;
        float leftPos = mainCam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightPos = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float topPos = mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        float bottomPos = mainCam.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        int row = Random.Range(4, 7);
        int col = 7;
        float width = brickPrefab[0].GetComponent<MeshFilter>().sharedMesh.bounds.size.x * brickPrefab[0].transform.localScale.x;
        float height = brickPrefab[0].GetComponent<MeshFilter>().sharedMesh.bounds.size.y * brickPrefab[0].transform.localScale.y;

        Debug.Log("width:" + width + ", height:" + height);

        float destX = width/3;
        float destY = height;
        if(bricks!=null)
        {
            for(int i=0; i<bricks.Count; ++i)
            {
                if(bricks[i]!=null)
                    Destroy(bricks[i]);
            }
            bricks.Clear();
        }
        else
        {
            bricks = new List<GameObject>();
        }
        brickCount = 0;
        for (int i=0; i< row; ++i)
        {
            for(int j=0; j< col; ++j)
            {
                float x = leftWall.transform.position.x +  width + j * (width + destX);
                float y = topWall.transform.position.y - 4 * height - i * (height + destY);
                Debug.Log(x + ":" + y);
                GameObject obj = Instantiate(brickPrefab[Random.Range(0, brickPrefab.Length)], new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                Brick bcom = obj.AddComponent<Brick>();
                bcom.Init(Random.Range(1, 4)); //初始化可被撞击次数
                bricks.Add(obj);
               
                ++brickCount;
            }
        }
        canNext = false;
    }

    //开始游戏
    public void StartGame()
    {
        if (canNext)
            this.Create();
        text.gameObject.SetActive(false);
    }

    //删除brick
    public void DestroyBrick(int _score)
    {
        --brickCount;
        this.ChangeScore(_score);
        if(brickCount<=0)
        {
            FinishGame();
        }
    }

    //改变生命值
    public void ChangeLife(int count)
    {
        lives += count;
        if(count<0)
            Reset();
        if (lives<=0)
        {
            LifeOver();
        }
    }

    public void Reset()
    {
        this.ballScript.Reset();
        this.paddle.Reset();
        score = 0;
        scoreTxt.text = "Score: " + score.ToString();
    }

    //完成
    public void FinishGame()
    {
        text.gameObject.SetActive(true);
        text.text = "finish";
        this.Reset();
        this.canNext = true;
    }

    //生命值为0
    public void LifeOver()
    {
        text.gameObject.SetActive(true);
        text.text = "failed";
    }

    public void ChangeScore(int _score)
    {
        score += _score;
        scoreTxt.text = "Score: " + score.ToString();
    }
}
