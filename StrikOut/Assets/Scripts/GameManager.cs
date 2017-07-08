using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;


public enum GameState
{
    Start,
    Success,
    Fail
}

public class MapCfg
{
    public int[] Maps;
}

public class GameManager : MonoBehaviour {


    public Transform leftWall;
    public Transform rightWall;
    public Transform topWall;
    public Transform bottomWall;
    public Text text;
    public Text scoreTxt, hpTxt;

    public GameObject[] brickPrefab;
    private List<GameObject> bricks;

    public static GameManager Instance;

    public Bullet ballScript;
    public Paddle paddle;

    public int brickCount; //剩余砖块数量
    public int lives=3; //生命值
    private GameState gameState = 0; //1成功2失败
    private int level = 1; //当前关卡
    public int score = 0;
    private List<List<int>> maps;
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
        gameState = GameState.Start;
        hpTxt.text = "Lives:" + lives.ToString();

        string path = Application.dataPath + "/Resources/map.json";
        //TextAsset txt = Resources.Load("map") as TextAsset;

        if(!File.Exists(path))
        {
            Debug.LogError("Can't find file:" + path);
        }

        StreamReader sr = new StreamReader(path);
        string text = sr.ReadToEnd();

        maps = JsonMapper.ToObject<List<List<int>>>(text);
        sr.Close();
        Create();
    }
	
    public GameState GetGameState()
    {
        return gameState;
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

        if (level < 0)
            level = 0;
        if (level >= maps.Count)
            level = level % maps.Count;
        List<int> nowMap = maps[level - 1];
        int col = 7;
        int row = nowMap.Count/col;
        if (nowMap.Count % col != 0)
            row += 1;
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

        for(int i=0; i<nowMap.Count; ++i)
        {
            if (nowMap[i] == 0)
                continue;

            int col1 = i % 7;
            int row1 = (i-col1) / 7;

            float x = leftWall.transform.position.x + width + col1 * (width + destX);
            float y = topWall.transform.position.y - 4 * height - row1 * (height + destY);
            Debug.Log(x + ":" + y);
            GameObject obj = Instantiate(brickPrefab[Random.Range(0, brickPrefab.Length)], new Vector3(x, y, 0), Quaternion.identity) as GameObject;

            Brick bcom = obj.AddComponent<Brick>();
            //bcom.Init(Random.Range(1, 4)); //初始化可被撞击次数
            bcom.Init(1); //初始化可被撞击次数

            bricks.Add(obj);

            ++brickCount;
        }


        //for (int i=0; i< col; ++i)
        //{
        //    for(int j=0; j< row; ++j)
        //    {
        //        float x = leftWall.transform.position.x +  width + j * (width + destX);
        //        float y = topWall.transform.position.y - 4 * height - i * (height + destY);
        //        Debug.Log(x + ":" + y);
        //        GameObject obj = Instantiate(brickPrefab[Random.Range(0, brickPrefab.Length)], new Vector3(x, y, 0), Quaternion.identity) as GameObject;

        //        Brick bcom = obj.AddComponent<Brick>();
        //        bcom.Init(Random.Range(1, 4)); //初始化可被撞击次数
        //        bricks.Add(obj);
               
        //        ++brickCount;
        //    }
        //}
        gameState = 0;
    }

    //开始游戏
    public void StartGame()
    {
        if(gameState==GameState.Fail)
        {
            score = 0;
            scoreTxt.text = "Score: " + score.ToString();

            lives = 3;
            hpTxt.text = "Lives:" + lives.ToString();
        }

        if (gameState == GameState.Success)
            level += 1;

        if (gameState != GameState.Start)
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
        hpTxt.text = "Lives:" + lives.ToString();
    }

    public void Reset()
    {
        this.ballScript.Reset();
        this.paddle.Reset();
    }

    //完成
    public void FinishGame()
    {
        text.gameObject.SetActive(true);
        text.text = "finish";
        this.Reset();
        this.gameState = GameState.Success;
    }

    //生命值为0
    public void LifeOver()
    {
        text.gameObject.SetActive(true);
        text.text = "failed";
        gameState = GameState.Fail;
    }

    public void ChangeScore(int _score)
    {
        score += _score;
        scoreTxt.text = "Score: " + score.ToString();
    }
}
