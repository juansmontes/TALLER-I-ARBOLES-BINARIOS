# DocuTrack S.A. — Árbol Binario de Búsqueda (BST) en C#

Taller integrador de Programación III — Universidad de Manizales.

## Descripción
Aplicación de consola que representa un repositorio de carpetas y archivos mediante un Árbol Binario de Búsqueda (BST). La clave de ordenamiento es el nombre, comparado sin distinguir mayúsculas/minúsculas mediante `StringComparer.OrdinalIgnoreCase`.

## Arquitectura MVC

- **Model** (`Nodo.cs`, `ArbolBinario.cs`): estructura y reglas del BST. No utiliza `Console`.
- **View** (`ConsolaView.cs`): presenta mensajes, métricas, recorridos y el diagrama ASCII.
- **Controller** (`ArbolController.cs`): coordina la secuencia de casos de uso y pruebas.
- **Program.cs**: punto de entrada que invoca exclusivamente al Controller.

## Funcionalidades

- Insertar, buscar, actualizar y eliminar.
- Rechazo de nombres duplicados (sin distinguir mayúsculas/minúsculas).
- Validación de nombres vacíos.
- Los archivos no pueden tener hijos; solo las carpetas pueden contener subnodos.
- Actualización implementada como eliminar + insertar, conservando el tipo de elemento.
- Eliminación de hoja, nodo con un hijo y nodo con dos hijos. Para dos hijos se utiliza el sucesor inorden (mínimo del subárbol derecho).
- Recorridos preorden, inorden, postorden y por niveles (Queue).
- Impresión ASCII con conectores `├──`, `└──` y `│`.
- Conteo de comparaciones en búsquedas y cálculo de altura.

## Pruebas incluidas

Al ejecutar el programa se insertan 18 elementos (carpetas y archivos), se intenta insertar un duplicado, se realizan seis búsquedas (cuatro existentes y dos inexistentes), tres actualizaciones (hoja, carpeta con un hijo y raíz), tres eliminaciones (hoja, nodo con un hijo y raíz con dos subárboles), y finalmente se muestran los cuatro recorridos y la altura.

## Requisitos

- .NET SDK 8.0 o superior.
- Visual Studio 2022, Visual Studio Code con la extensión C# o cualquier editor compatible con proyectos .NET.

## Ejecución

Desde una terminal, ubicarse en la carpeta del proyecto y ejecutar:

```bash
dotnet restore
dotnet run
```

Para compilar sin ejecutar:

```bash
dotnet build
```

## Publicación en GitHub

1. Crear un repositorio público llamado `DocuTrackBST`.
2. Abrir una terminal dentro de esta carpeta.
3. Ejecutar:

```bash
git init
git add .
git commit -m "Entrega taller integrador BST MVC en C#"
git branch -M main
git remote add origin https://github.com/TU-USUARIO/DocuTrackBST.git
git push -u origin main
```

Reemplazar `TU-USUARIO` por el usuario real de GitHub. Si el trabajo es colaborativo, agregar los nombres y responsabilidades de los integrantes y asegurarse de que cada uno realice al menos un commit.

## Recorrido que verifica el orden

El recorrido **inorden** (izquierda, raíz, derecha) debe mostrar las claves ordenadas alfabéticamente según el mismo comparador usado por el árbol. Si aparece una clave fuera de orden, se debe revisar la inserción, actualización o eliminación.

## Integrantes y responsabilidades

Completar antes de entregar:

- Integrante 1: nombre y responsabilidad.
- Integrante 2: nombre y responsabilidad (si aplica).
- Integrante 3: nombre y responsabilidad (si aplica).

## Nota sobre la altura

La altura se mide por niveles: árbol vacío = 0 y árbol con solo la raíz = 1.
