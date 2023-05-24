using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public GameObject fishingRod;

        private CharacterController playerCC;
        private PlayerSpriteController playerSprite;
        private ItemController itemController;

        public GameController gameController;
        public BoatController boatController;
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
                itemController = rod.GetComponent<ItemController>();
                itemController.SetBounds(bounds);
                itemController.gameController = gameController;

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
                itemController.reflectState = (int) Mathf.Sign(mouseAngle);

                // TODO: redesign to cancel instead of lock out.

                if (Input.GetMouseButtonDown(0))
                {
                        // Check state idle or currently fishing
                        if (gameController.GetState() == 0 || gameController.GetState() == 2)
                        {
                                // Set state to casting
                                gameController.SetState(1);
                                // Initiate cast.
                                itemController.Cast();
                        }
                }

                // Fixing boat holes (incomplete)
                if (Input.GetKeyDown(KeyCode.R)) {
                        // Check state idle or currently carrying
                        if (gameController.GetState() == 0 || gameController.GetState() == 4)
                        {
                                boatController.Fix(transform.position.x, transform.position.y);
                        }
                }
        }

        public void ItemSetSprite(int state) { 
                itemController.SetSprite(state);
        }
}
