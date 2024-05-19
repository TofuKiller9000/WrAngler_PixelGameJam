using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollider : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;

    public GameObject collider1;
    public GameObject collider2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Object.Destroy(object1);
        Object.Destroy(object2);
        Object.Destroy(object3);
        Object.Destroy(object4);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Object.Destroy(gameObject);
        Object.Destroy(collider1);
        Object.Destroy(collider2);
    }
}
