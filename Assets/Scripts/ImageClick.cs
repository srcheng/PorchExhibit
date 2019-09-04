using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageClick : MonoBehaviour
{
    [SerializeField, Tooltip("The images that will be displayed to the user.")]
    private RawImage[] _pics = null;

    Button _button;
    ButtonManager _bm;

    private int _selfNumber;
    private bool _isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _pics.Length; i++)
        {
            if(_pics[i] == null)
            {
                Debug.Log("Null image is attached");
                enabled = false;
                return;
            }
        }

        _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate { onClick(); });

        _bm = GameObject.Find("ButtonHandler").GetComponent<ButtonManager>();
        _selfNumber = _bm.addToList(_button);

        for (int i = 0; i < _pics.Length; i++)
        {
            _pics[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        if (_isActive)
        {
            for (int i = 0; i < _pics.Length; i++)
            {
                _pics[i].enabled = false;
            }
            _isActive = false;
            _bm.endActive(_selfNumber);
        }
        else
        {
            for (int i = 0; i < _pics.Length; i++)
            {
                _pics[i].enabled = true;
            }
            _isActive = true;
            _bm.setActiveButton(_selfNumber, "Images are displayed. Click on any button to close the images");
        }
    }
}
