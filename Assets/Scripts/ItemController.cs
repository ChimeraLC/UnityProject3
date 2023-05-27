using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemController : MonoBehaviour
{
        public GameObject bobber;
        private GameObject curBobber;
        public GameController gameController;
        public FishParentController clampedFish;
        private BobberController bobberController;
        private LineRenderer fishingLine;
        private SpriteRenderer spriteRenderer;
        private int castState = 0;
        private float castAngle;
        public int reflectState = 1;

        // Sprite updates
        public Sprite sprRod;
        public Sprite sprRepairs;
        private int sprState;

        // Bounds of boat
        private float[] bounds;
        // Start is called before the first frame update
        void Start()
        {
                castAngle = 0;
                fishingLine = GetComponent<LineRenderer>();
                fishingLine.startWidth = 0.02f;
                fishingLine.endWidth = 0.02f;
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
                // Facing direction
                spriteRenderer.flipX = (reflectState == 1) ? false : true;
                // Charging up cast.
                if (castState == 1) {
                        castAngle = Mathf.Min(castAngle + 45 * Time.deltaTime, 90);

                        // Releasing cast.
                        if (Input.GetMouseButtonUp(0)) {
                                // Minimum chargeup.
                                if (castAngle > 30)
                                {
                                        // Set player state to fishing
                                        gameController.SetState(2);
                                        castState = 2;
                                        // Mouse position.
                                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                                        // Create bobber
                                        curBobber = Instantiate(bobber, transform.parent.position, Quaternion.identity);
                                        bobberController = curBobber.GetComponent<BobberController>();
                                        bobberController.SetDestination(
                                            (mousePosition - (Vector2)transform.parent.position).normalized * castAngle / 15
                                            + (Vector2)transform.parent.position);
                                        bobberController.parentRod = this;
                                        bobberController.bounds = bounds;
                                        bobberController.gameController = gameController;
                                }
                                // Not charged enough.
                                else {
                                        castState = 0;
                                        castAngle = 0;

                                        // Set player state to idle
                                        gameController.SetState(0);
                                }
                        }
                }

                // Release
                if (castState == 2) {
                        // Drawing fishing line and setting angle
                        fishingLine.enabled = true;

                        //Animation for pulling
                        if (Input.GetMouseButton(0)) {
                                castAngle = Mathf.Lerp(castAngle, 50, 0.3f);
                        }
                        else
                        {
                                castAngle = Mathf.Lerp(castAngle, 0, 0.1f);
                        }
                        fishingLine.SetPosition(0, transform.position + 
                            new Vector3(Mathf.Cos(Mathf.Deg2Rad * (castAngle + 45)) * reflectState / 1.5f, 
                            Mathf.Sin(Mathf.Deg2Rad * (castAngle + 45)) / 1.5f));
                        fishingLine.SetPosition(1, curBobber.transform.position);
                }

                // Reeling in
                if (castState == -1) {
                        castState = 0;
                        fishingLine.enabled = false;

                        // Check state of bobber
                        bobberController.Pull();
                        // Destroy bobber
                        Destroy(curBobber);
                }

                // Reset angle
                if (castState == 0) {
                        castAngle = Mathf.Lerp(castAngle, 0, 0.1f);
                }
                transform.rotation = Quaternion.Euler(Vector3.forward * castAngle * reflectState);
        }
        // Cast fishing rod.
        public void Cast() {
                // Starting up cast.
                if (castState == 0) {
                        castAngle = 0;
                        castState = 1;
                }

                // Reeling back in
                if (castState == 2) {
                        if (clampedFish == null)
                        {
                                Reset();
                        }
                        // Call fish's reel mechanic
                        else {
                                gameController.SetState(5);
                                clampedFish.Reel();
                        }
                }

        }
        // Reset function that sets fishing rod back to normal state.
        public void Reset()
        {
                castState = -1;
                // Set state back to idle
                gameController.SetState(0);
                clampedFish = null;
        }
        public void SetBounds(float[] newBounds) {
                bounds = newBounds;
        }
        // Set the sprite to match the state
        public void SetSprite(int state) {
                if (state == 0)
                {
                        spriteRenderer.sprite = sprRod;
                        transform.localScale = new Vector3(2, 2, 1);

                }
                if (state == 1)
                {
                        spriteRenderer.sprite = sprRepairs;
                        transform.localScale = new Vector3(0.75f, 0.75f, 1);
                }
        }
}
