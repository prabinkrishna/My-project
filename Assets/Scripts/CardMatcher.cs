using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CardMatcher : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;

    [SerializeField] private GameObject _cardContainer;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _gameWindow;
    
    [SerializeField] private TextAsset textAsset;

    [SerializeField] private TextMeshProUGUI _moveText;
    [SerializeField] private TextMeshProUGUI _matchText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _streakText;
    [SerializeField] private TextMeshProUGUI _livesRemainingText;
    [SerializeField] private TextMeshProUGUI _gameOverText;    
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;
    [Serializable]
    private class ImageData
    {
        public string id;
        public string fileName;
    }
    [Serializable]
    private class ImageDataList
    {
        public List<ImageData> images;
    }
    private int _livesRemaining ;
    private int _matchCounter =0;
    private int _streak =0;
    private int _totalMove =0;
    private int _score = 0;
    private List<GameObject> _cards = new List<GameObject>();
    private List<int> _clickCardList = new List<int>();
    private ImageDataList imageDataList;
    private int _gridColumn = 4;
    private int _gridRow = 2;
    private List<int> _imageIndexList = new List<int>();
    private int _currentRevealedCardId = -1;
    private int _currentRevealedCardIndex = -1;
    private List<int> _revealedCardArray;
    private GameData _gameData;
    private static readonly Dictionary<int,float> SCALE_FACTOR = new Dictionary<int,float>{{2,0.5f}, { 3,0.35f},{ 4,0.25f}};
    public int revealedCardCounter =0;
    private bool _isResume = false;
    void Start()
    {   
        _gameOverText.gameObject.SetActive(false);
        _gameData = new GameData();
        InitializeData();
        if(_totalMove == 0)
        {
            _isResume= false;
            _mainMenu.GetComponent<MainMenu>().Init(this);
            _mainMenu.SetActive(true);
            _game.SetActive(false);
        }
        else
        {
             _isResume= true;
            StartGame();
        }
        // StartGame();
      
    }

    public void StartGame(string level="L1")
    {

        if(!_isResume)
        {

            switch(level)
            {
                case "L1":
                    _gridColumn =3;
                    _gridRow = 2;
                    _livesRemaining = 8;
                    break;
                case "L2":
                    _gridColumn = 4;
                    _gridRow = 3;
                    _livesRemaining = 6;
                    break;
                case "L3":
                    _gridColumn = 4;
                    _gridRow = 4;
                    _livesRemaining = 4;
                    break;
            }
        }


      _mainMenu.SetActive(false);
      _game.SetActive(true);
    //    _game.SetActive(true);
       
        InitializeGame();
    }

    private void InitializeGame()
    {
        _nextButton.SetActive(false);
        _gameWindow.SetActive(true);
        SetScore();
        if (textAsset != null)
        {
            string jsonContent = textAsset.text;
            imageDataList = JsonUtility.FromJson<ImageDataList>(jsonContent);
           
          
        }
        GridLayoutGroup gridLayoutGroup = _cardContainer.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint= GridLayoutGroup.Constraint.Flexible;
       // gridLayoutGroup.constraintCount = _gridRow;

        float cellWidth = _cardContainer.GetComponent<RectTransform>().rect.width / _gridColumn;
        float cellHeight = _cardContainer.GetComponent<RectTransform>().rect.height / _gridRow;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight+30);
    
       // imageDataList.images = ShuffleList(imageDataList.images);
        int totalCards = _gridColumn * _gridRow;
        if(_imageIndexList.Count == 0)
        {
            for (int i = 0; i < totalCards/2; i++)
            {
                _imageIndexList.Add(i);
                _imageIndexList.Add(i);
            }
            ShuffleList(_imageIndexList);
        }
        for (int i = 0; i <totalCards; i++)
        {
            GameObject card = Instantiate(_cardPrefab, _cardContainer.transform);
            card.transform.localScale = Vector3.one * SCALE_FACTOR[_gridRow];
            card.transform.SetParent(_cardContainer.transform);
            Sprite sprite = Resources.Load<Sprite>("Images/" + imageDataList.images[_imageIndexList[i]].fileName);
            card.GetComponent<Card>().SetResultImage(sprite);
            card.GetComponent<Card>().Init(this);
            card.GetComponent<Card>().SetImageId(_imageIndexList[i]);
            card.GetComponent<Card>().SetCardId(i);
            _cards.Add(card);
        }
        if(_revealedCardArray != null)
        {
            InitiliazeCards();
        }
        StartCoroutine(RevealCards(0.5f));
    }
    private void InitiliazeCards()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            if(_revealedCardArray.Contains(i))
            {
                 _cards[i].GetComponent<Card>().DestroyCard();
            }
        }
        if(_currentRevealedCardIndex != -1)
        {
            _cards[_currentRevealedCardIndex].GetComponent<Card>().OnReveal();
        }
    }
    private IEnumerator RevealCards(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].GetComponent<Card>().ToggleHideCard(false);
        }
        StartCoroutine(HideCards(1f));
    }

    private IEnumerator HideCards(float delay)
    {  
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].GetComponent<Card>().ToggleHideCard(true);
        }
         InitiliazeCards();
    }

    private void InitializeData()
    {
        _streak = 0;
        string[] data = _gameData.LoadData();
        

        _totalMove =data[0] == ""? 0:int.Parse(data[0]);
        _matchCounter =data[1] == ""?0:int.Parse(data[1]);
        _currentRevealedCardId =data[2] == ""?-1:int.Parse(data[2]);
        _currentRevealedCardIndex =data[3] == ""?-1:int.Parse(data[3]);
        revealedCardCounter =data[4] == ""?0:int.Parse(data[4]);
        _gridColumn =data[5] == ""?_gridColumn:int.Parse(data[5]);
        _gridRow =data[6] == ""?_gridRow:int.Parse(data[6]);
        string str = data[7];
        string[] strArray = str.Split(new char[] { ',' });
        _revealedCardArray = ConvertToIntList(strArray);
        str= data[8];
        strArray = str.Split(new char[] { ',' });
        _score = data[9] == ""?0:int.Parse(data[9]);
        _livesRemaining = data[10] == ""?0:int.Parse(data[10]);
        _imageIndexList = ConvertToIntList(strArray);
        
    }
    private List<int> ConvertToIntList(string[] strArray)
    {
        List<int> intList = new List<int>();
        foreach (string s in strArray)
        {
            if (int.TryParse(s, out int number))
            {
                intList.Add(number);
            }
            else
            {
            //    Debug.Log("Failed to parse: " + s);
            }
        }
        return intList;
    }
  
   
    private void CheckCards(int cardId1,int cardId2)
    {

     
            _totalMove++;
            if(_imageIndexList[cardId1]== _imageIndexList[cardId2])
            {
                Debug.Log("Matched");
              
                StartCoroutine(DestroyCardsAfterDelay(cardId1,cardId2, 0.5f));
                _matchCounter++;
                _streakText.gameObject.SetActive(true);
                _streak++;
                _streakText.text = _streak+"xStreak ";
                _score++;

            }
            else
            {
              
                StartCoroutine(HideCardsAfterDelay(cardId1,cardId2, 0.5f));
                _streakText.gameObject.SetActive(false);
                _score+=_streak;
                _streak = 0;
                _livesRemaining--;
                _livesRemainingText.text = _livesRemaining+"";
                Debug.Log("Not Matched");
            }
           
     
        
        Debug.Log( "total move: "+_totalMove+"Matched: "+_matchCounter);

    }
    private IEnumerator HideCardsAfterDelay(int cardId1,int cardId2, float delay)
    {
        yield return new WaitForSeconds(delay);
        _audioSource.clip = _audioClips[2];
        _audioSource.Play();
        Debug.Log(cardId1+"Hide Cards"+cardId2);
        _cards[cardId1].GetComponent<Card>().ToggleHideCard(true);
        _cards[cardId2].GetComponent<Card>().ToggleHideCard(true);
        StartCoroutine(SelectionComplete());
    }
     private IEnumerator DestroyCardsAfterDelay(int cardId1,int cardId2, float delay)
    {
        yield return new WaitForSeconds(delay);
        _audioSource.clip = _audioClips[1];
        _audioSource.Play();
        _revealedCardArray.Add(cardId1);
        _revealedCardArray.Add(cardId2);
        _cards[cardId1].GetComponent<Card>().DestroyCard();
        _cards[cardId2].GetComponent<Card>().DestroyCard();
        StartCoroutine(SelectionComplete());
      
    }


    private IEnumerator SelectionComplete()
    {
        _currentRevealedCardId = -1;
        _currentRevealedCardIndex = -1;
        SetScore();
        revealedCardCounter =0;
        SaveData();
        if(_livesRemaining == 0)
        {
            _score =0;
            _livesRemaining = 0;
            _streak = 0;
            Debug.Log("Game Over");
            ClearGrid();
            _gameOverText.gameObject.SetActive(true);
            _gameWindow.SetActive(false);
            _audioSource.clip = _audioClips[0];
            _audioSource.Play();
            ResetData();
            yield return new WaitForSeconds(2f);
             _gameOverText.gameObject.SetActive(false);
            _game.SetActive(false);
            _mainMenu.SetActive(true);

           // ResetGame();
        }

        if(_matchCounter == _gridColumn*_gridRow/2)
        {
            Debug.Log("New Game");
            ClearGrid();
            _gameWindow.SetActive(false);
            _nextButton.SetActive(true);
           // ResetGame();
        }
    }
    private void SetScore()
    {
        _moveText.text = _totalMove+"";
        _matchText.text = _matchCounter+"";
        _livesRemainingText.text = _livesRemaining+"";
        _scoreText.text = _score+"";
    }
    private void SaveData()
    {
         _gameData.SaveData(new int[]{_totalMove,_matchCounter,_currentRevealedCardId,_currentRevealedCardIndex,revealedCardCounter,_gridColumn,_gridRow,_score,_livesRemaining},_revealedCardArray,_imageIndexList);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("All PlayerPrefs have been cleared.");
        }
    }
    private List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    private void ResetData()
    {
        
        _totalMove = 0;
        _matchCounter = 0;
        _currentRevealedCardId = -1;
        _currentRevealedCardIndex = -1;
        revealedCardCounter = 0;
        _revealedCardArray = new List<int>();
        _imageIndexList = new List<int>();
        SaveData();
    }
    private void ResetGame()
    {
       ResetData();
      //  ClearGrid();
        InitializeGame();
    }
    public void OnCardReveal( int cardId)
    {
      
        Debug.Log(cardId+"Card Id: " );
        _clickCardList.Add(cardId);
        if(_clickCardList.Count == 2)
        {
            CheckCards(_clickCardList[0],_clickCardList[1]);
            _clickCardList.Clear();
        }

       
    }

    
    public void OnNextClick()
    {
        ResetGame();
    }
    public void ClearGrid()
    {
        foreach (GameObject child in _cards)
        {
            Destroy(child);
        }
        _cards.Clear();
    }
    public void OnApplicationQuit()
    {
          SaveData();
    }

    public void OnDestroy()
    {
         SaveData();
    }


}
