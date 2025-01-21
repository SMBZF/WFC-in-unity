using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SingleCameraController : MonoBehaviour
{
    // 是否启用自由模式
    private bool isFlyMode = false;

    // 自由移动参数
    public float acceleration = 50f; // 加速度
    public float accSprintMultiplier = 4f; // 冲刺倍数
    public float lookSensitivity = 2f; // 鼠标灵敏度
    public float damping = 5f; // 减速系数

    // 平滑切换参数
    public float transitionDuration = 0.5f; // 过渡持续时间（秒）

    private Vector3 velocity; // 当前速度
    private bool cursorLocked = true; // 鼠标是否锁定

    // UI 控制
    public GameObject uiElement; // 需要隐藏/显示的 UI

    // 用于存储初始状态
    private Vector3 initialPosition; // 初始位置
    private Quaternion initialRotation; // 初始旋转

    private bool isTransitioning = false; // 是否正在平滑过渡
    private float transitionStartTime; // 过渡开始时间
    private Vector3 startTransitionPosition; // 过渡起始位置
    private Quaternion startTransitionRotation; // 过渡起始旋转

    void Start()
    {
        // 记录初始位置和旋转
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        ToggleCursorLock(false); // 初始状态解锁鼠标
    }

    void Update()
    {
        // 按下 E 键切换模式
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isFlyMode)
            {
                // 切换到自由模式
                isFlyMode = true;
                ToggleCursorLock(true);

                // 隐藏 UI
                if (uiElement != null)
                    uiElement.SetActive(false);
            }
            else
            {
                // 退出自由模式，开始平滑切换
                isFlyMode = false;
                isTransitioning = true;
                transitionStartTime = Time.time;
                startTransitionPosition = transform.position;
                startTransitionRotation = transform.rotation;
                ToggleCursorLock(false);

                // 显示 UI
                if (uiElement != null)
                    uiElement.SetActive(true);
            }
        }

        if (isFlyMode && !isTransitioning)
        {
            HandleFlyMode();
        }
        else if (isTransitioning)
        {
            SmoothTransitionToInitial();
        }
    }

    /// <summary>
    /// 自由移动模式
    /// </summary>
    private void HandleFlyMode()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * 0.2f; // 降低水平旋转速度
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * 0.4f; // 降低垂直旋转速度

        // 获取当前摄像机位置
        Vector3 currentPosition = transform.position;

        // 水平旋转（绕摄像机自身的 Y 轴旋转）
        Quaternion horizRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
        transform.rotation = horizRotation * transform.rotation;

        // 垂直旋转（绕摄像机自身的 X 轴旋转）
        Quaternion vertRotation = Quaternion.AngleAxis(-mouseY, transform.right);
        transform.rotation = vertRotation * transform.rotation;

        // 保持摄像机位置的 y 值不变
        transform.position = new Vector3(transform.position.x, currentPosition.y, transform.position.z);

        // 获取加速度向量
        velocity += GetAccelerationVector() * Time.deltaTime;

        // 应用阻尼
        velocity = Vector3.Lerp(velocity, Vector3.zero, damping * Time.deltaTime);

        // 更新摄像机位置
        transform.position += velocity * Time.deltaTime;
    }

    /// <summary>
    /// 获取加速度向量
    /// </summary>
    private Vector3 GetAccelerationVector()
    {
        Vector3 moveInput = default;

        // 添加方向键控制
        void AddMovement(KeyCode key, Vector3 dir)
        {
            if (Input.GetKey(key))
                moveInput += dir;
        }

        AddMovement(KeyCode.W, Vector3.forward);
        AddMovement(KeyCode.S, Vector3.back);
        AddMovement(KeyCode.D, Vector3.right);
        AddMovement(KeyCode.A, Vector3.left);
        AddMovement(KeyCode.Space, Vector3.up);
        AddMovement(KeyCode.LeftControl, Vector3.down);

        Vector3 direction = transform.TransformVector(moveInput.normalized);

        // 判断是否按下冲刺键（左Shift）
        if (Input.GetKey(KeyCode.LeftShift))
            return direction * (acceleration * accSprintMultiplier);
        return direction * acceleration;
    }

    /// <summary>
    /// 平滑过渡到初始位置和旋转
    /// </summary>
    private void SmoothTransitionToInitial()
    {
        float elapsedTime = Time.time - transitionStartTime;
        float t = Mathf.Clamp01(elapsedTime / transitionDuration); // 计算归一化时间（0 到 1）

        // 使用 SmoothStep 实现平滑插值
        t = Mathf.SmoothStep(0, 1, t);

        transform.position = Vector3.Lerp(startTransitionPosition, initialPosition, t);
        transform.rotation = Quaternion.Lerp(startTransitionRotation, initialRotation, t);

        // 检查过渡完成
        if (t >= 1f)
        {
            isTransitioning = false; // 过渡完成
            velocity = Vector3.zero; // 清除移动速度
        }
    }

    /// <summary>
    /// 切换鼠标锁定状态
    /// </summary>
    private void ToggleCursorLock(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }
}
