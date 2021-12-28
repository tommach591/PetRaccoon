using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    bool turnedOn = false;
    AudioSource bgm;
    [SerializeField] MusicNote notePrefab;

    float noteSpawn = 0.8f;
    float lastNote = 0.0f;

    private Animator anim;
    [SerializeField] public string[] state;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.volume = 0.04f;
        anim = GetComponent<Animator>();
    }   

    // Update is called once per frame
    void Update()
    {
        if (turnedOn) {
            if (!bgm.isPlaying) {
                bgm.Play();
            }
            playAnim(1);
            if (Time.time - lastNote >= noteSpawn) {
                Instantiate(notePrefab, new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z), transform.rotation);
                lastNote = Time.time;
            }
        }
        else {
            if (bgm.isPlaying) {
                bgm.Pause();
            }
            playAnim(0);
        }
    }

    void OnMouseDown() {
        turnedOn = !turnedOn;
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
