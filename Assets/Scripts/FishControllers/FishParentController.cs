using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishParentController : MonoBehaviour
{
        public GameController gameController;
        protected BobberController bob;
        //protected ItemController item;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        // Function for when bobber lands in hitbox
        public abstract void Hook(GameObject bobber);

        // Set direction of fish travel
        public abstract void SetDirection(int dir);

        // What happens when fish is reeled in.
        public abstract int Reel(ItemController item);
        // Releasing hook without reeling
        public abstract int Release(ItemController item);
}
