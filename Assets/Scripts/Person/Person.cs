using System;

using UnityEngine;

// Clase para definir a una persona que viene a comprar figuras al puesto.
// La personalidad se pilla de PersonalidadPersona.cs   
// Aqui se puede definir cosas como su nombre, que figura quiere, etc.
// Cuando la persona llega al puesto, cuando clickeas, se activa esta clase para que empiece la interacción.

public class Person : MonoBehaviour {
  #region Variables
  [Header("Person Info")]
  [SerializeField] private string _name = "";
  [SerializeField] private PersonPersonality _personality;
  [SerializeField] private Figuras _figureWants;
  [SerializeField] private Action<Person> _onFinishedBuying;
  private IPersonState _state;
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
  public void FinishBuying() {
    _onFinishedBuying?.Invoke(this);
  }
  public void DestroySelf() {
    CleanEvents();
    Destroy(gameObject);
  }
  #region Getters
  public PersonPersonality Personality => _personality;
  public string Name => _name;
  public Figuras FigureWants => _figureWants;
  public IPersonState State => _state;
  public Action<Person> OnFinishedBuying => _onFinishedBuying;
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

