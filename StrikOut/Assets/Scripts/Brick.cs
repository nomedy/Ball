using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    private int hp; //kick count to destroy
    private int score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    
    public void Init(int _hp)
    {
        hp = _hp;
        if (hp <= 0)
            hp = 1;

        score = hp;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ball")
        {
            --hp;

            if(hp<=0)
            {
                Destroy(this.gameObject);
                GameManager.Instance.DestroyBrick(score);

            }
        }
    }
}
