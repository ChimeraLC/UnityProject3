using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishParentController : MonoBehaviour
{
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        // Function for when 
        public abstract void Hook();

        // Set direction of fish travel
        public abstract void SetDirection(int dir);

        // What happens when fish is reeled in.
        public abstract int Reel();
}
