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

                holeTotal = (int) xScale * (int) yScale;
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
                        if (Input.GetKeyUp(KeyCode.R)) {
                                fixState = false;
                                fixTimer = 0;
                                Destroy(curBar);
                        }

                        // Fixing hole
                        if (fixTimer > fixTimerTotal) {
                                fixTimer = 0;
                                fixState = false;
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
        public bool Fix(float x, float y) {
                fixPos = (int) (x + xScale / 2) + (int) (y + yScale / 2) * (int) xScale;
                // Todo: fix check for being at very top of boat
                if (fixPos > holeTotal) fixPos -= (int)xScale;
                if (holes[fixPos] != null)
                {
                        fixState = true;
                        curBar = Instantiate(progressbarPrefab, (Vector2)holes[fixPos].transform.position +
                            new Vector2(0, 1), Quaternion.identity);
                        curBar.GetComponent<ProgressBarController>().totalLifetime = fixTimerTotal;
                        Debug.Log(fixPos);
                }
                return (holes[fixPos] != null);
        }
}