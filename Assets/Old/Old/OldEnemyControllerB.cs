//using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemyControllerB : MonoBehaviour
{
        /*
        // Player controller
        private OldPlayerController target;
        private Rigidbody2D bodyRb;

        public int state = 0;

        // Pathfinding ai.
        IAstarAI ai;
        // Start is called before the first frame update
        void Start()
        {
                bodyRb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
                
                ai = GetComponent<IAstarAI>();
                if (ai != null) ai.onSearchPath += Update;


                target = GameObject.Find("PlayerController").GetComponent<OldPlayerController>();

        }

        private void OnDisable()
        {
                if (ai != null) ai.onSearchPath -= Update;
        }
        // Update is called once per frame
        void Update()
        {
                // If in normal state.
                if (state == 0)
                {
                        if (target != null && ai != null) ai.destination = target.currentPos;
                }


                // Post-hit state.
                else if (state == 1) {
                        ai.isStopped = true;
                }
        }

        public void Hit()
        {
                state = 1;
        }
        */
}
