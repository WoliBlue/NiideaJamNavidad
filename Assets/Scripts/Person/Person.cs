using System;

using UnityEngine;

// Clase para definir a una persona que viene a comprar figuras al puesto.
// La personalidad se pilla de PersonalidadPersona.cs   
// Aqui se puede definir cosas como su nombre, que figura quiere, etc.
// Cuando la persona llega al puesto, cuando clickeas, se activa esta clase para que empiece la interacción.

// TODO: Añadir lógica de figura

public class Person : MonoBehaviour {
  #region Variables
  [Header("Person Info")]
  [SerializeField] private string _name = "";
  [SerializeField] private PersonPersonality _personality;

  [Header("State")]
  private IPersonState _state = new PersonStateWaiting();
  [SerializeField] private Vector3 _targetPosition = Vector3.zero;

  [Header("Shop")]
  // [SerializeField] private Figure _figureWants;
  [SerializeField] private string _figureWants = ""; //* Temporal para pruebas

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
  }
  void Update() {
    _state.Update(this);
  }
  #endregion

  #region Public Methods
  public void ChangeState(IPersonState newState) {
    _state = newState;
  }
  public void RecieveGoodResponse() {
    _personality.Add(1);
  }
  public void RecieveBadResponse() {
    _personality.Add(-1);
  }
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
    return Vector3.Distance(transform.position, _targetPosition)
            <= _reachThreshold;
  }
  public void SetTargetPosition(Vector3 targetPos) {
    _targetPosition = targetPos;
  }
  #region Getters
  public PersonPersonality Personality => _personality;
  public string Name => _name;
  // public Figure FigureWants => _figureWants;
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

