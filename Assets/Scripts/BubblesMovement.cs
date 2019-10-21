using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesMovement : MonoBehaviour
{

    public float Speed = 0.000002f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = (Vector3.up * Speed) + this.transform.position;

        if (Camera.main.WorldToScreenPoint(transform.position).y > Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
