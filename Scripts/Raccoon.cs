using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : MonoBehaviour
{

    public Rigidbody2D rb2d;
    public Collider2D col;
    public Raccoon friend;

    public int id;

    public bool facingLeft = true;
    bool grounded = false;
    bool pause = false;
    float speed = 1.0f;
    float jumpheight = 5.0f;

    float selectNewActionCD = 1.0f;
    float lastSelectNewAction = 0.0f;

    public bool pickedup = false;
    public bool onTopBuilding = false;
    public bool occupied = false;
    bool dead = false;
    bool isRespawning = false;
    Coroutine lastRespawn;
    public Building current;

    float flickThreshold = 0.25f;
    public bool beingFlicked = false;
    float flickCD = 1f;
    float lastflick;
    Vector3 offset;
    Vector3 screenPoint;

    private Animator anim;
    [SerializeField] public string[] state;

    float pickedUpDelay = 0.25f;
    float lastPickedUp = 0f;

    Collider2D platform;
    bool stairsAvailable = true;
    float stairsCD = 10f;
    float lastStairUse;

    bool friendCollision = false;
    float stuckDistance = 2.0f;
    float collisionTimer = 3.0f;
    float collisionTimerStart;

    private AudioSource thud;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        platform = GameObject.Find("Platforms").GetComponent<Collider2D>();

        thud = GetComponent<AudioSource>();
        thud.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickedup && grounded && !occupied && (Time.time - lastPickedUp >= pickedUpDelay || lastPickedUp == 0)) {
            if (Time.time - lastSelectNewAction >= selectNewActionCD) {
                selectAction();
                lastSelectNewAction = Time.time;
            }
            Vector3 deltaPos;
            if (pause) {
                deltaPos = Vector3.zero;
            }
            else {
                deltaPos = facingLeft ? transform.right * -speed * Time.deltaTime : transform.right * speed * Time.deltaTime;
            }
            transform.position += deltaPos;
        }
        if (occupied || friend.occupied) {
            Physics2D.IgnoreCollision(col, friend.col, true); 
        }
        else {
            Physics2D.IgnoreCollision(col, friend.col, false);
        }
        if (dead && !isRespawning) {
            lastRespawn = StartCoroutine(respawn());
        }
        else if (!dead && isRespawning) {
            isRespawning = false;
            StopCoroutine(lastRespawn);
        }
        if (Time.time - lastflick >= flickCD && beingFlicked) {
            beingFlicked = false;
        }
        if (Time.time - lastStairUse >= stairsCD && !stairsAvailable) {
            stairsAvailable = true;
        }
        if (friendCollision) {
            if (Time.time - collisionTimerStart >= collisionTimer && (transform.position - friend.transform.position).magnitude <= stuckDistance) {
                StartCoroutine(ignoreFriend());
            }
            else if ((transform.position - friend.transform.position).magnitude > stuckDistance) {
                friendCollision = false;
            }
        }
        resetAnimation();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.tag == "Ground")
        {
            grounded = true;
        }
        if (collided.tag == "Wall")
        {
            if ((collided.transform.position.x < transform.position.x && facingLeft) || (collided.transform.position.x > transform.position.x && !facingLeft)) {
                flip();
            }
        }
        if (collided.tag == "Raccoon") {
            friendCollision = true;
            collisionTimerStart = Time.time;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.tag == "Ground")
        {
            grounded = false;
        }
    }

	public void resetAnimation() {
        if (pickedup) {
            playAnim(1);
        }
        else if (occupied) {
            playAnim(2);
        }
        else if (pause) {
            playAnim(0);
        }
        else if (!pickedup && !occupied && !pause) {
            playAnim(3);
        }
	}

	public bool isPlaying(int i) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName(state[i])) {
			return true;
		}
		else {
			return false;
		}
	}

	public void playAnim(int i) {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[i])) {
			anim.Play(state[i], 0, 0);
		}
	}
    
    void selectAction() {
        int doSomethingNew = Random.Range(0, 100);
        if (doSomethingNew < 33) {
            int doWhatNew = Random.Range(0, 100);
            if (doWhatNew <= 33) {
                flip();
            }
            if (doWhatNew > 33 && doWhatNew <= 66) {
                StartCoroutine(standstill());
            }
            if (doWhatNew > 66 && doWhatNew <= 100) {
                rb2d.AddForce(transform.up * jumpheight, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator ignoreFriend() {
        Physics2D.IgnoreCollision(friend.col, col, true);
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreCollision(friend.col, col, false);
    }

    IEnumerator standstill() {
        pause = true;
        yield return new WaitForSeconds(5.0f);
        pause = false;
    }

    public void flip() {
        facingLeft = !facingLeft;
        transform.localScale *= new Vector2(-1, 1);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        GameObject collided = collider.gameObject;
        if (collided.tag == "Building" && pickedup) {
            onTopBuilding = true;
        }
        if (collided.tag == "Up" && !pickedup && grounded && stairsAvailable) {
            if (Random.Range(0, 100) <= 10) {
                if (!facingLeft) {
                    flip();
                }
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(transform.up * jumpheight * 2f, ForceMode2D.Impulse);
                stairsAvailable = false;
                lastStairUse = Time.time;
            } 
        }
        if (collided.tag == "Down" && !pickedup && grounded && stairsAvailable) {
            if (Random.Range(0, 100) <= 10) {
                StartCoroutine(goDownStairs());
            } 
        }
    }

    IEnumerator goDownStairs() {
        stairsAvailable = false;
        lastStairUse = Time.time;
        Physics2D.IgnoreCollision(platform, col, true);
        yield return new WaitForSeconds(0.75f);
        Physics2D.IgnoreCollision(platform, col, false);

    }

    void OnTriggerStay2D(Collider2D collider) {
        GameObject collided = collider.gameObject;
        if (collided.tag == "Building" && pickedup) {
            onTopBuilding = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        GameObject collided = collider.gameObject;
        if (collided.tag == "Building" && pickedup) {
            onTopBuilding = false;
        }
        if (collided.tag == "Building" && occupied) {
            occupied = false;
            collided.GetComponent<Building>().inUse[id] = 0;
        }
    }

    void OnMouseDown() {
        pickedup = true;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb2d.velocity = Vector3.zero;
    }

    void OnMouseDrag() {
        Vector3 curScreenPoint = Input.mousePosition;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        rb2d.velocity = Vector3.zero;
        if (occupied) {
            current.inUse[id] = 0;
            occupied = false;
        }
    }

    void OnMouseUp() {
        pickedup = false;
        lastPickedUp = Time.time;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        Vector2 dir = mousePos - transform.position;
        if (dir.magnitude < flickThreshold) {
            dir = Vector2.zero;
            beingFlicked = false;
        }
        else {
            beingFlicked = true;
            lastflick = Time.time;
        }
        dir.Normalize();
        rb2d.velocity = dir * 7.5f;
    }

    void OnBecameInvisible() {
        dead = true;
    }

    void OnBecameVisible() {
        dead = false;
    }

    public IEnumerator respawn() {
        isRespawning = true;
        rb2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(3.0f);
        transform.position = new Vector2(0, 10f);
        isRespawning = false;
    }
}
