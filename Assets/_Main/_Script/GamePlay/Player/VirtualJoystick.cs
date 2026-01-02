using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class VirtualJoystick : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    private InputAction _moveAction;
    private VisualElement _joystickPanel;
    private VisualElement _joystickBackground;
    private VisualElement _joystickHandle;
    //Vị trí Joystick Backgroud được sử dụng
    private Vector2 _touchStartPos;
    //Biến kiểm tra trạng thái kéo thả
    private bool _isDragging;
    //Bán kính tối đa của Joystick
    private float _maxRadius;
    private Vector2 _direction;
    private Vector2 _normalizedDirection;
    private Vector2 _currentTouchPos;
    private Joystick _joystick;
    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        _joystickPanel = root.Q<VisualElement>("joystickPanel");
        _joystickBackground = _joystickPanel.Q<VisualElement>("joystickBackground");
        _joystickHandle = _joystickPanel.Q<VisualElement>("joystickHandle");

        _joystickBackground.style.display = DisplayStyle.None;

        _joystickBackground.RegisterCallback<GeometryChangedEvent>(OnJoystickGeometryChanged);

        _moveAction = _inputActions.FindAction("Player/Move");
    }
    void Start()
    {
        _joystickPanel.RegisterCallback<PointerDownEvent>(OnPointerDown);
        _joystickPanel.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _joystickPanel.RegisterCallback<PointerUpEvent>(OnPointerUp);

    }
    private void OnJoystickGeometryChanged(GeometryChangedEvent evt)
    {
        // evt.newRect.width là kích thước chính xác sau layout
        _maxRadius = evt.newRect.width * 0.5f;

        // chỉ cần 1 lần => hủy callback
        _joystickBackground.UnregisterCallback<GeometryChangedEvent>(OnJoystickGeometryChanged);
    }
    private void OnPointerDown(PointerDownEvent evt)
    {
        _isDragging = true;
        _touchStartPos = evt.localPosition;
        //Đặt vị trí Joystick về vị trí chạm
        _joystickBackground.style.translate = _touchStartPos;
        _joystickHandle.style.translate = _touchStartPos;

        _joystickBackground.style.display = DisplayStyle.Flex;
    }
    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!_isDragging) return;
        // Debug.Log("Pointer Move at: " + evt.localPosition);
        _currentTouchPos = evt.localPosition;
        _direction = _currentTouchPos - _touchStartPos;

        // Hiển thị handle trong phạm vi
        if (_direction.magnitude > _maxRadius)
        {
            _direction = _direction.normalized * _maxRadius;
        }

        _joystickHandle.style.translate = _direction;

        _normalizedDirection.y = -_direction.y;
        _normalizedDirection.x = _direction.x;
        SendValueToInputSystem(_normalizedDirection);
    }
    private void OnPointerUp(PointerUpEvent evt)
    {

        if (!_isDragging) return;
        _joystickBackground.style.display = DisplayStyle.None;
        _isDragging = false;
        _touchStartPos = Vector2.zero;
        SendValueToInputSystem(Vector2.zero);

    }
    private void SendValueToInputSystem(Vector2 value)
    {
        // Debug.Log("_joystickBackground.style.display: " + _joystickBackground.style.display);
        _joystick = Joystick.current;
        // Debug.Log("_joystick: " + (_joystick == null ? "null" : "found"));
        if (_joystick == null)
        {
            InputSystem.RegisterLayout<Joystick>();
            _joystick = InputSystem.AddDevice<Joystick>();
            InputSystem.QueueDeltaStateEvent(_joystick.stick, value);
            // Debug.Log("Joystick Released. _joystick == null");
        }
        // InputSystem.Update(); 
        if (_joystick != null)
        {  
            InputSystem.QueueDeltaStateEvent(_joystick.stick, value);
            // Debug.Log("Joystick Released. _joystick != null");
        }
    }
}