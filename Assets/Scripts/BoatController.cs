using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

        private float xScale = 6;
        private float yScale = 3;

        private GameObject[] holes;
        public GameObject holePrefab;
        public GameController gameController;
        private int holeNum = 0;
        private int holeTotal;
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

        public bool Fix(float x, float y) {
                int pos = (int) (x + xScale / 2) + (int) (y + yScale / 2) * (int) xScale;
                // Todo: fix check for being at very top of boat
                if (pos > holeTotal) pos -= (int)xScale;
                Debug.Log(pos);
                if (holes[pos] != null) {
                        Destroy(holes[pos]);
                        holes[pos] = null;
                        holeNum--;
                }
                gameController.BoatHeal(20);
                return true;
        }
}
