using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextClick : MonoBehaviour
{
    [SerializeField] Text _text = null;
    Button _button;
    ButtonManager _bm;

    private int _selfNumber;
    // Start is called before the first frame update
    void Start()
    {
        if(_text == null)
        {
            Debug.Log("No text attached to button");
            enabled = false;
            return;
        }

        _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate { onClick(); });

        _bm = GameObject.Find("ButtonHandler").GetComponent<ButtonManager>();
        _selfNumber = _bm.addToList(_button);

        _text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        if (_text.enabled)
        {
            _text.enabled = false;
            _bm.endActive(_selfNumber);
        }
        else
        {
            _text.enabled = true;
            _bm.setActiveButton(_selfNumber, "Text is open. Click on any button to close the text.");
        }
    }
}
