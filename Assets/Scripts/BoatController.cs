using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Timeline;

public class BoatController : MonoBehaviour
{


        private GameObject[] holes;
        public GameObject holePrefab;
        public GameObject progressbarPrefab;
        public GameController gameController;
        public PlayerController playerController;
        public GameObject marker;

        // Tile management
        private int xScale = 10;
        private int yScale = 5;

        private GameObject[] tiles;
        private bool[] bfs;
        public GameObject tilePrefab;

        // Variables for holes
        private int holeNum = 0;
        private int tileTotal;
        private int tileMade = 0;
        private float fixTimer = 0;
        private float fixTimerTotal = 2;
        private bool fixState = false;
        private int fixPos;
        private GameObject curBar;

        // Shake effects
        private float _timer;
        private Vector3 _randomPos;

        public float _time = 0.2f;
        public float _distance = 0.1f;
        public float _delayBetweenShakes = 0f;

        // Start is called before the first frame update
        void Start()
        {

                // Subtract one position for repair materials
                tileTotal = (2 * xScale + 1) * (2 * yScale + 1);
                holes = new GameObject[tileTotal];

                // Instantiating tiles
                tiles = new GameObject[tileTotal];
                

                for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                                GameObject temp = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity);
                                temp.transform.SetParent(transform);
                                SetTile(i, j, temp);
                                tileMade++;
                        }
                }

                CheckTiles();

                marker.SetActive(false);
                // SKip over the central tile;
                tileMade--;
        }

        // Update is called once per frame
        void Update()
        {

                if (Input.GetKeyDown(KeyCode.O)) {
                        StartCoroutine(Shake());
                }
                // Fixing
                if (fixState) {
                        fixTimer += Time.deltaTime;

                        // TODO: draw some sort of progress bar.

                        // Premature release
                        if (Input.GetKeyUp(KeyCode.R) ||
                            ConvTile(Mathf.RoundToInt(playerController.transform.position.x),    // Checking if they leave the vacinity
                            Mathf.RoundToInt(playerController.transform.position.y)) != fixPos) {
                                Reset();

                                // Update state
                                gameController.SetState(0);

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
                if (holeNum == tileMade)
                        return false;
                int newHole = 0;
                int center = ConvTile(0, 0);
                // iterate through null elements
                // TODO: Check the randomness, seems to be off
                for (int i = 0; i < UnityEngine.Random.Range(0, tileMade - holeNum - 1); i++) {
                        // Skip over nonexistant tiles and places where there are already holes
                        while (newHole == center || tiles[newHole] == null || holes[newHole] != null) newHole++;
                        newHole++;
                }
                while (newHole == center || tiles[newHole] == null || holes[newHole] != null) newHole++;

                // Create corresponding hole

                // TODO: fix this location calculation
                holes[newHole] = Instantiate(holePrefab, new Vector2(newHole % (2 * xScale + 1) - xScale,
                    newHole / (2 * xScale + 1) - yScale), Quaternion.identity);
                holes[newHole].GetComponent<HoleController>().boatController = this;
                holeNum++;

                return true;
        }
        /* Initiate a hole fix attempt at given location.
         * Returns true if theres a hole there and false otherwise.
         */
        public void Fix(float x, float y)
        {
                int xPos = Mathf.RoundToInt(x);
                int yPos = Mathf.RoundToInt(y);
                fixPos = ConvTile(xPos, yPos);

                // Picking up supplies
                //if (1 == 1 || fixPos == tileTotal) {
                if (gameController.GetState() != 4) { 
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
        // Helper function to cancel repair operations
        public void Reset()
        {
                Debug.Log("failed");
                fixState = false;
                fixTimer = 0;
                Destroy(curBar);

                // Update external objects      
                playerController.ItemSetSprite(0);
        }

        // Helper function to make interacting with tiles
        private void SetTile(int x, int y, GameObject newTile)
        {
                tiles[ConvTile(x, y)] = newTile;
        }

        private GameObject GetTile(int x, int y) {
                return tiles[ConvTile(x, y)];
        }

        // Returns the 1d equivalent of a tile
        private int ConvTile(int x, int y) {
                return x + xScale + (y + yScale) * (2 * xScale + 1);
        }
        // Returns the tile equivalent of a 1d
        private Tuple<int, int> DeconvTile(int val)
        {
                return Tuple.Create(val % (2 * xScale + 1) - xScale,
                    val / (2 * xScale + 1) - yScale);
        }
        public GameObject CheckPosition(float x, float y) {
                int xPos = Mathf.RoundToInt(x);
                int yPos = Mathf.RoundToInt(y);
                // Checking if out of bounds
                if (Mathf.Abs(xPos) > xScale || Mathf.Abs(yPos) > yScale) return null;
                // If position is not in tile
                return GetTile(xPos, yPos);
        }

        public void DestroyPosition(float x, float y)
        {
                int xPos = Mathf.RoundToInt(x);
                int yPos = Mathf.RoundToInt(y);

                // SKip over central tile
                if (xPos == 0 && yPos == 0) return;
                // Checking if out of bounds
                if (Mathf.Abs(xPos) > xScale || Mathf.Abs(yPos) > yScale) return;
                // Destroying that tile if it exist
                if (GetTile(xPos, yPos) != null) {
                        Destroy(GetTile(xPos, yPos));
                        SetTile(xPos, yPos, null);
                        if (holes[ConvTile(xPos, yPos)] != null)
                        {
                                Destroy(holes[ConvTile(xPos, yPos)]);
                                holes[ConvTile(xPos, yPos)] = null;
                                holeNum--;
                        }
                        CheckTiles();
                }

        }
        // Placement helper functions
        public void Place(float x, float y)
        {
                int xPos = Mathf.RoundToInt(x);
                int yPos = Mathf.RoundToInt(y);
                // Checking if out of bounds
                if (Mathf.Abs(xPos) > xScale || Mathf.Abs(yPos) > yScale) return;
                // Checking if there is already a tile
                if (GetTile(xPos, yPos) == null) {
                        // Checking if the tile is connected
                        GameObject temp = Instantiate(tilePrefab, new Vector2(xPos, yPos), Quaternion.identity);
                        temp.transform.SetParent(transform);
                        SetTile(xPos, yPos, temp);
                        tileMade++;
                        CheckTiles();
                }

                // Marker updates
                marker.SetActive(false);

        }
        public bool PlaceMarkerHelper(int x, int y)
        {
                if (Mathf.Abs(x) > xScale || Mathf.Abs(y) > yScale) {
                        return false;
                }
                return bfs[ConvTile(x, y)];
        }
        public void PlaceMarker(float x, float y)
        {

                marker.SetActive(false);
                int xPos = Mathf.RoundToInt(x);
                int yPos = Mathf.RoundToInt(y);
                // Checking if out of bounds
                if (Mathf.Abs(xPos) > xScale || Mathf.Abs(yPos) > yScale) return;
                // Checking if there is already a tile
                if (GetTile(xPos, yPos) == null)
                {

                        marker.SetActive(true);
                        // Checking if position is valid
                        if (PlaceMarkerHelper(xPos - 1, yPos) ||
                            PlaceMarkerHelper(xPos + 1, yPos) ||
                            PlaceMarkerHelper(xPos, yPos - 1) ||
                            PlaceMarkerHelper(xPos, yPos + 1)) {
                                marker.transform.position = new Vector2(xPos, yPos);
                        }
                }
        }
        // Checks that all tiles are connected somehow to the (0,0) tile
        private void CheckTiles() 
        {
                
                // Reset bfs and create queue
                Queue<int> tilesQueue = new Queue<int>();
                bfs = new bool[tileTotal];
                Tuple<int, int> curTile;
                // Add main tile to queue
                tilesQueue.Enqueue(ConvTile(0, 0));


                // Repeatedly pull from queue
                while (tilesQueue.Count != 0) {
                        int current = tilesQueue.Dequeue();
                        
                        bfs[current] = true;
                        curTile = DeconvTile(current);

                        //Debug.Log(curTile);
                        int xPos = curTile.Item1;
                        int yPos = curTile.Item2;
                        // Check all adjactent tiles
                        if (xPos > -xScale && !bfs[ConvTile(xPos - 1, yPos)] && tiles[ConvTile(xPos - 1, yPos)] != null) {
                                tilesQueue.Enqueue(ConvTile(xPos - 1, yPos));
                        }
                        if (yPos > -yScale && !bfs[ConvTile(xPos, yPos - 1)] && tiles[ConvTile(xPos, yPos - 1)] != null)
                        {
                                tilesQueue.Enqueue(ConvTile(xPos, yPos - 1));
                        }
                        if (xPos < xScale && !bfs[ConvTile(xPos + 1, yPos)] && tiles[ConvTile(xPos + 1, yPos)] != null)
                        {
                                tilesQueue.Enqueue(ConvTile(xPos + 1, yPos));
                        }
                        if (yPos < yScale && !bfs[ConvTile(xPos, yPos + 1)] && tiles[ConvTile(xPos, yPos + 1)] != null)
                        {
                                tilesQueue.Enqueue(ConvTile(xPos, yPos + 1));
                        }
                }

                DestroyTiles();
        }
        // Destroys all tiles not connected
        private void DestroyTiles() {
                for (int i = 0; i < tileTotal; i++) {
                        if (tiles[i] != null && bfs[i] == false) {
                                Destroy(tiles[i]);
                                tiles[i] = null;
                                if (holes[i] != null)
                                {
                                        Destroy(holes[i]);
                                        holes[i] = null;
                                        holeNum--;
                                }
                                tileMade--;
                        }
                }
        }


        // Shaking effect
        private IEnumerator Shake()
        {
                _timer = 0f;

                while (_timer < _time)
                {
                        _timer += Time.deltaTime;

                        _randomPos = (UnityEngine.Random.insideUnitSphere * _distance);

                        transform.position = _randomPos;

                        if (_delayBetweenShakes > 0f)
                        {
                                yield return new WaitForSeconds(_delayBetweenShakes);
                        }
                        else
                        {
                                yield return null;
                        }
                }

                transform.position = Vector2.zero;
        }
}
