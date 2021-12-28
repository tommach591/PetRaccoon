using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public float xmin;
    public float xmax;

    public float ymin;
    public float ymax;

    public float speed;
    bool goingLeft = false;
    bool goingUp = false;

    float lastSelectNewAction;
    float selectNewActionCD = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastSelectNewAction = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSelectNewAction >= selectNewActionCD) {
            selectAction();
            lastSelectNewAction = Time.time;
        }
        Vector3 deltaXPos = goingLeft ? transform.right * -speed * Time.deltaTime : transform.right * speed * Time.deltaTime;
        transform.localPosition += deltaXPos;
        if (transform.localPosition.x < xmin) {
            transform.localPosition = new Vector3(xmin, transform.localPosition.y, transform.localPosition.z);
            flipX();
        }
        if (transform.localPosition.x > xmax) {
            transform.localPosition = new Vector3(xmax, transform.localPosition.y, transform.localPosition.z);
            flipX();
        }

        Vector3 deltaYPos = goingUp ? transform.up * -speed * Time.deltaTime : transform.up * speed * Time.deltaTime;
        transform.localPosition += deltaYPos;
        if (transform.localPosition.y < ymin) {
            transform.localPosition = new Vector3(transform.localPosition.x, ymin, transform.localPosition.z);
            flipY();
        }
        if (transform.localPosition.y > ymax) {
            transform.localPosition = new Vector3(transform.localPosition.x, ymax, transform.localPosition.z);
            flipY();
        }
    }

    void flipX() {
        goingLeft = !goingLeft;
    }

    void flipY() {
        goingUp = !goingUp;
    }

    void selectAction() {
        int doSomethingNew = Random.Range(0, 100);
        if (doSomethingNew < 33) {
            int doWhatNew = Random.Range(0, 100);
            if (doWhatNew <= 50) {
                flipX();
            }
            if (doWhatNew > 50 && doWhatNew <= 100) {
                flipY();
            }
        }
    }
}
