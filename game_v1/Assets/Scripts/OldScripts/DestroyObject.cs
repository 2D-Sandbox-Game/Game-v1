using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    bool isShacking = false;
    Vector2 pos;
    float shacke = .2f;
    [SerializeField]
    GameObject destructable;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isShacking)
        {
            transform.position = pos + UnityEngine.Random.insideUnitCircle * shacke;
        }
    }

    private void OnTrigger2D(Collider2D collision)
    {
        if(collision.CompareTag("HolzSchwert"));
        {
            isShacking = true;
            // int Variable Health decrement 
            // if(health<=0)
            ExplodeTheObject();
            Invoke("StopShaking", .5f);
        }
    }

    void StopShaking()
    {
        isShacking = false;
    }

    void ExplodeTheObject()
    {
        GameObject destruct = (GameObject) Instantiate(destructable); // serialiseField 
        destruct.transform.position = transform.position;
        Destroy(gameObject);
    }

}
