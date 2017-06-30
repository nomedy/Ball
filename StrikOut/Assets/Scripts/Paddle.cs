using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

    // Use this for initialization
    public float speed = 1200f;
    public int moveDir = 0; // -1向左1向右

    public Bullet ballscript;

    private Vector3 initPosition;

    void Start () {
        initPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
    }

	// Update is called once per frame
	void Update () {

        if(Input.touchCount > 0)
        {
            if (!ballscript.IsLanched)
            {
                GameManager.Instance.StartGame();
                ballscript.IsLanched = true;
                ballscript.Lanch();
            }
            if (Input.touches[0].phase==TouchPhase.Began || Input.touches[0].phase == TouchPhase.Stationary)
            {
                if (Input.touches[0].position.x < Screen.width / 2)
                    moveDir = 1;
                else
                    moveDir = -1;
            }
        }
        else
            moveDir = 0;


        if (moveDir==1)
        {
            if (this.transform.position.x > -4f)
            {
                this.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }


        }
        else if (moveDir==-1)
        {
            if (this.transform.position.x < 4f)
            {
                this.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!ballscript.IsLanched)
            {
                GameManager.Instance.StartGame();
                ballscript.IsLanched = true;
                ballscript.Lanch();
            }

        }

        if (GameManager.Instance.GetGameState() != GameState.Start || !ballscript.IsLanched)
            return;
        if (Input.GetKey(KeyCode.A))
        {
            if (this.transform.position.x > -4f)
            {
                this.transform.Translate(Vector3.left * speed * Time.deltaTime);
                
            }
            moveDir = 1;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (this.transform.position.x < 4f)
            {
                this.transform.Translate(Vector3.right * speed * Time.deltaTime);
                
            }
            moveDir = -1;
        }
        //else
        //    moveDir = 0;

    }

    public void Reset()
    {
        this.transform.position = initPosition;
    }
}
