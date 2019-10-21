using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBubble : MonoBehaviour
{

    public GameObject bubble;
    public float index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float randomTime = Random.Range(0.6f, 1.5f);

        if (index >= randomTime)
        {
            
            float randomX = Random.Range(-1.5f, 1.5f);
            Vector3 randomVec = new Vector3(randomX,0);
            Instantiate(bubble, this.transform.position + randomVec, Quaternion.identity);
            index = 0;
        }

        index += Time.deltaTime;
    }
}
