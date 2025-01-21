using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SingleCameraController : MonoBehaviour
{
    // �Ƿ���������ģʽ
    private bool isFlyMode = false;

    // �����ƶ�����
    public float acceleration = 50f; // ���ٶ�
    public float accSprintMultiplier = 4f; // ��̱���
    public float lookSensitivity = 2f; // ���������
    public float damping = 5f; // ����ϵ��

    // ƽ���л�����
    public float transitionDuration = 0.5f; // ���ɳ���ʱ�䣨�룩

    private Vector3 velocity; // ��ǰ�ٶ�
    private bool cursorLocked = true; // ����Ƿ�����

    // UI ����
    public GameObject uiElement; // ��Ҫ����/��ʾ�� UI

    // ���ڴ洢��ʼ״̬
    private Vector3 initialPosition; // ��ʼλ��
    private Quaternion initialRotation; // ��ʼ��ת

    private bool isTransitioning = false; // �Ƿ�����ƽ������
    private float transitionStartTime; // ���ɿ�ʼʱ��
    private Vector3 startTransitionPosition; // ������ʼλ��
    private Quaternion startTransitionRotation; // ������ʼ��ת

    void Start()
    {
        // ��¼��ʼλ�ú���ת
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        ToggleCursorLock(false); // ��ʼ״̬�������
    }

    void Update()
    {
        // ���� E ���л�ģʽ
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isFlyMode)
            {
                // �л�������ģʽ
                isFlyMode = true;
                ToggleCursorLock(true);

                // ���� UI
                if (uiElement != null)
                    uiElement.SetActive(false);
            }
            else
            {
                // �˳�����ģʽ����ʼƽ���л�
                isFlyMode = false;
                isTransitioning = true;
                transitionStartTime = Time.time;
                startTransitionPosition = transform.position;
                startTransitionRotation = transform.rotation;
                ToggleCursorLock(false);

                // ��ʾ UI
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
    /// �����ƶ�ģʽ
    /// </summary>
    private void HandleFlyMode()
    {
        // ��ȡ�������
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * 0.2f; // ����ˮƽ��ת�ٶ�
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * 0.4f; // ���ʹ�ֱ��ת�ٶ�

        // ��ȡ��ǰ�����λ��
        Vector3 currentPosition = transform.position;

        // ˮƽ��ת�������������� Y ����ת��
        Quaternion horizRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
        transform.rotation = horizRotation * transform.rotation;

        // ��ֱ��ת�������������� X ����ת��
        Quaternion vertRotation = Quaternion.AngleAxis(-mouseY, transform.right);
        transform.rotation = vertRotation * transform.rotation;

        // ���������λ�õ� y ֵ����
        transform.position = new Vector3(transform.position.x, currentPosition.y, transform.position.z);

        // ��ȡ���ٶ�����
        velocity += GetAccelerationVector() * Time.deltaTime;

        // Ӧ������
        velocity = Vector3.Lerp(velocity, Vector3.zero, damping * Time.deltaTime);

        // ���������λ��
        transform.position += velocity * Time.deltaTime;
    }

    /// <summary>
    /// ��ȡ���ٶ�����
    /// </summary>
    private Vector3 GetAccelerationVector()
    {
        Vector3 moveInput = default;

        // ��ӷ��������
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

        // �ж��Ƿ��³�̼�����Shift��
        if (Input.GetKey(KeyCode.LeftShift))
            return direction * (acceleration * accSprintMultiplier);
        return direction * acceleration;
    }

    /// <summary>
    /// ƽ�����ɵ���ʼλ�ú���ת
    /// </summary>
    private void SmoothTransitionToInitial()
    {
        float elapsedTime = Time.time - transitionStartTime;
        float t = Mathf.Clamp01(elapsedTime / transitionDuration); // �����һ��ʱ�䣨0 �� 1��

        // ʹ�� SmoothStep ʵ��ƽ����ֵ
        t = Mathf.SmoothStep(0, 1, t);

        transform.position = Vector3.Lerp(startTransitionPosition, initialPosition, t);
        transform.rotation = Quaternion.Lerp(startTransitionRotation, initialRotation, t);

        // ���������
        if (t >= 1f)
        {
            isTransitioning = false; // �������
            velocity = Vector3.zero; // ����ƶ��ٶ�
        }
    }

    /// <summary>
    /// �л��������״̬
    /// </summary>
    private void ToggleCursorLock(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }
}
