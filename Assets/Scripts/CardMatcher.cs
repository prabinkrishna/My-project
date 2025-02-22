using System.Collections;
using System.Collections.Generic;
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
    private List<GameObject> _cards = new List<GameObject>();
    private ImageDataList imageDataList;
    private int _gridColumn = 4;
    private int _gridRow = 3;
    private List<int> _imageIndexList = new List<int>();

    private int _currentRevealedCardId = -1;
    private int _currentRevealedCardIndex = -1;

    public int revealedCardCounter =0;
    void Start()
    {
         if (textAsset != null)
        {
            string jsonContent = textAsset.text;
            imageDataList = JsonUtility.FromJson<ImageDataList>(jsonContent);
           
          
        }
      
        GridLayoutGroup gridLayoutGroup = _cardContainer.GetComponent<GridLayoutGroup>();
       
        gridLayoutGroup.constraint= GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = 3;
        imageDataList.images = ShuffleList(imageDataList.images);

        int totalCards = _gridColumn * _gridRow;

        for (int i = 0; i < totalCards/2; i++)
        {
            _imageIndexList.Add(i);
            _imageIndexList.Add(i);
        }
        ShuffleList(_imageIndexList);


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
            if(_currentRevealedCardId == id)
            {
                Debug.Log("Matched");
                // _cards[_currentRevealedCardIndex].GetComponent<Card>().HideCard();
             //   _cards[cardId].GetComponent<Card>().HideCard();
                StartCoroutine(DestroyCardsAfterDelay(cardId, 0.5f));

            }
            else
            {
                 StartCoroutine(HideCardsAfterDelay(cardId, 0.5f));
              //  _cards[_currentRevealedCardIndex].GetComponent<Card>().HideCard();
                //_cards[cardId].GetComponent<Card>().HideCard();
                Debug.Log("Not Matched");
            }
           
     
        }
        Debug.Log("Card Id: " + id);
    }
    private IEnumerator HideCardsAfterDelay(int cardId, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(cardId+"Hide Cards"+_currentRevealedCardIndex);
        _cards[_currentRevealedCardIndex].GetComponent<Card>().HideCard();
        _cards[cardId].GetComponent<Card>().HideCard();
        _currentRevealedCardId = -1;
        _currentRevealedCardIndex = -1;
        revealedCardCounter =0;
    }
     private IEnumerator DestroyCardsAfterDelay(int cardId, float delay)
    {
        yield return new WaitForSeconds(delay);

        _cards[_currentRevealedCardIndex].GetComponent<Card>().DestroyCard();
        _cards[cardId].GetComponent<Card>().DestroyCard();
        _currentRevealedCardId = -1;
        _currentRevealedCardIndex = -1;
        revealedCardCounter =0;
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
    
}
