using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemController : MonoBehaviour
{
        public GameController gameController;
        public BoatController boatController;
        private FishParentController clampedFish;
        private BobberController bobberController;
        private SpriteRenderer spriteRenderer;
        
        private int castState = 0;
        private float castAngle;
        public int reflectState = 1;

        // Fishing bobber and line
        public GameObject bobber;
        private GameObject curBobber;
        private LineRenderer fishingLine;

        // Casting marker
        public GameObject markerPrefab;
        private GameObject marker;

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

                // landing marker
                marker = Instantiate(markerPrefab, Vector2.zero, Quaternion.identity);
                marker.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
                // Facing direction
                spriteRenderer.flipX = (reflectState == 1) ? false : true;
                // Charging up cast.
                if (castState == 1) {


                        // Mouse position.
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        castAngle = Mathf.Min(castAngle + 45 * Time.deltaTime, 90);

                        // Updating marker
                        Vector2 velocity = (mousePosition - (Vector2)transform.parent.position).normalized *
                            castAngle / 15 * 1.5f;
                        marker.transform.position = transform.parent.position +
                            (Vector3)velocity.normalized * Mathf.Pow(velocity.magnitude, 2) / 12;

                        // Releasing cast.
                        if (Input.GetMouseButtonUp(0)) {
                                // Minimum chargeup.
                                if (castAngle > 30)
                                {
                                        // Set player state to fishing
                                        gameController.SetState(2);
                                        castState = 2;
                                        // Create bobber
                                        curBobber = Instantiate(bobber, transform.parent.position, Quaternion.identity);
                                        bobberController = curBobber.GetComponent<BobberController>();
                                        bobberController.SetDestination((mousePosition - (Vector2)transform.parent.position)
                                                .normalized * castAngle / 15 + (Vector2)transform.parent.position);

                                        bobberController.parentRod = this;
                                        bobberController.boatController = boatController;
                                        bobberController.gameController = gameController;
                                }
                                // Not charged enough.
                                else {
                                        castState = 0;
                                        castAngle = 0;

                                        // Set player state to idle
                                        gameController.SetState(0);
                                }

                                // Disable marker
                                marker.SetActive(false);
                        }
                }

                // Release
                if (castState == 2) {
                        // Drawing fishing line and setting angle
                        fishingLine.enabled = true;

                        //Animation for pulling
                        if (Input.GetMouseButton(0)) {
                                castAngle = Mathf.Lerp(castAngle, 50, 0.05f) + Random.Range(-3, 3);
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
                        if (bobberController != null)
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

                        // Activate marker
                        marker.SetActive(true);
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
                                clampedFish.Reel(this);
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

                // Additional case for when stopping mid cast
                marker.SetActive(false);
        }
        // Releases fish or resets rod without reeling
        public void Release() {
                if (clampedFish != null)
                {
                        clampedFish.Release(this);
                }
                else {
                        Reset();
                }
                         
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
                if (state == -1)
                {
                        spriteRenderer.sprite = null;
                }
        }

        // Setter method for fish
        public void SetFish(FishParentController clampedFish) {
                this.clampedFish = clampedFish;
        }
}
