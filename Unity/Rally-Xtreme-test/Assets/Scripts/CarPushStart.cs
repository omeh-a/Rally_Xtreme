using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPushStart : MonoBehaviour
{
    [SerializeField]
    float forceValue = 5f;
    Rigidbody2D myBody;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        // assigning reference to the rigidbody
        myBody.AddForce(new Vector2(forceValue * 50, 50));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
