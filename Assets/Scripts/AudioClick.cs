using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioClick : MonoBehaviour
{
    AudioSource _mp3;
    Button _button;
    ButtonManager _bm;

    private bool _isActive = false;
    private int _selfNumber;
    // Start is called before the first frame update
    void Start()
    {
        _mp3 = GetComponentInChildren<AudioSource>();
        if(_mp3 == null)
        {
            Debug.Log("No audio source attached to button");
            enabled = false;
            return;
        }

        _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate { onClick(); });

        _bm = GameObject.Find("ButtonHandler").GetComponent<ButtonManager>();
        _selfNumber = _bm.addToList(_button);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
        {
            if(_mp3.isPlaying == false)
            {
                _isActive = false;
                _bm.endActive(_selfNumber);
            }
        }
    }

    public void onClick()
    {
        if (_mp3.isPlaying)
        {
            _bm.endActive(_selfNumber);
            _mp3.Stop();
            _isActive = false;
        }
        else
        {
            _bm.setActiveButton(_selfNumber, "mp3 is currently playing. Click on any button to stop.");
            _mp3.Play();
            _isActive = true;
        }
    }
}
