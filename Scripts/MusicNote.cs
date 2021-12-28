using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{

    SpriteRenderer sr;
    [SerializeField] Sprite[] notetype;
    public float speed;
    public float despawnTime;
    public float spawnTime;

    public float flipTime;
    public float lastflip;

    public bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        spawnTime = Time.time;
        despawnTime = 1.5f;
        speed = 1f;
        flipTime = 0.5f;
        facingLeft = false;
        sr.sprite = notetype[Random.Range(0, notetype.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        flip();
        Vector3 deltaPos;
        deltaPos = facingLeft ? transform.right * -speed / 2 * Time.deltaTime : transform.right * speed / 2 * Time.deltaTime;
        transform.position += deltaPos;
        deltaPos = transform.up * speed * Time.deltaTime;
        transform.position += deltaPos;

        if (Time.time - spawnTime >= despawnTime) {
            StartCoroutine(startDeathAnimation());
        }
    }

    public void flip() {
        if (Time.time - lastflip >= flipTime) {
            facingLeft = !facingLeft;
            lastflip = Time.time;
        }
    }

    IEnumerator startDeathAnimation() {
        for (int i = 0; i < 10; i++) {
            Color fadeColor = sr.color;
            fadeColor.a -= 0.01f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fadeColor.a);
            yield return new WaitForSeconds(0.25f);
        }
        Destroy(gameObject);
    }

}
