using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButtonHandler : MonoBehaviour
{
    /*
     * 
     * Case 1:
     * From Main Menu (Pushed in Andorid BackButton Handler):
     *      Quit the game.
     * 
     * Case 2 (Pushed in SettingsMenu.cs):
     * From any kind of Dialogue State, Location, or from MirrorBeats state:
     *      Open the Account Panel Menu
     *      
     * Case 3:
     * From Account Panel Menu Open (Pushed in SettingsMenu.cs):
     *      Close the Account Panel Menu
     *      
     * Case 4:
     * From Settings Menu Open (menu with sliders) (Pushed in SettingsMenu.cs):
     *      Close Settigns menu (this closes the Account Panel Menu)
     * 
     * Case 5:
     * From Phone State Main Screen (Pushed in PhoneUIController.cs):
     *      Close the phone + go back to location state. [Exit state and return to last]
     * 
     * Case 6:
     * From any phone state subscreen (Same Push as case 5):
     *      Back to the main phone screen. [Exit state and return to last]
     * 
     */

    public static AndroidBackButtonHandler Instance;

    private Stack<Action> subscribedActions;

    private List<Action> subscribedActionsList;

    private void Awake()
    {
        if (AndroidBackButtonHandler.Instance != null)
        {
            Destroy(this);
        }
        else
        {
            subscribedActionsList = new List<Action>();
            subscribedActionsList.Add(QuitApplication);
            subscribedActions = new Stack<Action>();
            subscribedActions.Push(QuitApplication);
            AndroidBackButtonHandler.Instance = this;
        }
    }

    public void PushAction(Action action)
    {
        if (subscribedActionsList.Contains(action))
        {
            subscribedActionsList.Remove(action);
        }
        subscribedActionsList.Add(action);
        subscribedActions.Push(action);
    }

    public void PopAction(Action action)
    {
        subscribedActionsList.Remove(action);
    }

    private void ExecuteAction()
    {
        subscribedActionsList[subscribedActionsList.Count - 1].Invoke();
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExecuteAction();
        }
    }
}
