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

    public void OnReveal()
    {
        foreach (var card in _cards)
        {
            card.GetComponent<Card>().OnReveal();
        }
    }

    public void OnCardReveal(int id)
    {
        if(_currentRevealedCardId == -1)
        {
            _currentRevealedCardId = id;
        }
        else
        {
            if(_currentRevealedCardId == id)
            {
                Debug.Log("Matched");
            }
            else
            {
                Debug.Log("Not Matched");
            }
            _currentRevealedCardId = -1;
        }
        Debug.Log("Card Id: " + id);
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
