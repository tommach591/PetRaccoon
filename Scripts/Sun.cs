using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    float maxY = 1.3f;
    Vector3 restPos;

    Vector3 offset;
    Vector3 screenPoint;

    SpriteRenderer sr;
    [SerializeField] Sprite[] sunmoon;
    public bool daytime = true;

    [SerializeField] GameObject darkness;
    SpriteRenderer darknesssr;
    [SerializeField] Sprite[] darknesstype;
    Desktop desktop;

    private AudioSource plop;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        desktop = GameObject.Find("PC").GetComponent<Desktop>();
        restPos = transform.position;
        darkness = GameObject.Find("Darkness");
        darknesssr = darkness.GetComponent<SpriteRenderer>();
        darkness.SetActive(false);

        plop = GetComponent<AudioSource>();
        plop.volume = 0.30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!daytime && desktop.occupied) {
            darknesssr.sprite = darknesstype[1];
        }
        else if (!daytime) {
            darknesssr.sprite = darknesstype[0];
        }
    }

    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag() {
        Vector3 curScreenPoint = Input.mousePosition;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        if (curPosition.y < maxY) {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.x);
        }
        else {
            transform.position = new Vector3(transform.position.x, curPosition.y, transform.position.x);
        }
    }

    void OnMouseUp() {
        if (transform.position.y == maxY) {
            if (daytime) {
                sr.sprite = sunmoon[1];
                darkness.SetActive(true);
            }
            else {
                sr.sprite = sunmoon[0];
                darkness.SetActive(false);
            }
            daytime = !daytime;
            plop.Play();
        }
        transform.position = restPos;
    }
}
