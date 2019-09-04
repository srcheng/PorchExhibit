using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    // purpose of this class is to keep track of which button is active
    // and to tell a button to deactivate when a new button is turned on
    List<Button> _buttonList;
    Text _instructions;
    private int activeButton;

    // Start is called before the first frame update
    void Start()
    {
        _buttonList = new List<Button>();
        activeButton = -1;

        _instructions = GameObject.Find("HeadposeCanvas").GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int addToList(Button newButton)
    {
        _buttonList.Add(newButton);
        return _buttonList.Count - 1;
    }

    public void setActiveButton(int buttonNumber, string instructions)
    {
        // deactivate the active button
        if (activeButton != -1)
        {
            _buttonList[activeButton].onClick.Invoke();
        }

        // new button activates itself on click. no need to invoke
        // just keep track of which button is active
        activeButton = buttonNumber;
        _instructions.text = instructions;
    }

    public void endActive(int buttonNumber)
    {
        // button is turned off by a click event, or an mp3 file self terminates
        if(activeButton != buttonNumber)
        {
            return;
        }
        activeButton = -1;
        _instructions.text = "Click on a button to find out more about the building.";
    }
}
