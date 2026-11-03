using DocuTrackBST.Model;
using DocuTrackBST.View;

namespace DocuTrackBST.Controller;

/// <summary>Orquesta los casos de uso; no contiene la lógica estructural del BST.</summary>
public sealed class ArbolController
{
    private readonly ArbolBinario _arbol = new();
    private readonly ConsolaView _vista = new();

    public void Ejecutar()
    {
        _vista.Titulo("DocuTrack S.A. | Taller integrador - Árboles binarios (MVC)");
        ConstruirArbol();
        EjecutarBusquedas();
        EjecutarActualizaciones();
        EjecutarEliminaciones();
        MostrarVerificacionesFinales();
        _vista.Titulo("Fin de las pruebas");
        _vista.Mensaje("Ejecución terminada. Presione una tecla para salir...");
        Console.ReadKey(true);
    }

    private void ConstruirArbol()
    {
        _vista.Titulo("1. Construcción del árbol (18 elementos)");
        // La secuencia está diseñada para que las carpetas puedan tener hijos y los archivos sean hojas.
        (string Nombre, bool Carpeta)[] elementos =
        {
            ("Mantenimiento", true), ("Facturas", true), ("Usuarios", true),
            ("Contratos", true), ("Historias", true), ("Soporte", true), ("Zonas", true),
            ("Acta.pdf", false), ("Datos.csv", false), ("Guia.pdf", false),
            ("Imagenes", true), ("Procesos", true), ("Tareas", true),
            ("Varios", false), ("Zorro", false), ("Lecturas", true),
            ("Libros", true), ("Luz.pdf", false)
        };
        foreach (var elemento in elementos)
            _vista.MostrarOperacion(_arbol.Insertar(elemento.Nombre, elemento.Carpeta));

        // Intento duplicado para evidenciar la política solicitada.
        _vista.MostrarOperacion(_arbol.Insertar("facturas", true));
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void EjecutarBusquedas()
    {
        _vista.Titulo("2. Seis búsquedas rápidas con comparaciones");
        string[] nombres = { "Contratos", "Acta.pdf", "Soporte", "Zonas", "NoExiste.docx", "Temporal" };
        foreach (string nombre in nombres) _vista.MostrarBusqueda(nombre, _arbol.Buscar(nombre));
    }

    private void EjecutarActualizaciones()
    {
        _vista.Titulo("3. Actualizaciones selectivas (eliminar + insertar)");
        ActualizarYMostrar("Acta.pdf", "Acta2026.pdf", "Actualización de archivo hoja");
        ActualizarYMostrar("Lecturas", "Letras2026", "Actualización de carpeta que tenía un solo hijo");
        ActualizarYMostrar("Mantenimiento", "Repositorio", "Actualización de la raíz");
    }

    private void ActualizarYMostrar(string anterior, string nuevo, string encabezado)
    {
        _vista.Titulo(encabezado);
        _vista.MostrarOperacion(_arbol.Actualizar(anterior, nuevo));
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void EjecutarEliminaciones()
    {
        _vista.Titulo("4. Eliminaciones selectivas");
        EliminarYMostrar("Acta2026.pdf", "Eliminación de hoja");
        EliminarYMostrar("Imagenes", "Eliminación de nodo con un hijo");
        string? raizActual = _arbol.Raiz?.Nombre;
        if (raizActual is not null) EliminarYMostrar(raizActual, "Eliminación de raíz con dos subárboles");
    }

    private void EliminarYMostrar(string nombre, string encabezado)
    {
        _vista.Titulo(encabezado);
        _vista.MostrarOperacion(_arbol.Eliminar(nombre));
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void MostrarVerificacionesFinales()
    {
        _vista.Titulo("5. Recorridos de verificación y métricas");
        _vista.MostrarRecorrido("Preorden", _arbol.Preorden());
        _vista.MostrarRecorrido("Inorden", _arbol.Inorden());
        _vista.MostrarRecorrido("Postorden", _arbol.Postorden());
        _vista.MostrarRecorrido("Por niveles", _arbol.PorNiveles());
        _vista.Mensaje($"Altura final del árbol: {_arbol.Altura()} nivel(es).");
        _vista.Mensaje("El recorrido inorden confirma el orden BST: sus nombres deben aparecer de menor a mayor, sin duplicados.");
    }
}
