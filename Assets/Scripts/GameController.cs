using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
        
        // UI elements
        public Image boatHealthBar;
        public TextMeshProUGUI debugMessage;
        public TextMeshProUGUI fishMessage;

        // Player state
        private int playerState;
        private string[] playerStates = { "idle", "casting", "fishing", "fixing" };

        // Player initialization
        public GameObject playerPrefab;
        private PlayerController playerController;

        // Boat initialization
        public GameObject boatPrefab;
        private BoatController boatController;
        // Player data
        private float boatHealth = 100f;
        private int fishCaught = 0;
        // Start is called before the first frame update
        void Start()
        {
                playerState = 0;
                playerController = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity).GetComponent<PlayerController>();
                playerController.gameController = this;
                boatController = Instantiate(boatPrefab, Vector2.zero, Quaternion.identity).GetComponent<BoatController>();
                boatController.gameController = this;
                playerController.boatController = boatController;

        }

        // Update is called once per frame
        void Update()
        {
                boatHealthBar.fillAmount = boatHealth / 100;


                // Temporary health elements
                if (Input.GetKeyDown(KeyCode.T)) {
                        boatHealth -= 20;
                        boatController.NewHole();
                }

        }
        // Set the player state indicator.
        public void SetState(int state) {
                playerState = state;
                debugMessage.text = playerStates[playerState];
        }
        // Caught a fish
        public void Caught(int fishType) {
                fishCaught++;
                fishMessage.text = fishCaught.ToString();
        }
        public void BoatHeal(int health) {
                boatHealth = Mathf.Min(100, boatHealth + health);
        }

}
