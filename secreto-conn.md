# Guardar credenciales MySQL de forma segura (local)

Este documento explica cómo almacenar de forma segura las credenciales de la base de datos MySQL para el proyecto y cómo cargarlas en una aplicación .NET o en entornos locales.

Claves recomendadas (JSON/.env):

- DB_HOST
- DB_PORT
- DB_NAME
- DB_USER
- DB_PASSWORD

Ejemplo de cadena de conexión (MySQL):

```
server=HOST;port=3306;user=USERNAME;password=PASSWORD;database=DB_NAME;Persist Security Info=True;
```

Opciones para almacenar secretos localmente

1) .NET User Secrets (desarrollo local)

- Ideal para desarrolladores que usan .NET y no quieren almacenar credenciales en el control de versiones.
- Requisitos: SDK .NET instalado.

Pasos rápidos:

```bash
# En la carpeta del proyecto (donde está el .csproj)
# Inicializa user-secrets (solo una vez por proyecto)
dotnet user-secrets init

# Añade secretos (ejemplo)
dotnet user-secrets set "DB_HOST" "127.0.0.1"
dotnet user-secrets set "DB_PORT" "3306"
dotnet user-secrets set "DB_NAME" "trabajoclase02"
dotnet user-secrets set "DB_USER" "trabajo"
dotnet user-secrets set "DB_PASSWORD" "trabajopass"
```

En tu `appsettings.Development.json` o en el arranque de la app puedes leer estas variables desde Configuration como de costumbre (el proveedor de user-secrets se añade automáticamente en los templates de ASP.NET Core cuando el entorno es Development).

2) Archivo `.env` (simple, multiplataforma)

- Crea un archivo `.env` en la raíz del proyecto (NO lo comitees).
- Contenido de ejemplo:

```
DB_HOST=127.0.0.1
DB_PORT=3306
DB_NAME=trabajoclase02
DB_USER=trabajo
DB_PASSWORD=trabajopass
```

- Para cargar automáticamente en .NET puedes usar librerías como `DotNetEnv` o `Microsoft.Extensions.Configuration.EnvironmentVariables` con algún loader de `.env`.

3) Archivo JSON local (secreto.json) — ejemplo

- Usa `secreto.json` (o `secreto.json.local`) para desarrollo local y añade `secreto.json` a `.gitignore`.
- Ejemplo (archivo `secreto.json.example` incluido en el repo como plantilla):

```json
{
  "DB_HOST": "127.0.0.1",
  "DB_PORT": "3306",
  "DB_NAME": "trabajoclase02",
  "DB_USER": "trabajo",
  "DB_PASSWORD": "trabajopass"
}
```

Uso de la cadena de conexión en .NET

En C# (ejemplo):

```csharp
var host = configuration["DB_HOST"];
var port = configuration["DB_PORT"] ?? "3306";
var user = configuration["DB_USER"];
var pass = configuration["DB_PASSWORD"];
var db = configuration["DB_NAME"];

var conn = $"server={host};port={port};user={user};password={pass};database={db};Persist Security Info=True;";
```

Buenas prácticas

- Nunca subas `secreto.json`, `.env` o claves al repositorio. Añade estas entradas a `.gitignore`.
- Usa `user-secrets` para desarrollo local y variables de entorno (o servicios de secreto) para staging/producción.
- Rotación de claves: cambia las contraseñas periódicamente y evita contraseñas triviales en producción.

---

Archivo de ejemplo: `secreto.json.example` en el repositorio. Crear `secreto.json` a partir de él y añadirlo a `.gitignore`.
