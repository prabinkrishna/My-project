using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMatcher : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;

    [SerializeField] private GameObject _cardContainer;

    [System.Serializable]
    private class ImageData
    {
        public string id;
        public string fileName;
    }

    [System.Serializable]
    private class ImageDataList
    {
        public List<ImageData> images;
    }

    [SerializeField]
    private TextAsset textAsset;

    [SerializeField] private TextMeshProUGUI _moveText;

    [SerializeField] private TextMeshProUGUI _matchText;

    private int _matchCounter =0;
    private int _totalMove =0;
    private List<GameObject> _cards = new List<GameObject>();
    private ImageDataList imageDataList;
    private int _gridColumn = 4;
    private int _gridRow = 3;
    private List<int> _imageIndexList = new List<int>();
    private int _currentRevealedCardId = -1;
    private int _currentRevealedCardIndex = -1;
    public int revealedCardCounter =0;
    private List<int> _revealedCardArray;
    private GameData _gameData;
    void Start()
    {
        _gameData = new GameData();
        InitializeData();
        SetScore();
         if (textAsset != null)
        {
            string jsonContent = textAsset.text;
            imageDataList = JsonUtility.FromJson<ImageDataList>(jsonContent);
           
          
        }
        GridLayoutGroup gridLayoutGroup = _cardContainer.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint= GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = 3;
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
            card.transform.localScale = Vector3.one;
            card.transform.SetParent(_cardContainer.transform);
            Sprite sprite = Resources.Load<Sprite>("Images/" + imageDataList.images[_imageIndexList[i]].fileName);
            card.GetComponent<Card>().SetResultImage(sprite);
            card.GetComponent<Card>().Init(this);
            card.GetComponent<Card>().SetImageId(_imageIndexList[i]);
            card.GetComponent<Card>().SetCardId(i);
            _cards.Add(card);
        }
        InitiliazeCards();
    }
    public void InitiliazeCards()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            if(_revealedCardArray.Contains(i))
            {
                 _cards[i].GetComponent<Card>().DestroyCard();
            }
        }
    }
    public void InitializeData()
    {
        string[] data = _gameData.LoadData();
        

        _totalMove =data[0] == ""? 0:int.Parse(data[0]);
        _matchCounter =data[1] == ""?0:int.Parse(data[1]);
        _currentRevealedCardId =data[2] == ""?-1:int.Parse(data[2]);
        _currentRevealedCardIndex =data[3] == ""?-1:int.Parse(data[3]);
        revealedCardCounter =data[4] == ""?0:int.Parse(data[4]);
        _gridColumn =data[5] == ""?4:int.Parse(data[5]);
        _gridRow =data[6] == ""?3:int.Parse(data[6]);
        string str = data[7];
        string[] strArray = str.Split(new char[] { ',' });
        _revealedCardArray = ConvertToIntList(strArray);
        str= data[8];
        strArray = str.Split(new char[] { ',' });
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
                Debug.Log("Failed to parse: " + s);
            }
        }
        return intList;
    }
  
    public void OnCardReveal(int id, int cardId)
    {
      
        Debug.Log(cardId+"Card Id: " + id);
        
        if(_currentRevealedCardIndex == -1)
        {
            _currentRevealedCardIndex = cardId;
        }
        if(_currentRevealedCardId == -1)
        {
            _currentRevealedCardId = id;
        }
        else
        {
            _totalMove++;
            if(_currentRevealedCardId == id)
            {
                Debug.Log("Matched");
                StartCoroutine(DestroyCardsAfterDelay(cardId, 0.5f));
                _matchCounter++;

            }
            else
            {
                StartCoroutine(HideCardsAfterDelay(cardId, 0.5f));
                Debug.Log("Not Matched");
            }
           
     
        }
        Debug.Log("Card Id: " + id + "total move: "+_totalMove+"Matched: "+_matchCounter);
    }
    private IEnumerator HideCardsAfterDelay(int cardId, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(cardId+"Hide Cards"+_currentRevealedCardIndex);
        _cards[_currentRevealedCardIndex].GetComponent<Card>().HideCard();
        _cards[cardId].GetComponent<Card>().HideCard();
        SelectionComplete();
    }
     private IEnumerator DestroyCardsAfterDelay(int cardId, float delay)
    {
        yield return new WaitForSeconds(delay);

        _revealedCardArray.Add(_currentRevealedCardIndex);
        _revealedCardArray.Add(cardId);
        _cards[_currentRevealedCardIndex].GetComponent<Card>().DestroyCard();
        _cards[cardId].GetComponent<Card>().DestroyCard();
        SelectionComplete();
    }


    private void SelectionComplete()
    {
        _currentRevealedCardId = -1;
        _currentRevealedCardIndex = -1;
        SetScore();
        revealedCardCounter =0;
        SaveData();
    }
    private void SetScore()
    {
        _moveText.text = _totalMove+"";
        _matchText.text = _matchCounter+"";
    }
    private void SaveData()
    {
        _gameData.SaveData(new int[]{_totalMove,_matchCounter,_currentRevealedCardId,_currentRevealedCardIndex,revealedCardCounter,_gridColumn,_gridRow},_revealedCardArray,_imageIndexList);
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
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
     void OnApplicationQuit()
    {
          SaveData();
    }

    void OnDestroy()
    {
         SaveData();
    }


}
