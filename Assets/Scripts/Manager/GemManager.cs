using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemManager : Singleton<GemManager>
{
    public TextMeshProUGUI text;

    [SerializeField]
    private int gemCount = 0;

    public int GemCount
    {
        get { return gemCount;  }
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = gemCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGem(int addedGemCount)
    {
        gemCount += addedGemCount;
        text.text = gemCount.ToString();
    }

    public bool UseGem(int usedGemCount)
    {
        if (gemCount - usedGemCount < 0) return false;
        gemCount -= usedGemCount;
        text.text = gemCount.ToString();
        return true;
    }
}