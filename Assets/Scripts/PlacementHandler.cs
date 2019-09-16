using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class PlacementHandler : MonoBehaviour
{
    #region Private Variables
    [SerializeField, Tooltip("The controller that is used in the scene to cycle and place objects.")]
    private ControllerConnectionHandler _controllerConnectionHandler = null;

    [SerializeField, Tooltip("The model to be placed.")]
    private GameObject _buildingModel = null;

    [SerializeField, Tooltip("The text on the headpose canvas.")]
    private Text _instructions = null;

    [SerializeField]
    private MLInputModule _inputModule = null;

    private GameObject _placementObject = null;
    private Vector3 _placementPos = Vector3.zero;
    private LineRenderer _beam = null;

    // vars for location, scale, and rotation of the model
    private float _beamLen = 0.88f;
    private float _modelScale = 1.0f;
    private float _modelRotation = 0.0f;

    // fixed consts that change the rate the vars above ^ change
    private const float _changeRate = 0.01f;
    private const float _changeRateRotation = 1.4f;

    private bool _placing = true;

    private HeadposeCanvasPE _hpCanvas;

    private enum touchMode
    {
        Beam,
        Rotation,
        Scale
    }

    private string[] _modeText = {"Changing beam length \nSwipe forward to increase length or back to decrease length",
        "Changing model rotation(y-axis) \nSwipe right or left to rotate the model that direction",
        "Changing model scale \nSwipe forward to increase size or back to decrease size" };

    private string _moveText = "\n\nPress the home button to reposition the text.\nPress the bumper to change something else." +
        "\nPress the trigger to place the model.";

    private touchMode _mode;
    #endregion

    #region Unity Methods
    void Start()
    {
        if (_controllerConnectionHandler == null)
        {
            Debug.LogError("Error: No controller connected.");
            enabled = false;
            return;
        }

        MLInput.OnControllerButtonDown += HandleOnButtonDown;
        MLInput.OnControllerButtonUp += HandleOnButtonUp;
        MLInput.OnTriggerDown += HandleOnTriggerDown;

        _placementObject = Instantiate(_buildingModel);
        _beam = GetComponent<LineRenderer>();

        _mode = touchMode.Beam;
        _instructions.text = _modeText[0] + _moveText;

        GameObject canvas = GameObject.Find("HeadposeCanvas");
        _hpCanvas = canvas.GetComponent<HeadposeCanvasPE>();
    }

    void Update()
    {
        _beam.SetPosition(0, _controllerConnectionHandler.ConnectedController.Position);
        _beam.SetPosition(1, _controllerConnectionHandler.ConnectedController.Position + (transform.forward * _beamLen));

        if(_placing == false)
        {
            if (_inputModule.PointerLineSegment.End.HasValue)
            {
                _beam.SetPosition(1, _inputModule.PointerLineSegment.End.Value);
            }
            return;
        }

        _placementPos = _beam.GetPosition(1);

        if (_placementObject != null)
        {
            _placementObject.transform.position = _placementPos;
        }

        if(_controllerConnectionHandler.ConnectedController.Touch1Active)
        {
            HandleTouch();
        }
    }

    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= HandleOnButtonDown;
        MLInput.OnControllerButtonUp -= HandleOnButtonUp;
        MLInput.OnTriggerDown -= HandleOnTriggerDown;
    }
    #endregion

    #region Event Handlers
    private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (_controllerConnectionHandler.IsControllerValid() && _controllerConnectionHandler.ConnectedController.Id == controllerId &&
            button == MLInputControllerButton.Bumper && _placing)
        {
            // iterates to the next touch mode
            int pos = ((int)_mode + 1) % 3;
            _mode = (touchMode)pos;

            _instructions.text = _modeText[pos] + _moveText;
        }
    }

    private void HandleOnButtonUp(byte controllerId, MLInputControllerButton button)
    {
        if (_controllerConnectionHandler.IsControllerValid() && _controllerConnectionHandler.ConnectedController.Id == controllerId &&
            button == MLInputControllerButton.HomeTap)
        {
            _hpCanvas.MoveCanvas();
        }
    }

    private void HandleOnTriggerDown(byte controllerId, float pressure)
    {
        if (_placing)
        {
            CompletePlacement();
        }
    }
    #endregion

    #region Private Methods
    private void HandleTouch()
    {
        MLInputControllerTouchpadGestureType gesture = _controllerConnectionHandler.ConnectedController.TouchpadGesture.Type;
        if(gesture != MLInputControllerTouchpadGestureType.Swipe)
        {
            return;
        }

        MLInputControllerTouchpadGestureDirection direction = _controllerConnectionHandler.ConnectedController.TouchpadGesture.Direction;

        if(_mode == touchMode.Beam)
        {
            if(direction == MLInputControllerTouchpadGestureDirection.Up)
            {
                _beamLen += _changeRate;
            }
            else if(direction == MLInputControllerTouchpadGestureDirection.Down)
            {
                _beamLen -= _changeRate;
                if(_beamLen < 0)
                {
                    _beamLen = 0;
                }
            }
        }
        else if(_mode == touchMode.Rotation)
        {
            if(direction == MLInputControllerTouchpadGestureDirection.Right)
            {
                _modelRotation -= _changeRateRotation;
            }
            else if(direction == MLInputControllerTouchpadGestureDirection.Left)
            {
                _modelRotation += _changeRateRotation;
            }

            if(_modelRotation > 360)
            {
                _modelRotation -= 360;
            }
            else if(_modelRotation < -360)
            {
                _modelRotation += 360;
            }

            _placementObject.transform.rotation = Quaternion.Euler(0, _modelRotation, 0);
        }
        else if(_mode == touchMode.Scale)
        {
            if(direction == MLInputControllerTouchpadGestureDirection.Up)
            {
                _modelScale += _changeRate;
            }
            else if(direction == MLInputControllerTouchpadGestureDirection.Down)
            {
                _modelScale -= _changeRate;
                if(_modelScale < 0)
                {
                    _modelScale = 0;
                }
            }

            _placementObject.transform.localScale = new Vector3(_modelScale, _modelScale, _modelScale);
        }
    }

    private void CompletePlacement()
    {
        _instructions.text = "Click on a button to find out about the building.";
        _placing = false;
    }
    #endregion
}
