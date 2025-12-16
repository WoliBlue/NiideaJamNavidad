using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float MinYaw = -360;
    public float MaxYaw = 360;
    public float MinPitch = -60;
    public float MaxPitch = 60;
    public float LookSensitivity = 2;

    [Header("Movement Settings")]
    public float MoveSpeed = 6f; // Velocidad normal reducida para realismo
    public float SprintSpeed = 10f;
    [Range(0f, 1f)] public float BackwardSpeedMultiplier = 0.6f; // Ir hacia atrás es 40% más lento
    
    [Header("Physics Settings")]
    public float Gravity = -15.0f; // Gravedad un poco más fuerte para que no flote al caer
    public float AccelerationTime = 0.1f; // Cuanto tarda en pillar velocidad (da sensación de peso)
    
    // Referencias internas
    protected CharacterController movementController;
    protected Camera playerCamera;
    protected float yaw;
    protected float pitch;

    // Variables de estado
    private float _verticalVelocity; // Velocidad solo en eje Y (Gravedad/Salto)
    private Vector2 _currentVelocityInput; // Para el suavizado (SmoothDamp)
    private Vector2 _smoothInputVelocity; // Referencia para la función SmoothDamp

    protected virtual void Start() {
        movementController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // Ocultar y bloquear cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Inicializar rotación actual
        yaw = transform.eulerAngles.y;
        pitch = playerCamera.transform.localEulerAngles.x;
    }

    protected virtual void Update() {
        // Rotación de cámara
        yaw += Input.GetAxisRaw("Mouse X") * LookSensitivity;
        pitch -= Input.GetAxisRaw("Mouse Y") * LookSensitivity;

        yaw = ClampAngle(yaw, MinYaw, MaxYaw);
        pitch = ClampAngle(pitch, MinPitch, MaxPitch);

        // Rotamos el cuerpo del personaje en Y (izquierda/derecha)
        transform.rotation = Quaternion.Euler(0.0f, yaw, 0.0f);
        // Rotamos solo la cámara en X (arriba/abajo)
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : MoveSpeed;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.y < 0)
        {
            targetSpeed *= BackwardSpeedMultiplier;
        }

        input.Normalize();
        _currentVelocityInput = Vector2.SmoothDamp(_currentVelocityInput, input * targetSpeed, ref _smoothInputVelocity, AccelerationTime);

        // Convertir el input 2D a dirección 3D relativa al personaje
        Vector3 velocity = transform.right * _currentVelocityInput.x + transform.forward * _currentVelocityInput.y;

        // Gravedad 
        if (movementController.isGrounded) {
            if (_verticalVelocity < 0.0f) {
                _verticalVelocity = -2f;
            }
        } else {
            // Aplicar gravedad
            _verticalVelocity += Gravity * Time.deltaTime;
        }
        velocity.y = _verticalVelocity;
		// Moverte
        movementController.Move(velocity * Time.deltaTime);
    }

    // Funciones de utilidad para los ángulos
    protected float ClampAngle(float angle, float min, float max) {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}