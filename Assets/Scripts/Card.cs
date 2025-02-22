using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button _revealButton;
    [SerializeField] private Image _resultImage;
    [SerializeField]   private int _id;
    public int Id => _id;
    public void SetId(int id)
    {
        _id = id;
    }
    public void OnReveal()
    {
        _revealButton.gameObject.SetActive(false);
    }
    public void SetResultImage(Sprite sprite)
    {
        _resultImage.sprite = sprite;
    }
}
