using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        _revealButton.gameObject.SetActive(false);
        _game.OnCardReveal(_imageId);
    }
    public void OnHide()
    {
        _revealButton.gameObject.SetActive(true);
    }
    public void SetResultImage(Sprite sprite)
    {
        _resultImage.sprite = sprite;
    }
}
