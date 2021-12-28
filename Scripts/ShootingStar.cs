using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
	protected Rigidbody2D rb2d;
	protected SpriteRenderer sr;

    public Sprite starType;
    protected Vector2 dir;
	protected float angle;
    protected float speed;

    public float despawnTime;
    public float spawnTime;

    AudioSource zoom;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake() {
		rb2d = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();

        zoom = GetComponent<AudioSource>();
        zoom.volume = 0.1f;
        zoom.Play();

        dir = new Vector2(2, -1);
        spawnTime = Time.time;
        despawnTime = 2f;
        speed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (sr.sprite != starType) {
            sr.sprite = starType;
        }
        fire();
        if (Time.time - spawnTime >= despawnTime) {
            Destroy(gameObject);
        }
    }

	protected void fire() {
		dir.Normalize();
		rb2d.velocity = dir * speed;
		angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

}
