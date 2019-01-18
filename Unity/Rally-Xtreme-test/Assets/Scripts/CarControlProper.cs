using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlProper : MonoBehaviour
{
    [SerializeField]
    float speed = 0.5f;
    Transform myTransform;
    // this is a reference to the transform (?)
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // LateUpdate calls at the end of a frame
    void LateUpdate()
    {
        /*
        void OnCollisionExit2D(Collision2D other)
        {
            float adjust = 5 * direction;
            other.rigidbody.velocity = new Vector2(other.rigidbody.velocity.x, other.rigidbody.velocity.y + adjust);
        }*/ // useful code for determining contacts
    }

    // FixedUpdate calls each physics tick
    void FixedUpdate()
    {
        


        if (Input.GetKey ("w"))
        {
            MoveUp();
        }
        else if (Input.GetKey("s"))
        {
            MoveDown();
        }
        if (Input.GetKey("a"))
        {
            MoveLeft();
        }
        else if (Input.GetKey("d"))
        {
            MoveRight();
        }
    }

    void MoveUp()
    {
        myTransform.position = new Vector2(myTransform.position.x, myTransform.position.y + speed);
    }
    void MoveDown()
    {
        myTransform.position = new Vector2(myTransform.position.x, myTransform.position.y - speed);
    }
    void MoveLeft()
    {
        myTransform.position = new Vector2(myTransform.position.x - speed, myTransform.position.y);
    }
    void MoveRight()
    {
        myTransform.position = new Vector2(myTransform.position.x + speed, myTransform.position.y);
    }
}
