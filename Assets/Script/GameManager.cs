using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the <see cref="GameManager" />
/// </summary>
public class GameManager:MonoBehaviour
{
    // Prefabs
    /// <summary>
    /// Keeping all game windows
    /// </summary>
    public Transform[] UIMenu;

    /// <summary>
    /// Number table
    /// </summary>
    public Transform numberGrid;

    /// <summary>
    /// Keep all instantiated numbers
    /// </summary>
    public Transform[] numbers;

    /// <summary>
    /// Number prefab
    /// </summary>
    public Transform numberPrefab;

    /// <summary>
    /// Main menu remained number label
    /// </summary>
    public Text remainNumberLabel;

    /// <summary>
    /// Options menu cut prize buttons
    /// </summary>
    public Transform[] cutButtons = new Transform[8];

    /// <summary>
    /// Options menu tombola prize cut label
    /// </summary>
    public Text tombolaCut;

    /// <summary>
    /// Options menu cut prize buttons image 0:passive 1:active
    /// </summary>
    public Sprite[] cutImages = new Sprite[2];

    /// <summary>
    /// Main menu add player button
    /// </summary>
    public InputField addPlayerName;

    /// <summary>
    /// Player scrool viewer
    /// </summary>
    public ScrollRect scroolViewer;

    /// <summary>
    /// Player list prefab
    /// </summary>
    public Transform playerGridPrefab;

    /// <summary>
    /// Scrool view content
    /// </summary>
    public Transform playerScroolViewContent;

    /// <summary>
    /// Player info window canvas transformations
    /// 0:Info label(Left)
    /// 1:Info label(Righ)
    /// 2:Buy ticket button
    /// 3:Withdraw button
    /// 4:Player name label
    /// 5:Remove player button
    /// </summary>
    public Transform[] playerUIPanelTransforms;

    /// <summary>
    /// UI Messages
    /// </summary>
    public Text[] menuMessages;

    /// <summary>
    /// Main menu start button
    /// </summary>
    public Transform startGameButton;

    /// <summary>
    /// Main menu number board
    /// </summary>
    public Transform randomNumberBoard;

    /// <summary>
    /// Ticket price menu input field
    /// </summary>
    public Transform ticketMoneyInput;

    /// <summary>
    /// Main menu ticket price label
    /// </summary>
    public Transform ticketPriceLabelMainMenu;

    /// <summary>
    /// Main menu total cash label
    /// </summary>
    public Transform totalCashLabelMainMenu;

    /// <summary>
    /// Main menu big prize label
    /// </summary>
    public Transform bigPrizeMainMenu;

    /// <summary>
    /// Variables
    /// </summary>
    protected float numberPrefabWidth = 95.0f;// Number prefab size
    protected int maxNumber = 90;// Total number from table
    protected int row = 9;// Total number of row from table
    protected int col = 10;// Total number of column from table
    protected int remainNumber;// Total remaining number count
    protected int firstCinq = 10;// First cinquia prize cut
    protected int secondCinq = 40;// Second cinquia prize cut
    protected int playerLastId = 1;// Player Id
    protected List<Player> players;// Keep generated players
    protected List<Transform> playerGridList = new List<Transform>();// Keep contents in a list
    protected bool isGameStarted = false;// Flag for game running state
    protected bool isRandomActive = false;// Flag for random number state
    protected bool isResetActive = false;// Flag for reset state
    protected List<int> selectedNumbers = new List<int>();// Selected numbers
    protected List<int> notSelectedNumbers = new List<int>();// Available numbers
    protected decimal ticketPrice = 5m;// Total ticket price
    protected decimal totalCash = 0m;// Total cash
    protected int[] awardPlayers = new int[3];// 0:cinq1 1:cinq2 2:tombola set player id
    protected int currentAward = 0;// Keep current award state

    /// <summary>
    /// Use this for initialization
    /// </summary>
    internal void Start()
    {
        players = new List<Player>();
        numbers = new Transform[maxNumber];
        PrepareNumberGrid();
        UpdateMainMenuLabels();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    internal void Update()
    {
    }

    /// <summary>
    /// Start game
    /// </summary>
    public void StartGame()
    {
        if(!isGameStarted)
        {
            if(players.Count > 0)
            {
                menuMessages[4].text = "";  // Clear main menu message box
                isGameStarted = true;       // Set game state
                isRandomActive = true;      // Set random number generate flag
                isResetActive = false;      // Set reset status

                // Change main menu start button
                startGameButton.GetComponentInChildren<Text>().text = "Take Number";
                startGameButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                startGameButton.GetComponentInChildren<Button>().onClick.AddListener(() => { RandomNumberGenerate(); });

                // Update main menu labels
                UpdateMainMenuLabels();
            }
            else
            {
                // Show message
                menuMessages[4].text = "Please add new player.";
            }
        }
    }

    /// <summary>
    /// Stop game
    /// </summary>
    public void StopGame()
    {
        isGameStarted = false;  // Set game state
        isRandomActive = false; // Set random number generate flag
        isResetActive = true;   // Set reset status
        currentAward = 0;

        // Change main menu start button
        startGameButton.GetComponentInChildren<Text>().text = "Reset Table";
        startGameButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        startGameButton.GetComponentInChildren<Button>().onClick.AddListener(() => { ResetNumbers(); });

        // Update main menu labels
        UpdateMainMenuLabels();
    }

    /// <summary>
    /// Reset numbers
    /// </summary>
    public void ResetNumbers()
    {
        // Clear menu message box
        menuMessages[4].GetComponent<Text>().text = "";

        // Destroy previously created objects
        for(int i = 0; i < numbers.Length; i++)
        {
            Destroy(numbers[i].gameObject);
        }

        // Clear all awards from players
        for(int x = 0; x < awardPlayers.Length; x++)
        {
            int awardedPlayerGridIndex = FindPlayerIndex(awardPlayers[x]);
            if(awardedPlayerGridIndex >= 0)
            {
                playerGridList[awardedPlayerGridIndex].transform.GetChild(5 + x).transform.GetComponent<Button>()
         .GetComponent<Image>().color = new Color32(0, 0, 100, 10);
            }
        }
        for(int i = 0; i < awardPlayers.Length; i++)
        {
            awardPlayers[i] = 0;
        }

        // Prepare number table
        PrepareNumberGrid();

        // Change flags
        isGameStarted = false;
        isResetActive = false;
        isRandomActive = false;

        // Change button
        startGameButton.GetComponentInChildren<Text>().text = "Start Game";
        startGameButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        startGameButton.GetComponentInChildren<Button>().onClick.AddListener(() => { StartGame(); });
    }

    /// <summary>
    /// UICanvas Handle
    /// </summary>
    /// <param name="i">The i<see cref="int"/></param>
    public void ActivateMenu(int i)
    {
        UIMenu[i].gameObject.SetActive(true);
    }

    /// <summary>
    /// The DeactivateMenu
    /// </summary>
    /// <param name="i">The i<see cref="int"/></param>
    public void DeactivateMenu(int i)
    {
        UIMenu[i].gameObject.SetActive(false);
        // Clear messages
        if(i == 0)
        {
            menuMessages[0].text = "";
        }
        if(i == 2)
        {
            menuMessages[2].text = "";
        }
        if(i == 3)
        {
            ticketMoneyInput.GetComponent<Text>().text = "";
        }
        if(i == 4)
        {
            ClearActionListeners();
        }
        UpdateMainMenuLabels();
    }

    /// <summary>
    /// The Clear action listeners from buttons
    /// </summary>
    public void ClearActionListeners()
    {
        UIMenu[4].GetComponentInChildren<Button>().onClick.RemoveAllListeners(); // Deposit money button clear
        playerUIPanelTransforms[2].GetComponent<Button>().onClick.RemoveAllListeners(); // Clear buy ticket button
        playerUIPanelTransforms[3].GetComponent<Button>().onClick.RemoveAllListeners(); // Clear withdraw money button
        playerUIPanelTransforms[6].GetComponent<Button>().onClick.RemoveAllListeners(); // Clear remove button action
    }

    // 
    /// <summary>
    /// Update main menu labels
    /// </summary>
    public void UpdateMainMenuLabels()
    {
        totalCashLabelMainMenu.transform.GetComponent<Text>().text = "" + totalCash + "$";
        ticketPriceLabelMainMenu.transform.GetComponent<Text>().text = "" + ticketPrice + "$";
        bigPrizeMainMenu.GetComponent<Text>().text = totalCash * (100 - firstCinq - secondCinq) / 100 + "$";
    }

    /// <summary>
    /// Prepare number grid(table)
    /// </summary>
    internal void PrepareNumberGrid()
    {
        // Clear and set numbers
        selectedNumbers.Clear();
        notSelectedNumbers.Clear();
        for(int i = 1; i <= 90; i++)
        { notSelectedNumbers.Add(i); }

        // Set variables and prefabs
        remainNumber = maxNumber;
        remainNumberLabel.GetComponent<Text>().text = "" + remainNumber;
        menuMessages[4].text = "";

        // Update main menu labels
        UpdateMainMenuLabels();

        // Set all numbers on the table
        if(numberGrid != null && numberPrefab != null)
        {
            float spaceBetweenNumbers = 10f;
            float yPos = -10.0f;
            float numberGridWidth = numberGrid.transform.GetComponent<RectTransform>().sizeDelta.x;
            int numberCount = 0;
            for(int r = 0; r < row; r++)
            {
                float xPos = 0.0f;
                for(int c = 0; c < col; c++)
                {
                    Transform t = Instantiate(numberPrefab, numberGrid.transform) as Transform;
                    xPos += spaceBetweenNumbers;
                    t.SetParent(numberGrid.transform);
                    t.localPosition = new Vector3(xPos - numberGridWidth, yPos, 0.0f);
                    t.GetComponent<Image>().color = new Color32(199, 199, 199, 100);
                    int n = numberCount + 1;
                    t.gameObject.GetComponent<Button>().onClick.AddListener(() => { UpdateButton(n); });
                    t.name = "Number " + n;
                    t.GetComponentInChildren<Text>().text = "" + n;
                    xPos += spaceBetweenNumbers + numberPrefabWidth;
                    numbers[numberCount] = t;
                    numberCount++;
                }
                yPos -= numberPrefabWidth + spaceBetweenNumbers;
            }
        }
        else
        {
            // Debug.Log("Error: Prefabs doesn't exist.");
        }
    }

    /// <summary>
    /// Update number button
    /// </summary>
    /// <param name="btnNumber">The btnNumber<see cref="int"/></param>
    public void UpdateButton(int btnNumber)
    {
        menuMessages[4].GetComponent<Text>().text = "";
        if(isGameStarted)
        {
            Color32 c = numbers[btnNumber - 1].GetComponent<Image>().color; // Get component color
            // Make darker
            if(c.a > 200)
            {
                numbers[btnNumber - 1].GetComponent<Image>().color = new Color32(199, 199, 199, 100);
                remainNumber++;
                int selectedButtonIndex = FindNumberInList(selectedNumbers, btnNumber);
                selectedNumbers.RemoveAt(selectedButtonIndex);
                notSelectedNumbers.Add(btnNumber);
            }
            // Make brighter
            else
            {
                numbers[btnNumber - 1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                remainNumber--;

                int notSelectedButtonIndex = FindNumberInList(notSelectedNumbers, btnNumber);
                notSelectedNumbers.RemoveAt(notSelectedButtonIndex);
                selectedNumbers.Add(btnNumber);
            }
            remainNumberLabel.GetComponent<Text>().text = "" + notSelectedNumbers.Count;

            // Check game status
            if(notSelectedNumbers.Count <= 0)
            {
                StopGame();
            }
        }
        else
        { menuMessages[4].text = "You have to start game."; }
    }

    /// <summary>
    /// Update players list
    /// </summary>
    public void UpdatePlayerGrid()
    {
        for(int i = 0; i < playerGridList.Count; i++)
        {
            playerGridList[i].transform.GetChild(4).GetComponent<Text>().text = "" + players[i].getCredit() + "$";

        }
        for(int x = 0; x < currentAward; x++)
        {
            int awardedPlayerGridIndex = FindPlayerIndex(awardPlayers[x]);
            if(awardedPlayerGridIndex != -1){
                playerGridList[awardedPlayerGridIndex].transform.GetChild(5 + x).transform.GetComponent<Button>().GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    /// <summary>
    /// Show award canvas
    /// </summary>
    private void ShowAwardCanvas()
    {
        // Update Canvas
        for(int i = 0; i < awardPlayers.Length; i++)
        {
            int playerIndexID = FindPlayerIndex(awardPlayers[i]);
            UIMenu[5].transform.GetChild(0).GetChild(i + 1).GetComponent<Text>().text = "" + players[playerIndexID].getName();
            if(i == 0)
            {
                decimal playerCut = totalCash * firstCinq / 100;
                players[playerIndexID].increaseCredit(playerCut);
                players[playerIndexID].increaseTotalWonPrize(playerCut);
                UIMenu[5].transform.GetChild(0).GetChild(i + 4).GetComponent<Text>().text = "1st Cinquina\n" + playerCut + "$ (%" + firstCinq + ")";
            }
            if(i == 1)
            {
                decimal playerCut = totalCash * secondCinq / 100;
                players[playerIndexID].increaseCredit(playerCut);
                players[playerIndexID].increaseTotalWonPrize(playerCut);
                UIMenu[5].transform.GetChild(0).GetChild(i + 4).GetComponent<Text>().text = "2nd Cinquina\n" + playerCut + "$ (%" + secondCinq + ")";
            }
            if(i == 2)
            {
                decimal tomCut = 100 - firstCinq - secondCinq;
                decimal playerCut = totalCash * tomCut / 100;
                players[playerIndexID].increaseCredit(playerCut);
                players[playerIndexID].increaseTotalWonPrize(playerCut);
                UIMenu[5].transform.GetChild(0).GetChild(i + 4).GetComponent<Text>().text = "Tombola\n" + playerCut + "$ (%" + tomCut + ")";
            }
        }
        decimal playerCut1 = totalCash * firstCinq / 100;
        decimal playerCut2 = totalCash * secondCinq / 100;
        decimal tomCut1 = 100 - firstCinq - secondCinq;
        totalCash = totalCash - playerCut1 - playerCut2 - tomCut1;
        totalCash = 0;
        UpdatePlayerGrid();
        ActivateMenu(5);
    }

    /// <summary>
    /// Set player info window
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    public void SetPlayerMenu(int playerID)
    {
        ClearActionListeners();
        int playerIDIndex = FindPlayerIndex(playerID);
        if(playerIDIndex != -1)
        {
            playerUIPanelTransforms[5].GetComponent<Text>().text = players[playerIDIndex].getName();  // Player name
            playerUIPanelTransforms[0].GetComponent<Text>().text = "Total ticket bought: "
                + players[playerIDIndex].getBoughtTicket() + "\nTotal spent: "
                + players[playerIDIndex].getMoneySpent() + "$\nOverall: "
                + (-players[playerIDIndex].getMoneySpent() + players[playerIDIndex].getTotalWonPrize())
                + "$\nCredit: " + players[playerIDIndex].getCredit() + "$";
            playerUIPanelTransforms[1].GetComponent<Text>().text = "1st Cinque: " + players[playerIDIndex].getCinq1() + "\n2nd Cinque: " + players[playerIDIndex].getCinq2()
                + "\nTombola: " + players[playerIDIndex].getTombola() + "\nPrize won: " + players[playerIDIndex].getTotalWonPrize() + "$";
            playerUIPanelTransforms[2].GetComponent<Button>().onClick.AddListener(() => { BuyTicketToPlayer(playerID); }); // Buy ticket button
            playerUIPanelTransforms[3].GetComponent<Button>().onClick.AddListener(() => { WithdrawMoney(playerID); }); // Withdraw all credit button
            UIMenu[4].GetComponentInChildren<Button>().onClick.AddListener(() => { AddMoneyToPlayer(playerID, decimal.Parse(UIMenu[4].GetChild(0).GetChild(1).GetComponent<InputField>().text)); });
            playerUIPanelTransforms[6].GetComponent<Button>().onClick.AddListener(() => { RemovePlayer(playerID); }); // Remove player action
        }
        ActivateMenu(2);  // Activate menu
    }

    /// <summary>
    /// Set prize cuts
    /// </summary>
    /// <param name="value">The value<see cref="int"/></param>
    public void setFirstCinq(int value)
    {
        if(!isGameStarted)
        {
            int tombola = 100 - secondCinq - value;
            if(value > secondCinq || tombola <= secondCinq)
            {
                menuMessages[0].text = "You can't select bigger prize.";
            }
            else
            {
                firstCinq = value;
                tombolaCut.text = "" + tombola;
                menuMessages[0].text = "";
                if(value == 10)
                {
                    cutButtons[0].transform.GetComponent<Image>().sprite = cutImages[1];
                    cutButtons[1].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[2].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[3].transform.GetComponent<Image>().sprite = cutImages[0];
                }
                if(value == 15)
                {
                    cutButtons[0].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[1].transform.GetComponent<Image>().sprite = cutImages[1];
                    cutButtons[2].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[3].transform.GetComponent<Image>().sprite = cutImages[0];
                }
                if(value == 20)
                {
                    cutButtons[0].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[1].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[2].transform.GetComponent<Image>().sprite = cutImages[1];
                    cutButtons[3].transform.GetComponent<Image>().sprite = cutImages[0];
                }
                if(value == 25)
                {
                    cutButtons[0].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[1].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[2].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[3].transform.GetComponent<Image>().sprite = cutImages[1];
                }
            }
            UpdateMainMenuLabels();
        }
        else
        {
            menuMessages[0].text = "Game started. You can't change.";
        }
    }

    /// <summary>
    /// Set prize cuts
    /// </summary>
    /// <param name="value">The value<see cref="int"/></param>
    public void setSecondCinq(int value)
    {
       if(!isGameStarted)
        {
            int tombola = 100 - firstCinq - value;
            if(value < firstCinq || tombola <= value)
            {
                menuMessages[0].text = "You can't select bigger prize.";
            }
            else
            {
                menuMessages[0].text = "";
                secondCinq = value;
                tombolaCut.text = "" + tombola;
                if(value == 20)
                {
                    cutButtons[4].transform.GetComponent<Image>().sprite = cutImages[1];
                    cutButtons[5].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[6].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[7].transform.GetComponent<Image>().sprite = cutImages[0];
                }
                if(value == 25)
                {
                    cutButtons[4].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[5].transform.GetComponent<Image>().sprite = cutImages[1];
                    cutButtons[6].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[7].transform.GetComponent<Image>().sprite = cutImages[0];
                }
                if(value == 35)
                {
                    cutButtons[4].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[5].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[6].transform.GetComponent<Image>().sprite = cutImages[1];
                    cutButtons[7].transform.GetComponent<Image>().sprite = cutImages[0];
                }
                if(value == 40)
                {
                    cutButtons[4].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[5].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[6].transform.GetComponent<Image>().sprite = cutImages[0];
                    cutButtons[7].transform.GetComponent<Image>().sprite = cutImages[1];
                }
            }
            UpdateMainMenuLabels();
        }
        else
        {
            menuMessages[0].text = "You can't change.";
        }
    }

    /// <summary>
    /// Find number index in a list
    /// </summary>
    /// <param name="_list">The _list<see cref="List{int}"/></param>
    /// <param name="num">The num<see cref="int"/></param>
    /// <returns>The <see cref="int"/></returns>
    private int FindNumberInList(List<int> _list, int num)
    {
        for(int i = 0; i < _list.Count; i++)
        {
            if(_list[i] == num)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Find player index
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    /// <returns>The <see cref="int"/></returns>
    public int FindPlayerIndex(int playerID)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].getID() == playerID)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Random number generate
    /// </summary>
    public void RandomNumberGenerate()
    {
        if(notSelectedNumbers.Count > 0)
        {
            StartCoroutine(RandomNumberGenerateCall());
        }
    }

    /// <summary>
    /// Random number generator coroutine
    /// </summary>
    /// <returns>The <see cref="IEnumerator"/></returns>
    private IEnumerator RandomNumberGenerateCall()
    {
        if(isGameStarted && notSelectedNumbers.Count > 0 && isRandomActive)
        {
            float startTime = Time.deltaTime;
            int selectedNumber = 0;
            int elapse = 20;
            isRandomActive = false;
            while(elapse > 0)
            {
                selectedNumber = notSelectedNumbers[Random.Range(0, notSelectedNumbers.Count - 1)];
                if(notSelectedNumbers.Count > 10)
                {
                    randomNumberBoard.GetComponentInChildren<Text>().text = "" + selectedNumber;
                }
                else
                {
                    randomNumberBoard.GetComponentInChildren<Text>().text = "Wait";
                }
                yield return new WaitForSeconds(0.2f);
                elapse--;
            }
            randomNumberBoard.GetComponentInChildren<Text>().text = "" + selectedNumber;
            UpdateButton(selectedNumber);
            isRandomActive = true;
        }
    }

    /// <summary>
    /// Add player
    /// </summary>
    public void AddPlayer()
    {
        if(!isGameStarted)
        {
            string pName = addPlayerName.text;
            if(pName.Length != 0 && pName != " ")
            {
                players.Add(new Player(playerLastId, pName));
                if(playerGridList.Count == 0)
                {
                    Transform u = Instantiate(playerGridPrefab, playerScroolViewContent.transform) as Transform;
                    u.name = pName + " " + playerLastId;
                    u.transform.GetComponentInChildren<Text>().text = "[" + playerLastId + "] " + pName;
                    int playerIDNumber = playerLastId;
                    u.transform.GetChild(1).transform.GetComponent<Button>().onClick.AddListener(() => { SetPlayerMenu(playerIDNumber); });
                    u.transform.GetChild(7).transform.GetComponent<Button>().onClick.AddListener(() => { SetAwardToPlayer(playerIDNumber, 2); });
                    u.transform.GetChild(6).transform.GetComponent<Button>().onClick.AddListener(() => { SetAwardToPlayer(playerIDNumber, 1); });
                    u.transform.GetChild(5).transform.GetComponent<Button>().onClick.AddListener(() => { SetAwardToPlayer(playerIDNumber, 0); });
                    playerGridList.Add(u);
                }
                else
                {
                    Transform u = Instantiate(playerGridPrefab, playerScroolViewContent.transform) as Transform;
                    u.name = pName + " " + playerLastId;
                    u.transform.GetComponentInChildren<Text>().text = "[" + playerLastId + "] " + pName;
                    int playerIDNumber = playerLastId;
                    u.transform.GetChild(1).transform.GetComponent<Button>().onClick.AddListener(() => { SetPlayerMenu(playerIDNumber); });
                    u.transform.GetChild(7).transform.GetComponent<Button>().onClick.AddListener(() => { SetAwardToPlayer(playerIDNumber, 2); });
                    u.transform.GetChild(6).transform.GetComponent<Button>().onClick.AddListener(() => { SetAwardToPlayer(playerIDNumber, 1); });
                    u.transform.GetChild(5).transform.GetComponent<Button>().onClick.AddListener(() => { SetAwardToPlayer(playerIDNumber, 0); });
                    playerGridList.Add(u);
                }
                playerLastId++;
            }
            addPlayerName.text = "";
            UpdateMainMenuLabels();
        }
        else
        {
            menuMessages[4].text = "You can't add new player.";
        }
    }

    /// <summary>
    /// Set award as player id
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    /// <param name="awardID">The awardID<see cref="int"/></param>
    public void SetAwardToPlayer(int playerID, int awardID)
    {
        if(isGameStarted == true)
        {
            if(currentAward == awardID)
            {
                awardPlayers[awardID] = playerID;
                currentAward++;
                UpdatePlayerGrid();
            }
            if(currentAward >= awardPlayers.Length)
            {
                StopGame();

                // Show score board and share prize
                ShowAwardCanvas();
            }
        }
    }

    /// <summary>
    /// Set ticket price
    /// </summary>
    public void SetTicketPrice()
    {
        if(!isGameStarted)
        {
            ticketPrice = decimal.Parse(ticketMoneyInput.GetComponent<Text>().text);
        }
        else
        {
            menuMessages[4].GetComponent<Text>().text = "Game started. You can't change ticket price.";
        }
        DeactivateMenu(3);
        UpdateMainMenuLabels();
    }

    /// <summary>
    /// Deposit money
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    /// <param name="money">The money<see cref="decimal"/></param>
    public void AddMoneyToPlayer(int playerID, decimal money)
    {
        if(!isGameStarted)
        {
            int playerIndex = FindPlayerIndex(playerID);
            if(playerIndex != -1)
            {
                players[playerIndex].increaseCredit(money);
                menuMessages[2].text = money + "$ added to player " + players[playerIndex].getName();
            }
            else
            {
                menuMessages[2].text = money + "Player doesn't exist.";
            }
        }
        else
        {
            menuMessages[2].text = "Game had been started, you can't do any action.";
        }

        UIMenu[4].GetChild(0).GetChild(1).GetComponent<InputField>().text = "";
        UIMenu[4].gameObject.SetActive(false);
        SetPlayerMenu(playerID);
        UpdatePlayerGrid();
    }

    /// <summary>
    /// Buy ticket
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    public void BuyTicketToPlayer(int playerID)
    {
        if(!isGameStarted)
        {
            int playerIndex = FindPlayerIndex(playerID);
            if(playerIndex != -1)
            {
                if(players[playerIndex].getCredit() >= ticketPrice)
                {
                    players[playerIndex].increaseMoneySpent(ticketPrice);
                    players[playerIndex].decreaseCredit(ticketPrice);
                    players[playerIndex].increaseBoughtTicket();
                    menuMessages[2].text = "Player bought a ticket.";
                    totalCash = totalCash + ticketPrice;
                }
                else
                {
                    menuMessages[2].text = "Player has not enough money.";
                }
            }
            else
            {
                menuMessages[2].text = "Player doesn't exist.";
            }
        }
        else
        {
            menuMessages[2].text = "Game had been started, you can't buy ticket.";
        }
        SetPlayerMenu(playerID);
        UpdatePlayerGrid();
    }

    /// <summary>
    /// Withdraw money
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    public void WithdrawMoney(int playerID)
    {
        if(!isGameStarted)
        {
            int playerIndex = FindPlayerIndex(playerID);
            if(playerIndex != -1)
            {
                players[playerIndex].setCredit(0);
                menuMessages[2].text = "Withdrawed all money from " + players[playerIndex].getName() + "\'s account.";
            }
            else
            {
                menuMessages[2].text = "Player doesn't exist.";
            }
        }
        else
        {
            menuMessages[2].text = "Game had been started, you can't withdraw money.";
        }
        SetPlayerMenu(playerID);
        UpdatePlayerGrid();
    }

    /// <summary>
    /// Remove player
    /// </summary>
    /// <param name="playerID">The playerID<see cref="int"/></param>
    public void RemovePlayer(int playerID)
    {
        if(!isGameStarted)
        {
            // Clear
            ClearActionListeners();
            UpdatePlayerGrid();

            // Remove playef
            int playerIndex = FindPlayerIndex(playerID);
            players.RemoveAt(playerIndex);
            Destroy(playerGridList[playerIndex].gameObject);
            playerGridList.RemoveAt(playerIndex);
            DeactivateMenu(2);
        }
        else
        {
            menuMessages[4].text = "Cannot remove player";
        }
    }
}
