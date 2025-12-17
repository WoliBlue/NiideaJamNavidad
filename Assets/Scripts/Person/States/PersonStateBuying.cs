using UnityEngine;

public class PersonStateBuying : IPersonState {
    
    private bool _hasStartedDialogue = false;

    public void Update(Person person) {
        // 1. Moverse hacia el mostrador
        if (!person.CheckReachedTarget()) {
            person.Movement();
            return;
        }

        // 2. Una vez llega, iniciamos diálogo (solo una vez)
        if (!_hasStartedDialogue) {
            StartDialogue(person);
        }
        
        // Aquí no llamamos a FinishBuying. 
        // Esperamos a que person.EndConversation() decida qué hacer.
    }

    private void StartDialogue(Person person) {
        _hasStartedDialogue = true;
        
        Debug.Log("Iniciando conversación con " + person.Name);

        // Llamamos al DialogueBoxControllerMulti pasando el árbol de este personaje
        if (person.MyDialogue != null) {
            DialogueBoxControllerMulti.instance.StartDialogue(person.MyDialogue, 0, person);
        } else {
            Debug.LogError("¡Este personaje no tiene asignado un DialogueTree en el inspector!");
            // Failsafe: Si no tiene diálogo, se va para no bloquear el juego
            person.FinishBuying();
        }
    }
}