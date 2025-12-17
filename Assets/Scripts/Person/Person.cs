using System;
using UnityEngine;

public class Person : MonoBehaviour {
    #region Variables
    [Header("Person Info")]
    [SerializeField] private string _name = "";
    [SerializeField] private PersonPersonality _personality;
    
    // 1. AÑADIMOS EL ARBOL DE DIALOGO AQUÍ
    [Header("Dialogue")] 
    [SerializeField] private DialogueTree _dialogueTree; // <--- NUEVO: Arrastra aquí el ScriptableObject del diálogo
    public DialogueTree MyDialogue => _dialogueTree;     // <--- NUEVO: Getter para que el Estado pueda leerlo

    [Header("State")]
    private IPersonState _state = new PersonStateWaiting();
    [SerializeField] private Vector3 _targetPosition = Vector3.zero;

    [Header("Shop")]
    [SerializeField] private string _figureWants = ""; 

    [Header("Physics")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _reachThreshold = 0.05f;
    public float ReachThreshold => _reachThreshold;

    [Header("Callbacks")]
    private Action<Person> _onFinishedBuying;
    private Action<Person> _onFinishedLeaving;
    #endregion

    #region Events
    void Start() {
        if (_name == "") Debug.LogWarning("Person without name!");
        // Asegurarnos de que el target position inicial es nuestra posición para que no se mueva solo al spawnear
        if (_targetPosition == Vector3.zero) _targetPosition = transform.position;
    }
    void Update() {
        if(_state != null)
            _state.Update(this);
    }
    #endregion

    #region Public Methods
    
    // --- LÓGICA DE DECISIÓN AL ACABAR EL DIÁLOGO ---
    public void EndConversation()
    {
        // Preguntamos a la personalidad (PersonPersonality.cs) si le caemos bien
        if (_personality.CheckHumourToBuy())
        {
            Debug.Log(_name + ": ¡Me ha gustado la charla! Esperaré a que me des la figura.");
            // NO llamamos a FinishBuying todavía.
            // El personaje se queda quieto en estado Buying esperando a que pongas la figura en el mantel.
        }
        else
        {
            Debug.Log(_name + ": Vaya trato... Me voy.");
            // Si le caemos mal, terminamos la compra inmediatamente (sin vender nada) y se va.
            FinishBuying(); 
        }
    }

    // Método para llamar DESDE EL MANTEL cuando le das la figura correcta
    public void ClientReceivedFigure()
    {
        Debug.Log(_name + ": ¡Gracias por la figura!");
        FinishBuying(); // Ahora sí, activamos la secuencia de irse
    }
    // ------------------------------------------------

    public void ChangeState(IPersonState newState) {
        _state = newState;
    }
    
    // Estos métodos ya no los necesitas llamar manualmente desde los botones, 
    // lo hace el DialogueController automáticamente con los puntos del ScriptableObject
    public void RecieveGoodResponse() {
        _personality.Add(1);
    }
    public void RecieveBadResponse() {
        _personality.Add(-1);
    }

    // Este método avisa al Manager de que la interacción ha terminado (sea bien o mal)
    public void FinishBuying() {
        _onFinishedBuying?.Invoke(this);
    }
    
    public void FinishLeaving() {
        _onFinishedLeaving?.Invoke(this);
    }
    
    public void DestroySelf() {
        CleanEvents();
        Destroy(gameObject);
    }
    
    public void Movement() {
        if (_targetPosition == null || CheckReachedTarget()) return;

        transform.position = Vector3.MoveTowards(
            transform.position, _targetPosition, _speed * Time.deltaTime
        );
    }
    
    public bool CheckReachedTarget() {
        return Vector3.Distance(transform.position, _targetPosition) <= _reachThreshold;
    }
    
    public void SetTargetPosition(Vector3 targetPos) {
        _targetPosition = targetPos;
    }
    
    #region Getters
    public PersonPersonality Personality => _personality;
    public string Name => _name;
    public string FigureWants => _figureWants;
    public IPersonState State => _state;
    public Action<Person> OnFinishedBuying {
        get => _onFinishedBuying;
        set => _onFinishedBuying = value;
    }
    public Action<Person> OnFinishedLeaving {
        get => _onFinishedLeaving;
        set => _onFinishedLeaving = value;
    }
    public Vector3 TargetPosition => _targetPosition;
    public float Speed => _speed;
    #endregion
    #endregion

    #region Private Methods
    private void CleanEvents() {
        if (_onFinishedBuying == null) return;
        foreach (Action<Person> d in _onFinishedBuying.GetInvocationList()) {
            _onFinishedBuying -= d;
        }
    }
    #endregion
}