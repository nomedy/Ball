using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float force = 25f;
    // Use this for initialization
    private Rigidbody rb;

    private float minSpeed = 10f;
    private float maxSpeed = 15f;

    private float minY = 0.2f;

    private float initPosX, initPosY;

    public bool IsLanched;
    public bool isOver;
    void Start()
    {
        initPosX = this.transform.position.x;
        initPosY = this.transform.position.y;
        rb = this.transform.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLanched)
        {
            float speed = rb.velocity.magnitude;
            Vector3 v = rb.velocity.normalized;

            if (v.y > -minY && v.y < minY)
            {
                v.y = v.y > 0 ? minY : -minY;
                v.x = 1 - v.y;

                rb.velocity = v * speed;
            }
            if (speed < minSpeed || speed > maxSpeed)
            {
                speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
                rb.velocity = v * speed;
            }
        }

        
    }

    public void Lanch()
    {
        //将球旋转45度并向上发射  
        transform.forward = new Vector3(45f, 45f, 0);
        rb.velocity = transform.forward.normalized * minSpeed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "paddle")
        {
            Paddle gd = collision.gameObject.transform.GetComponent<Paddle>();

            if (gd.moveDir != 0)
            {
                float speed = rb.velocity.magnitude;

                if (rb.velocity.y > 0.3f)
                    rb.velocity = new Vector3(rb.velocity.x + gd.moveDir * 0.2f, rb.velocity.y - gd.moveDir * 0.2f, 0) * speed;
            }

        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name== "bottom")
        {
            GameManager.Instance.ChangeLife(-1);
        }
    }

    public void Reset()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(initPosX, initPosY, 0);
        IsLanched = false;
    }
}
