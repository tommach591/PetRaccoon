using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{

    int count = 0;
    float resetTimer = 0.3f;
    float lastClicked;

    Raccoon boy;
    Raccoon girl;

    // Start is called before the first frame update
    void Start()
    {
        boy = GameObject.Find("Boy").GetComponent<Raccoon>();
        girl = GameObject.Find("Girl").GetComponent<Raccoon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClicked >= resetTimer) {
            count = 0;
        }
    }

    void OnMouseDown() {
        lastClicked = Time.time;
        count++;
        if (count >= 5) {
            count = 0;
            boy.transform.position = new Vector2(0, 10f);
            boy.rb2d.velocity = Vector2.zero;
            girl.transform.position = new Vector2(0, 10f);
            girl.rb2d.velocity = Vector2.zero;
        }
    }
}
