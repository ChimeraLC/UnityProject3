using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public GameObject fishingRod;

        private CharacterController playerCC;
        private PlayerSpriteController playerSprite;
        private RodController rodController;

        private float[] bounds = { 3, -3, 1.5f, -1.5f };

        private float speed = 2;
        // Start is called before the first frame update
        void Start()
        {
                // Getting player sprite.
                playerSprite = transform.GetChild(0).GetComponent<PlayerSpriteController>();

                // Getting player controller.
                playerCC = GetComponent<CharacterController>();

                // Creating fishing rod.
                GameObject rod = Instantiate(fishingRod, transform.position, Quaternion.identity);
                rod.transform.parent = transform;
                rodController = rod.GetComponent<RodController>();
                rodController.SetBounds(bounds);

                playerSprite.SetRod(rod);
                        
        }

        // Update is called once per frame
        void Update()
        {

                // Mouse position
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float mouseAngle = Mathf.Atan2(mousePosition.x - transform.position.x,
                    mousePosition.y - transform.position.y) * Mathf.Rad2Deg;

                /*
                * movement control
                */
                Vector2 moveDir = Vector2.zero;
                if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;
                if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
                if (Input.GetKey(KeyCode.W)) moveDir.y += 1;
                if (Input.GetKey(KeyCode.S)) moveDir.y -= 1;

                //normalize and move
                moveDir.Normalize();
                playerCC.Move(speed * moveDir * Time.deltaTime);

                // Remaining in bounds.
                Vector2 boundPos = transform.position;
                boundPos.x = Mathf.Min(transform.position.x, bounds[0]); 
                boundPos.x = Mathf.Max(boundPos.x, bounds[1]);
                boundPos.y = Mathf.Min(transform.position.y, bounds[2]);
                boundPos.y = Mathf.Max(boundPos.y, bounds[3]);


                transform.position = boundPos;

                // Calling sprite updates.
                playerSprite.UpdateAnimation(mouseAngle, moveDir.magnitude > 0);
                rodController.reflectState = (int) Mathf.Sign(mouseAngle);

                if (Input.GetMouseButtonDown(0)) {
                        rodController.Cast();
                }
        }
}
