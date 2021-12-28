using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolume : MonoBehaviour
{

    [SerializeField] float delta;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustVolume() {
        AudioListener.volume += delta;
    }

    public void OnMouseDown() {
        AdjustVolume();
    }
}
