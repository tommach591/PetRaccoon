using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{

    private GameObject leftCoffee;
    private GameObject rightCoffee;

    private Animator anim;
    private AudioSource plop;
    [SerializeField] public string[] state;

    private float animateTime = 0.2f;
    private float lastOn;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        plop = GetComponent<AudioSource>();
        plop.volume = 0.30f;
        leftCoffee = GameObject.Find("Coffee 1");
        rightCoffee = GameObject.Find("Coffee 2");
    }
 
    // Update is called once per frame
    void Update()
    {
        if (isPlaying(1) && Time.time - lastOn >= animateTime) {
            playAnim(0);
        }
    }

    void OnMouseDown() 
    {
        playAnim(1);
        plop.Play();
        lastOn = Time.time;
        leftCoffee.SetActive(true);
        rightCoffee.SetActive(true);
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
}
