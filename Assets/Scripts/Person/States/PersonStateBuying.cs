using UnityEngine;

public class PersonStateBuying : IPersonState {
  #region Variables
  private bool _isInteracting = false;
  #endregion

  #region Public Methods
  public void Update(Person person) {
    Debug.Log(person.Name + " está COMPRANDO\n");

    if (!person.CheckReachedTarget()) {
      person.Movement();
      return;
    }

    // TODO: Activar posible interacción para que el player pueda dialogar con el y activar el sistema de diálogo
    // if (!_isInteracting) {
    //   StartDialogue(person);
    // }

    person.FinishBuying();
  }
  #endregion

  #region Private Methods
  private void StartDialogue(Person person) {
    _isInteracting = true;

    // DialogueManager.Instance.StartDialogue(person);
  }
  #endregion
}
