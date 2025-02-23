using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private ToggleGroup _levelSelector;
    
    private CardMatcher _game;
    

    public void Init(CardMatcher game)
    {
        _game = game;
    }
    
    public void OnPlayClick()
    {
        Toggle toggle = _levelSelector.ActiveToggles().FirstOrDefault();
        Debug.Log(toggle.name+"><"+toggle.GetComponentInChildren<Text>().text);
       _game.StartGame(toggle.name);
    }
    public void OnLevelSelected()
    {

    }
    public void OnQuitClick()
    {
        Application.Quit();
    }

}
