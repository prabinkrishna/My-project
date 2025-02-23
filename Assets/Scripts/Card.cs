using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _revealButton;
    [SerializeField] private Image _resultImage;
    [SerializeField] private int _imageId;
    [SerializeField] private AudioSource _audio;
   // [SerializeField] private Animator _animator;
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

   
    public void FlipSound()
    {
        _audio.Play();
    }
   
    public void OnReveal()
    {
        _revealButton.gameObject.SetActive(false);
        FlipSound();
        _game.OnCardReveal(_cardId);
        
    }
    public void ToggleInteraction(bool status )
    {
        _revealButton.interactable = status;
    }
    public void StopInteraction()
    {
        _revealButton.interactable = false;
    }

    public void ToggleHideCard(bool status)
    {
       
        _revealButton.gameObject.SetActive(status);
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
