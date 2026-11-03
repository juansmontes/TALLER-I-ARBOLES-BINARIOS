using DocuTrackBST.Model;

namespace DocuTrackBST.View;

/// <summary>Única capa encargada de escribir en consola.</summary>
public sealed class ConsolaView
{
    public void Titulo(string texto)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 76));
        Console.WriteLine($" {texto.ToUpperInvariant()}");
        Console.WriteLine(new string('=', 76));
    }

    public void Mensaje(string texto) => Console.WriteLine(texto);

    public void MostrarOperacion(ArbolBinario.ResultadoOperacion resultado)
    {
        Console.WriteLine($"[{(resultado.Exito ? "OK" : "AVISO")}] {resultado.Mensaje}");
        if (resultado.Comparaciones > 0) Console.WriteLine($"     Comparaciones: {resultado.Comparaciones}.");
        if (!string.IsNullOrWhiteSpace(resultado.Caso)) Console.WriteLine($"     Caso ejecutado: {resultado.Caso}.");
    }

    public void MostrarBusqueda(string nombre, ArbolBinario.ResultadoBusqueda resultado)
    {
        string estado = resultado.Encontrado ? $"HALLADO ({(resultado.Nodo!.EsCarpeta ? "carpeta" : "archivo")})" : "NO HALLADO";
        Console.WriteLine($"'{nombre}': {estado}. Comparaciones: {resultado.Comparaciones}.");
    }

    public void MostrarArbol(Nodo? raiz)
    {
        Console.WriteLine("\nÁrbol (📁 carpeta | 📄 archivo):");
        if (raiz is null) { Console.WriteLine("(árbol vacío)"); return; }
        Console.WriteLine($"{Icono(raiz)} {raiz.Nombre} [RAÍZ]");
        var hijos = new List<Nodo>();
        if (raiz.Izquierdo is not null) hijos.Add(raiz.Izquierdo);
        if (raiz.Derecho is not null) hijos.Add(raiz.Derecho);
        for (int i = 0; i < hijos.Count; i++)
            Dibujar(hijos[i], "", i == hijos.Count - 1, i == 0 && hijos.Count == 2 ? "IZQ" : (hijos[i] == raiz.Izquierdo ? "IZQ" : "DER"));
    }

    private static void Dibujar(Nodo nodo, string prefijo, bool esUltimo, string lado)
    {
        Console.WriteLine($"{prefijo}{(esUltimo ? "└── " : "├── ")}{Icono(nodo)} {nodo.Nombre} [{lado}]");
        var hijos = new List<Nodo>();
        if (nodo.Izquierdo is not null) hijos.Add(nodo.Izquierdo);
        if (nodo.Derecho is not null) hijos.Add(nodo.Derecho);
        for (int i = 0; i < hijos.Count; i++)
        {
            bool ultimo = i == hijos.Count - 1;
            string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   ");
            string nuevoLado = hijos[i] == nodo.Izquierdo ? "IZQ" : "DER";
            Dibujar(hijos[i], nuevoPrefijo, ultimo, nuevoLado);
        }
    }

    private static string Icono(Nodo nodo) => nodo.EsCarpeta ? "[DIR]" : "[FILE]";

    public void MostrarRecorrido(string nombre, IReadOnlyList<string> elementos) =>
        Console.WriteLine($"{nombre}: {(elementos.Count == 0 ? "(vacío)" : string.Join(" -> ", elementos))}");
}
