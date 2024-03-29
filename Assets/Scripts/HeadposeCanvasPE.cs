using UnityEngine;
using UnityEngine.UI;

public class HeadposeCanvasPE : MonoBehaviour
{
    #region Public Variables
    [Tooltip("The distance from the camera that this object should be placed.")]
    public float CanvasDistance = 1.5f;

    [Tooltip("The speed at which this object changes its position.")]
    public float PositionLerpSpeed = 5f;

    [Tooltip("The speed at which this object changes its rotation.")]
    public float RotationLerpSpeed = 5f;
    #endregion

    #region Private Varibles
    // The canvas that is attached to this object.
    private Canvas _canvas;

    // The camera this object will be in front of.
    private Camera _camera;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initializes variables and verifies that necessary components exist.
    /// </summary>
    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _camera = _canvas.worldCamera;

        // Disable this component if
        // it failed to initialize properly.
        if (_canvas == null)
        {
            Debug.LogError("Error: HeadposeCanvas._canvas is not set, disabling script.");
            enabled = false;
            return;
        }
        if (_camera == null)
        {
            Debug.LogError("Error: HeadposeCanvas._camera is not set, disabling script.");
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// Update position and rotation of this canvas object to face the camera using lerp for smoothness.
    /// </summary>
    void Update()
    {
    }
    #endregion

    public void MoveCanvas()
    {
        // Move the object CanvasDistance units in front of the camera.
        Vector3 posTo = _camera.transform.position + (_camera.transform.forward * CanvasDistance);
        transform.position = posTo;

        // Rotate the object to face the camera.
        Quaternion rotTo = Quaternion.LookRotation(transform.position - _camera.transform.position);
        transform.rotation = rotTo;
    }
}