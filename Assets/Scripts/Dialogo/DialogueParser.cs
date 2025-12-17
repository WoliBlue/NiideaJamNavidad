using UnityEngine;

public static class DialogueParser
{
    /// <summary>
    /// Sustituye etiquetas especiales en el texto del diálogo.
    /// </summary>
    /// <param name="originalText">El texto que viene del ScriptableObject</param>
    /// <param name="person">La referencia al NPC actual</param>
    /// <returns>El texto procesado listo para el UI</returns>
    public static string ParseText(string originalText, Person person)
    {
        if (string.IsNullOrEmpty(originalText)) return "";

        string processedText = originalText;

        // Lógica de decisión dinámica
    if (processedText.Contains("_decision"))
    {
        // Si el humor es suficiente para comprar (balance > 0 o según tu lógica)
        if (person.Personality.HumourBalance >= 1) 
        {
            processedText = processedText.Replace("_decision", "¡Vale, decidido, me voy a llevar esta, _figura!");
        }
        else 
        {
            processedText = processedText.Replace("_decision", "Bueno, voy a seguir mirando por ahí a ver qué decido...");
        }
    }

        // 1. Sustituir el nombre de la figura
        // Busca "_figura" y lo cambia por person.FigureWants (ej: "el buey")
        if (processedText.Contains("_figura"))
        {
            processedText = processedText.Replace("_figura", person.FigureWants);
        }

        // 2. Sustituir el nombre del personaje (Opcional, por si lo necesitas)
        if (processedText.Contains("_nombre"))
        {
            processedText = processedText.Replace("_nombre", person.Name);
        }

        return processedText;
    }
}