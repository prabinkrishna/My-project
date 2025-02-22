using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button _revealButton;
    [SerializeField] private Image _resultImage;
    
    public void OnReveal()
    {
        _revealButton.gameObject.SetActive(false);
    }
}
