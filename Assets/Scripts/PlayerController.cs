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

        // Hurt state
        private float hurtTimer = 0;
        private float hurtTimerMax = 0;
        private bool hurt;

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
                itemController.boatController = boatController;
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

                // Hurt
                if (hurt) {
                        hurtTimer += Time.deltaTime;
                        if (hurtTimer > hurtTimerMax) {
                                hurtTimer = 0;
                                hurt = false;
                                gameController.SetState(0);
                                itemController.SetSprite(0);
                        }
                        //TODO: hurt animation
                        playerSprite.UpdateAnimationHurt();
                }
                // Loss of control while hurt
                else
                {
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

                        // TODO: fix these calculations to be more robust
                        // Precalculate move
                        Vector3 boundPos = transform.position + speed * (Vector3)moveDir * Time.deltaTime;

                        if (boatController.CheckPosition(boundPos.x, transform.position.y) == null)
                        {
                                boundPos.x = Mathf.Floor(boundPos.x) + 0.5f - moveDir.x/100;
                        }
                        if (boatController.CheckPosition(transform.position.x, boundPos.y) == null)
                        {
                                boundPos.y = Mathf.Floor(boundPos.y) + 0.5f - moveDir.y / 100;
                        }

                        transform.position = boundPos;

                        // Calling sprite updates.
                        playerSprite.UpdateAnimation(mouseAngle, moveDir.magnitude > 0);
                        itemController.reflectState = (int)Mathf.Sign(mouseAngle);

                        // TODO: redesign to cancel instead of lock out.


                        // Item interaction
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
                        //Releasing rod
                        if (Input.GetMouseButtonDown(1))
                        {
                                // Check state is currently fishing or reeling
                                if (gameController.GetState() == 1 || gameController.GetState() == 2 || gameController.GetState() == 5)
                                {
                                        itemController.Release();
                                }
                        }
                        // Repairing

                        // Fixing boat holes (incomplete)
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                                // Check state idle or currently carrying
                                if (gameController.GetState() == 0 || gameController.GetState() == 4)
                                {
                                        boatController.Fix(transform.position.x, transform.position.y);
                                }
                        }


                        // Placing new tiles
                        if (Input.GetKey(KeyCode.Q)) {
                                if (gameController.GetState() == 0) { 
                                        if ((mousePosition - (Vector2) transform.position).magnitude < 2) {
                                                boatController.PlaceMarker(mousePosition.x, mousePosition.y);        
                                        }
                                }
                        }
                        if (Input.GetKeyUp(KeyCode.Q)) {
                                if (gameController.GetState() == 0)
                                {
                                        if ((mousePosition - (Vector2)transform.position).magnitude < 2)
                                        {
                                                boatController.Place(mousePosition.x, mousePosition.y);
                                        }
                                }
                        }

                        // Test state
                        if (Input.GetKeyDown(KeyCode.H))
                        {

                                Hit(1, Vector2.left);
                        }

                        if (Input.GetKeyDown(KeyCode.Y))
                        {
                                boatController.DestroyPosition(mousePosition.x, mousePosition.y);
                        }

                }
        }

        public void ItemSetSprite(int state) { 
                itemController.SetSprite(state);
        }
        // Damaging character
        public void Hit(float intensity, Vector2 dir) {
                int curState = gameController.GetState();
                // Release any potential rods
                if (curState == 1 || curState == 2 || curState == 5)
                {
                        itemController.Release();
                }
                // Cancel any fixes
                if (curState == 3) {
                        boatController.Reset();
                        // TODO: dropped supplies icon
                }
                itemController.SetSprite(-1);
                gameController.SetState(6);

                // Update sprite animator
                playerSprite.StartAnimationHurt();

                // Calculating hurttimer
                hurtTimer = 0;
                hurtTimerMax = intensity;
                hurt = true;
        }       
}
