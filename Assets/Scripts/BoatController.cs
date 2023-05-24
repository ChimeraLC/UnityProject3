using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

        private float xScale = 6;
        private float yScale = 3;

        private GameObject[] holes;
        public GameObject holePrefab;
        public GameObject progressbarPrefab;
        public GameController gameController;
        public PlayerController playerController;

        // Variables for holes
        private int holeNum = 0;
        private int holeTotal;
        private float fixTimer = 0;
        private float fixTimerTotal = 2;
        private bool fixState = false;
        private int fixPos;
        private GameObject curBar;
        // Start is called before the first frame update
        void Start()
        {
                transform.localScale = new Vector2(xScale, yScale);

                // Subtract one position for repair materials
                holeTotal = (int) xScale * (int) yScale - 1;
                holes = new GameObject[holeTotal];
        }

        // Update is called once per frame
        void Update()
        {
                // Fixing
                if (fixState) {
                        fixTimer += Time.deltaTime;

                        // TODO: draw some sort of progress bar.

                        // Premature release
                        if (Input.GetKeyUp(KeyCode.R) ||
                            CalcPos(playerController.transform.position.x,    // Checking if they leave the vacinity
                            playerController.transform.position.y) != fixPos) {
                                Debug.Log("failed");
                                fixState = false;
                                fixTimer = 0;
                                Destroy(curBar);

                                // Update external objects
                                gameController.SetState(0);
                                playerController.ItemSetSprite(1);

                        }


                        // Fixing hole
                        if (fixTimer > fixTimerTotal) {
                                fixTimer = 0;
                                fixState = false;

                                // Update external objects
                                gameController.SetState(0);
                                playerController.ItemSetSprite(0);

                                // Check if theres still a hole there
                                if (holes[fixPos] != null)
                                {
                                        // Destroy object and update variables
                                        Destroy(holes[fixPos]);
                                        holes[fixPos] = null;
                                        holeNum--;
                                }
                                // Update boat health
                                gameController.BoatHeal(20);
                        }
                }
        }
        /* Finds a new spot for a hole and creates a hole in that position
         * Returns true if this succeeds and false otherwise.
         */
        public bool NewHole() {
                if (holeNum == holeTotal)
                        return false;
                int newHole = 0;
                // iterate through null elements
                for (int i = 0; i < Random.Range(0, holeTotal - holeNum - 1); i++) {
                        while (holes[newHole] != null) newHole++;
                        newHole++;
                }
                while (holes[newHole] != null) newHole++;

                // Create corresponding hole
                // TODO: fix this location calculation
                holes[newHole] = Instantiate(holePrefab, new Vector2(-xScale / 2 + newHole % xScale + 0.5f,
                    -yScale / 2 + 0.5f + newHole / (int) xScale), Quaternion.identity);
                holeNum++;

                return true;
        }
        /* Initiate a hole fix attempt at given location.
         * Returns true if theres a hole there and false otherwise.
         */
        public void Fix(float x, float y) {
                fixPos = CalcPos(x, y);

                // Picking up supplies
                if (fixPos == holeTotal) {
                        gameController.SetState(4);
                        // Update item sprite
                        playerController.ItemSetSprite(1);
                }

                // Initiating a fix
                else if (holes[fixPos] != null && gameController.GetState() == 4)
                {
                        // Update state
                        gameController.SetState(3);
                        // Update internal state
                        fixState = true;
                        // Create progress bar.
                        curBar = Instantiate(progressbarPrefab, (Vector2)holes[fixPos].transform.position +
                            new Vector2(0, 1), Quaternion.identity);
                        curBar.GetComponent<ProgressBarController>().totalLifetime = fixTimerTotal;
                }
        }

        // Returns the corresponding hole position of the given position
        public int CalcPos(float x, float y) { 
                // Calculating liniarized position.
                int temp = (int)(x + xScale / 2) + (int)(y + yScale / 2) * (int)xScale;
                // Checking edges
                if (x >= (xScale / 2)) temp -= 1;
                if (y >= (yScale / 2)) temp -= (int) xScale;

                return temp;
        }
}
