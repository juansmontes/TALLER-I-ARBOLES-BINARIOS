namespace DocuTrackBST.Model;

/// <summary>Representa una carpeta o un archivo dentro del árbol BST.</summary>
public sealed class Nodo
{
    public string Nombre { get; set; }
    public bool EsCarpeta { get; set; }
    public Nodo? Izquierdo { get; set; }
    public Nodo? Derecho { get; set; }

    public Nodo(string nombre, bool esCarpeta)
    {
        Nombre = nombre;
        EsCarpeta = esCarpeta;
    }
}
