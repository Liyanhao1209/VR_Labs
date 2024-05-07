using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class UIID
{
    internal static string MainUI = "GameMainUI";
    internal static string DialogUI = "DialogUI";
    internal static string LevelInforUI = "LevelInforUI";
    internal static string LevelSucessUI = "LevelSucessUI";
    internal static string GameOverUI = "GameOverUI";
    internal static string DemandDisplayUI = "DemandDisplayUI";
}

// UI组件管理模块
public class UIManager : MonoBehaviour
{
    public T GetUIPanel<T>(string uiName)
    {
        return transform.Find(uiName).GetComponent<T>();
    }
    public void InitSet()
    {
        DialogUI.gameObject.SetActive(false);
        LevelInforUI.gameObject.SetActive(false);
        LevelSucessUI.gameObject.SetActive(false);
        GameOverUI.gameObject.SetActive(false);
        DemandDisplayUI.gameObject.SetActive(false);
    }

    private GameMainUI gameMainUI;
    internal GameMainUI GameMainUI
    {
        get
        {
            if (!gameMainUI)
                gameMainUI = GetUIPanel<GameMainUI>(UIID.MainUI);
            return gameMainUI;
        }
    }

    private DialogUI dialogUI;
    internal DialogUI DialogUI
    {
        get
        {
            if (!dialogUI)
                dialogUI = GetUIPanel<DialogUI>(UIID.DialogUI);
            return dialogUI;
        }
    }
    

    private LevelInforUI levelInforUI;
    internal LevelInforUI LevelInforUI
    {
        get
        {
            if (!levelInforUI)
                levelInforUI = GetUIPanel<LevelInforUI>(UIID.LevelInforUI);
            return levelInforUI;
        }
    }

    private LevelSucessUI levelSucessUI;
    internal LevelSucessUI LevelSucessUI
    {
        get
        {
            if (!levelSucessUI)
                levelSucessUI = GetUIPanel<LevelSucessUI>(UIID.LevelSucessUI);
            return levelSucessUI;
        }
    }

    private GameOverUI gameOverUI;
    internal GameOverUI GameOverUI
    {
        get
        {
            if (!gameOverUI)
                gameOverUI = GetUIPanel<GameOverUI>(UIID.GameOverUI);
            return gameOverUI;
        }
    }
    private DemandDisplayUI demandDisplayUI;
    internal DemandDisplayUI DemandDisplayUI
    {
        get
        {
            if (!demandDisplayUI)
                demandDisplayUI = GetUIPanel<DemandDisplayUI>(UIID.DemandDisplayUI);
            return demandDisplayUI;
        }
    }
}
