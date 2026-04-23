using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class VirtualJoystick : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    private InputAction moveAction;
    private VisualElement joystickPanel;
    private VisualElement joystickBackground;
    private VisualElement joystickHandle;
    //Vị trí Joystick Backgroud được sử dụng
    private Vector2 touchStartPos;
    //Biến kiểm tra trạng thái kéo thả
    private bool isDragging;
    //Bán kính tối đa của Joystick
    private float maxRadius;
    private Vector2 direction;
    private Vector2 normalizedDirection;
    private Vector2 currentTouchPos;
    private Joystick joystick;
    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        joystickPanel = root.Q<VisualElement>("joystickPanel");
        joystickBackground = joystickPanel.Q<VisualElement>("joystickBackground");
        joystickHandle = joystickPanel.Q<VisualElement>("joystickHandle");

        joystickBackground.style.display = DisplayStyle.None;

        joystickBackground.RegisterCallback<GeometryChangedEvent>(OnJoystickGeometryChanged);

        moveAction = _inputActions.FindAction("Player/Move");
    }
    void Start()
    {
        joystickPanel.RegisterCallback<PointerDownEvent>(OnPointerDown);
        joystickPanel.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        joystickPanel.RegisterCallback<PointerUpEvent>(OnPointerUp);

    }
    private void OnJoystickGeometryChanged(GeometryChangedEvent evt)
    {
        // evt.newRect.width là kích thước chính xác sau layout
        maxRadius = evt.newRect.width * 0.5f;

        // chỉ cần 1 lần => hủy callback
        joystickBackground.UnregisterCallback<GeometryChangedEvent>(OnJoystickGeometryChanged);
    }
    private void OnPointerDown(PointerDownEvent evt)
    {
        isDragging = true;
        touchStartPos = evt.localPosition;
        //Đặt vị trí Joystick về vị trí chạm
        joystickBackground.style.translate = touchStartPos;
        joystickHandle.style.translate = touchStartPos;

        joystickBackground.style.display = DisplayStyle.Flex;
    }
    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!isDragging) return;
        // Debug.Log("Pointer Move at: " + evt.localPosition);
        currentTouchPos = evt.localPosition;
        direction = currentTouchPos - touchStartPos;

        // Hiển thị handle trong phạm vi
        if (direction.magnitude > maxRadius)
        {
            direction = direction.normalized * maxRadius;
        }

        joystickHandle.style.translate = direction;

        normalizedDirection.y = -direction.y;
        normalizedDirection.x = direction.x;
        SendValueToInputSystem(normalizedDirection);
    }
    private void OnPointerUp(PointerUpEvent evt)
    {

        if (!isDragging) return;
        joystickBackground.style.display = DisplayStyle.None;
        isDragging = false;
        touchStartPos = Vector2.zero;
        SendValueToInputSystem(Vector2.zero);

    }
    private void SendValueToInputSystem(Vector2 value)
    {
        // Debug.Log("joystickBackground.style.display: " + joystickBackground.style.display);
        joystick = Joystick.current;
        // Debug.Log("joystick: " + (joystick == null ? "null" : "found"));
        if (joystick == null)
        {
            InputSystem.RegisterLayout<Joystick>();
            joystick = InputSystem.AddDevice<Joystick>();
            InputSystem.QueueDeltaStateEvent(joystick.stick, value);
            // Debug.Log("Joystick Released. joystick == null");
        }
        // InputSystem.Update(); 
        if (joystick != null)
        {  
            InputSystem.QueueDeltaStateEvent(joystick.stick, value);
            // Debug.Log("Joystick Released. joystick != null");
        }
    }
}