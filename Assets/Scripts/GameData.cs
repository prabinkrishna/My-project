using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData 
{
 
    
    public void SaveData(int[] _gameData , List<int> imageDataList,List<int> imageIndexList)
    {
        PlayerPrefs.SetString("totalMove", _gameData[0].ToString());
        PlayerPrefs.SetString("matchCounter", _gameData[1].ToString());
        PlayerPrefs.SetString("currentRevealedCardId", _gameData[2].ToString());
        PlayerPrefs.SetString("currentRevealedCardIndex", _gameData[3].ToString());   
        PlayerPrefs.SetString("revealedCardCounter", _gameData[4].ToString());
        PlayerPrefs.SetString("gridColumn", _gameData[5].ToString());
        PlayerPrefs.SetString("gridRow", _gameData[6].ToString());
        List<string> stringList = imageDataList.Select(x => x.ToString()).ToList();
        PlayerPrefs.SetString("imageDataList", String.Join(",", stringList));
        List<string> stringIndexList = imageIndexList.Select(x => x.ToString()).ToList();
        PlayerPrefs.SetString("imageIndexList", String.Join(",", stringIndexList));
        Debug.Log(  String.Join(",", imageDataList)+ "Saved Data");

        PlayerPrefs.Save();
    }
    public string[] LoadData()
    {
        string[] _gameData = new string[9];
        _gameData[0] = PlayerPrefs.GetString("totalMove");
        _gameData[1] = PlayerPrefs.GetString("matchCounter");
        _gameData[2] = PlayerPrefs.GetString("currentRevealedCardId");
        _gameData[3] = PlayerPrefs.GetString("currentRevealedCardIndex");
        _gameData[4] = PlayerPrefs.GetString("revealedCardCounter");
        _gameData[5] = PlayerPrefs.GetString("gridColumn");
        _gameData[6] = PlayerPrefs.GetString("gridRow");
        _gameData[7] = PlayerPrefs.GetString("imageDataList");
        _gameData[8] = PlayerPrefs.GetString("imageIndexList");


        return _gameData;
    }

}
