using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _revealButton;
    [SerializeField] private Image _resultImage;
    [SerializeField]   private int _imageId;
    private CardMatcher _game;
    private int _cardId;
    public int Id => _imageId;
    public void Init(CardMatcher game)
    {
        _game = game;
    }
    public void SetImageId(int id)
    {
        _imageId = id;
    }
    public void SetCardId(int id)
    {
        _cardId = id;
    }
    public void OnReveal()
    {
        Debug.Log("Revealed"+_game.revealedCardCounter);
        _game.revealedCardCounter++;
        if(_game.revealedCardCounter >2)
        {
            return;
        }
        Debug.Log("Revealed after "+_game.revealedCardCounter);
        

        _revealButton.gameObject.SetActive(false);
        _game.OnCardReveal(_imageId,_cardId);
    }
    public void ToggleInteraction(bool status )
    {
        _revealButton.interactable = status;
    }
    public void StopInteraction()
    {
        _revealButton.interactable = false;
    }

    public void HideCard()
    {
        _revealButton.gameObject.SetActive(true);
    }
    public void SetResultImage(Sprite sprite)
    {
        _resultImage.sprite = sprite;
    }
    public void DestroyCard()
    {
       _revealButton.gameObject.SetActive(false);
       _resultImage.gameObject.SetActive(false);

    }
}
