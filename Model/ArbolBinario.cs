namespace DocuTrackBST.Model;

public sealed class ArbolBinario
{
    private static readonly StringComparer Comparador = StringComparer.OrdinalIgnoreCase;
    public Nodo? Raiz { get; private set; }

    public sealed record ResultadoBusqueda(bool Encontrado, Nodo? Nodo, int Comparaciones);
    public sealed record ResultadoOperacion(bool Exito, string Mensaje, string? Caso = null, int Comparaciones = 0);

    public ResultadoOperacion Insertar(string? nombre, bool esCarpeta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return new(false, "El nombre no puede estar vacío.");

        nombre = nombre.Trim();
        if (Raiz is null)
        {
            Raiz = new Nodo(nombre, esCarpeta);
            return new(true, $"'{nombre}' insertado como raíz.");
        }

        Nodo actual = Raiz;
        int comparaciones = 0;
        while (true)
        {
            comparaciones++;
            int comparacion = Comparador.Compare(nombre, actual.Nombre);
            if (comparacion == 0)
                return new(false, $"Inserción rechazada: '{nombre}' ya existe (no se permiten duplicados).", Comparaciones: comparaciones);

            // Un archivo siempre debe ser hoja: nunca puede recibir descendientes.
            if (!actual.EsCarpeta)
                return new(false, $"Inserción rechazada: '{actual.Nombre}' es un archivo y no puede contener elementos.", Comparaciones: comparaciones);

            if (comparacion < 0)
            {
                if (actual.Izquierdo is null)
                {
                    actual.Izquierdo = new Nodo(nombre, esCarpeta);
                    return new(true, $"'{nombre}' insertado a la izquierda de '{actual.Nombre}'.", Comparaciones: comparaciones);
                }
                actual = actual.Izquierdo;
            }
            else
            {
                if (actual.Derecho is null)
                {
                    actual.Derecho = new Nodo(nombre, esCarpeta);
                    return new(true, $"'{nombre}' insertado a la derecha de '{actual.Nombre}'.", Comparaciones: comparaciones);
                }
                actual = actual.Derecho;
            }
        }
    }

    public ResultadoBusqueda Buscar(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return new(false, null, 0);
        Nodo? actual = Raiz;
        int comparaciones = 0;
        while (actual is not null)
        {
            comparaciones++;
            int c = Comparador.Compare(nombre.Trim(), actual.Nombre);
            if (c == 0) return new(true, actual, comparaciones);
            actual = c < 0 ? actual.Izquierdo : actual.Derecho;
        }
        return new(false, null, comparaciones);
    }

    /// <summary>Actualiza mediante la estrategia obligatoria: eliminar + insertar.</summary>
    public ResultadoOperacion Actualizar(string? nombreAntiguo, string? nombreNuevo)
    {
        if (string.IsNullOrWhiteSpace(nombreAntiguo) || string.IsNullOrWhiteSpace(nombreNuevo))
            return new(false, "Los nombres antiguo y nuevo son obligatorios.");

        nombreAntiguo = nombreAntiguo.Trim();
        nombreNuevo = nombreNuevo.Trim();
        var existente = Buscar(nombreAntiguo);
        if (!existente.Encontrado || existente.Nodo is null)
            return new(false, $"No se puede actualizar: '{nombreAntiguo}' no existe.");
        if (Comparador.Equals(nombreAntiguo, nombreNuevo))
            return new(false, "El nombre nuevo debe ser diferente al actual.");
        if (Buscar(nombreNuevo).Encontrado)
            return new(false, $"No se puede actualizar: '{nombreNuevo}' ya existe.");

        bool esCarpeta = existente.Nodo.EsCarpeta;
        var eliminacion = Eliminar(nombreAntiguo);
        if (!eliminacion.Exito) return eliminacion;

        var insercion = Insertar(nombreNuevo, esCarpeta);
        if (!insercion.Exito)
        {
            // Protección ante una restricción estructural: se restaura el nodo original.
            Insertar(nombreAntiguo, esCarpeta);
            return new(false, $"Actualización cancelada: {insercion.Mensaje}");
        }
        return new(true, $"'{nombreAntiguo}' fue actualizado a '{nombreNuevo}' mediante eliminar + insertar.");
    }

    public ResultadoOperacion Eliminar(string? nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return new(false, "El nombre no puede estar vacío.");
        bool eliminado = false;
        string caso = "";
        Raiz = EliminarRecursivo(Raiz, nombre.Trim(), ref eliminado, ref caso);
        return eliminado
            ? new(true, $"'{nombre}' eliminado correctamente.", caso)
            : new(false, $"No se encontró '{nombre}'.");
    }

    private static Nodo? EliminarRecursivo(Nodo? nodo, string nombre, ref bool eliminado, ref string caso)
    {
        if (nodo is null) return null;
        int c = Comparador.Compare(nombre, nodo.Nombre);
        if (c < 0) nodo.Izquierdo = EliminarRecursivo(nodo.Izquierdo, nombre, ref eliminado, ref caso);
        else if (c > 0) nodo.Derecho = EliminarRecursivo(nodo.Derecho, nombre, ref eliminado, ref caso);
        else
        {
            eliminado = true;
            if (nodo.Izquierdo is null && nodo.Derecho is null)
            {
                caso = "hoja";
                return null;
            }
            if (nodo.Izquierdo is null || nodo.Derecho is null)
            {
                caso = "nodo con un hijo";
                return nodo.Izquierdo ?? nodo.Derecho;
            }

            caso = "nodo con dos hijos (reemplazo por sucesor inorden)";
            Nodo sucesor = Minimo(nodo.Derecho);
            nodo.Nombre = sucesor.Nombre;
            nodo.EsCarpeta = sucesor.EsCarpeta;
            nodo.Derecho = EliminarMinimo(nodo.Derecho);
        }
        return nodo;
    }

    private static Nodo Minimo(Nodo nodo)
    {
        while (nodo.Izquierdo is not null) nodo = nodo.Izquierdo;
        return nodo;
    }

    private static Nodo? EliminarMinimo(Nodo? nodo)
    {
        if (nodo is null) return null;
        if (nodo.Izquierdo is null) return nodo.Derecho;
        nodo.Izquierdo = EliminarMinimo(nodo.Izquierdo);
        return nodo;
    }

    public IReadOnlyList<string> Preorden() { var r = new List<string>(); Preorden(Raiz, r); return r; }
    private static void Preorden(Nodo? n, List<string> r) { if (n is null) return; r.Add(n.Nombre); Preorden(n.Izquierdo, r); Preorden(n.Derecho, r); }

    public IReadOnlyList<string> Inorden() { var r = new List<string>(); Inorden(Raiz, r); return r; }
    private static void Inorden(Nodo? n, List<string> r) { if (n is null) return; Inorden(n.Izquierdo, r); r.Add(n.Nombre); Inorden(n.Derecho, r); }

    public IReadOnlyList<string> Postorden() { var r = new List<string>(); Postorden(Raiz, r); return r; }
    private static void Postorden(Nodo? n, List<string> r) { if (n is null) return; Postorden(n.Izquierdo, r); Postorden(n.Derecho, r); r.Add(n.Nombre); }

    public IReadOnlyList<string> PorNiveles()
    {
        var resultado = new List<string>();
        if (Raiz is null) return resultado;
        var cola = new Queue<Nodo>(); cola.Enqueue(Raiz);
        while (cola.Count > 0)
        {
            var actual = cola.Dequeue(); resultado.Add(actual.Nombre);
            if (actual.Izquierdo is not null) cola.Enqueue(actual.Izquierdo);
            if (actual.Derecho is not null) cola.Enqueue(actual.Derecho);
        }
        return resultado;
    }

    /// <summary>Altura medida en niveles: árbol vacío = 0 y raíz sola = 1.</summary>
    public int Altura() => Altura(Raiz);
    private static int Altura(Nodo? n) => n is null ? 0 : 1 + Math.Max(Altura(n.Izquierdo), Altura(n.Derecho));
}
