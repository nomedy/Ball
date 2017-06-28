using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
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
        //rb.velocity = new Vector3(1, 2, 0);
        ////rb.AddForce(new Vector3(2, 1, 0) * force);
        ////m_preVelocity = rb.velocity;
        //Debug.Log(m_preVelocity);

        rb.velocity = Vector3.zero;
        //Lanch();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.rotation);
        //transform.Translate(Vector3.up * 5 * Time.deltaTime);

        //transform.position -= transform.position.z * Vector3.forward;
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
        //Debug.Log("Into rb");
        ////ContactPoint contact = collision.contacts[0];
        ////Vector3 newDir = Vector3.zero;
        ////var curDir = rb.transform.TransformDirection(Vector3.forward);

        //////newDir = curDir - 2 * contact.normal * (Vector3.Dot(contact.normal, curDir));
        ////newDir = Vector3.Reflect(curDir, contact.normal);
        ////Debug.Log(newDir);
        //////rb.velocity = newDir.normalized * m_preVelocity.y / m_preVelocity.normalized.y;
        ////rb.rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        //Debug.Log("start:" + rb.velocity);
        ////m_preVelocity = rb.velocity;
        //ContactPoint contactPoint = collision.contacts[0];
        //Vector3 newDir = Vector3.zero;
        //Vector3 curDir = transform.TransformDirection(Vector3.forward);
        //newDir = Vector3.Reflect(curDir, contactPoint.normal);
        //newDir.z = 0;
        //Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
        //transform.rotation = rotation;
        ////rb.velocity = m_preVelocity;
        //Debug.Log(rb.velocity.normalized);
        //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.normalized.y * 2, 0);
        //Debug.Log("end:" + rb.velocity);

        if(collision.gameObject.tag=="unit")
        {
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.name== "guard")
        {
            guard gd = collision.gameObject.transform.GetComponent<guard>();

            if(gd.moveDir!=0)
            {
                float speed = rb.velocity.magnitude;

                rb.velocity = new Vector3(gd.moveDir * 0.2f, 1 - gd.moveDir * 0.2f, 0) * speed;
            }

        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name== "bottom")
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(initPosX, initPosY, 0);
            Lanch();
        }
    }
}
