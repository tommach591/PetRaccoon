using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    public float xmax;
    public float xmin;
    public float speed;
    public Sprite[] clouds;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = clouds[Random.Range(0, 4)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        if (transform.position.x >= xmax) {
            transform.position = new Vector3(xmin, transform.position.y, transform.position.z);
            sr.sprite = clouds[Random.Range(0, 4)];
        }
    }
}
