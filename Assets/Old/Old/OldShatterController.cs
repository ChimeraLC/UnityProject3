using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldShatterController : MonoBehaviour
{
        ParticleSystem pSystem;
        // Start is called before the first frame update
        void Start()
        {
                pSystem = GetComponent<ParticleSystem>();
                pSystem.Play();
        }

        // Update is called once per frame
        void Update()
        {
                
        }
}
