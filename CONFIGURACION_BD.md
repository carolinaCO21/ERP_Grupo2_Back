# Configuración de Base de Datos y Firebase

## 🔒 Configuración de Credenciales

Este proyecto utiliza **appsettings.json** para gestionar la configuración, incluyendo las cadenas de conexión a la base de datos y la configuración de Firebase.

### Para Desarrolladores

1. **Crea tu propio archivo `appsettings.Development.json`** en la carpeta `UI/`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "Firebase": {
    "ProjectId": "TU_PROJECT_ID_DE_FIREBASE",
    "CredentialPath": "firebase-credentials.json"
  },

  "ConnectionStrings": {
    "DefaultConnection": "server=TU_SERVIDOR;database=TU_BASE_DE_DATOS;uid=TU_USUARIO;pwd=TU_CONTRASEÑA;trustServerCertificate=true;"
  }
}
```

2. **Este archivo está en `.gitignore`** y NO se subirá al repositorio por seguridad.

3. **Reemplaza los valores** con tus credenciales reales:
   - `TU_PROJECT_ID_DE_FIREBASE`: Tu Project ID de Firebase (ej: "mi-proyecto-12345")
   - `TU_SERVIDOR`: Servidor de base de datos
   - `TU_BASE_DE_DATOS`: Nombre de la base de datos
   - `TU_USUARIO`: Usuario de la base de datos
   - `TU_CONTRASEÑA`: Contraseña del usuario

4. **Descarga tu archivo de credenciales de Firebase**:
   - Ve a Firebase Console → Project Settings → Service Accounts
   - Genera una nueva clave privada
   - Guarda el archivo como `firebase-credentials.json` en la carpeta `UI/`

### Para Producción

1. Las credenciales de producción deben configurarse en:
   - **Azure App Service**: Variables de entorno / Application Settings
   - **IIS**: En el archivo `appsettings.Production.json` (nunca subir a Git)
   - **Docker**: Variables de entorno en docker-compose.yml

### ⚠️ IMPORTANTE - Seguridad

- **NUNCA** subas credenciales reales al repositorio
- El archivo `appsettings.json` solo contiene valores de ejemplo
- Los archivos `appsettings.Development.json` y `appsettings.Production.json` están en `.gitignore`
- El archivo `firebase-credentials.json` está en `.gitignore`
- Si accidentalmente subes credenciales, **cámbialas inmediatamente**


### 🔥 Configuración de Firebase

El proyecto usa **Firebase Authentication** para autenticación JWT. Necesitas:

1. **Crear un proyecto en Firebase Console**
2. **Habilitar Authentication** con el método de inicio de sesión que uses
3. **Descargar las credenciales del Service Account**
4. **Colocar el archivo como `firebase-credentials.json`** en la carpeta `UI /`

## 📝 Estructura de la Base de Datos

Ver el script SQL en la raíz del proyecto para la estructura completa de las tablas.
