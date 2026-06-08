# DispenX Core API - Documentación Técnica

## 📌 Descripción General
API backend para el sistema **DispenX**, responsable de la telemetría de dispensadores de alimentos, gestión de horarios, cálculo dinámico de la próxima dispensación, alertas de stock y administración de usuarios con autenticación JWT.

## 🧱 Arquitectura
El proyecto sigue **Domain‑Driven Design (DDD)** con los siguientes contextos acotados:

| Contexto              | Responsabilidad |
|-----------------------|----------------|
| **IAM**               | Registro, login, JWT y gestión de contraseñas |
| **Inventario**        | Datos de sensores (peso, nivel, flujo) y cálculo de stock restante |
| **AlertasStock**      | Evaluación de umbrales y envío de alertas push |
| **Dispensadores**     | Dispensadores, estados, horarios, eventos y lógica de `nextDispenseAt` |
| **Dispositivos**      | Dispositivo IoT y versiones de firmware |
| **NotificacionesUsuario** | Notificaciones por usuario (bandeja de alertas) |
| **Usuarios** (heredado)| Perfil de usuario y vinculación con dispensador físico |

## ⚙️ Requisitos Previos
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- MySQL 8 (servidor local o remoto) con una base de datos vacía llamada `DispenX`
- Opcional: Rider, Visual Studio 2022+ o VS Code

## 🚀 Instalación y Ejecución

### 1. Clonar el repositorio
```bash
git clone <url-del-repo>
cd Backend-DispenXCore.Api
```

### 2. Configurar la cadena de conexión
Editar `appsettings.json` y ajustar:

```json
{
  "ConnectionStrings": {
    "DispenXDb": "Server=localhost;Database=DispenX;User=root;Password=TU_PASSWORD;"
  },
  "JwtSettings": {
    "SecretKey": "SuperSecretKey_AlMenos32Caracteres_Longitud12345678!",
    "Issuer": "DispenX",
    "Audience": "DispenXApp"
  }
}
```

### 3. Generar y aplicar migraciones
```bash
# Asegurarse de estar en la carpeta del proyecto (.csproj)
cd Backend-DispenXCore.Api
dotnet clean
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Ejecutar la API
```bash
dotnet run
```
La API se levanta en `https://localhost:5001` (según launchSettings.json). Swagger estará disponible en `https://localhost:5001/swagger`.

## 🔐 Autenticación JWT
La mayoría de endpoints requieren un token JWT. Flujo:

1. Registrar usuario (`POST /api/v1/auth/register`)
2. Iniciar sesión (`POST /api/v1/auth/login`) → obtienes un token
3. En las peticiones autenticadas, incluir el header: `Authorization: Bearer <token>`

## 📦 Endpoints

### 1. Auth (/api/v1/auth)
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| POST | `/auth/register` | No | Registrar un nuevo usuario |
| POST | `/auth/login` | No | Iniciar sesión y obtener token |
| POST | `/auth/logout` | Sí | Cerrar sesión (simbólico, JWT es stateless) |

Ejemplo de `POST /auth/register` Body (JSON):

```json
{
  "firstName": "Diego",
  "lastName": "Bastidas",
  "email": "diego@ejemplo.com",
  "password": "MiClave123!"
}
```
Respuesta: `200 OK` con `{ "message": "Usuario registrado correctamente" }`.

Ejemplo de `POST /auth/login` Body:

```json
{
  "email": "diego@ejemplo.com",
  "password": "MiClave123!"
}
```
Respuesta:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "Diego",
    "lastName": "Bastidas",
    "email": "diego@ejemplo.com",
    "role": "USER",
    "status": "ACTIVE",
    "photoUrl": null
  }
}
```

### 2. Users (/api/v1/users) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/users/{id}` | Obtener perfil de usuario por ID |
| PUT | `/users/{id}` | Actualizar nombre, apellido y foto |
| PATCH | `/users/{id}/password` | Cambiar contraseña (requiere actual y nueva) |

`PUT /users/{id}` Body:

```json
{
  "firstName": "Diego",
  "lastName": "Bastidas",
  "photoUrl": "https://foto.url"
}
```

`PATCH /users/{id}/password` Body:

```json
{
  "currentPassword": "MiClave123!",
  "newPassword": "NuevaClave456!"
}
```

### 3. Inventario – Telemetría (/api/v1/inventario) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/inventario/medicion` | Registrar datos de sensor (peso, nivel, flujo) |
| GET | `/inventario/estado` | Obtener estado de todos los contenedores |

`POST /inventario/medicion` Parámetros (query string o form): `contenedorId`, `peso`, `nivel`, `flujo`.
Ejemplo: `POST /api/v1/inventario/medicion?contenedorId=3fa85f64...&peso=2.5&nivel=30&flujo=0.1`

`GET /inventario/estado` Respuesta: lista de objetos con `id`, `grano`, `porcentajeRestante`, `pesoActual`, `nivelActual`.

### 4. Alertas de Stock (/api/v1/alertas-stock) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/alertas-stock/evaluar` | Evalúa umbrales y envía push si es necesario |
| GET | `/alertas-stock/{contenedorId}` | Lista todas las alertas de un contenedor |

`POST /alertas-stock/evaluar` Query: `contenedorId`, `umbral` (double), `deviceToken` (string para push).
Ejemplo: `POST .../evaluar?contenedorId=...&umbral=10&deviceToken=abc123`

### 5. Dispensadores (/api/v1/dispensators) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/dispensators` | Lista todos los dispensadores |
| GET | `/dispensators/{id}` | Estado dinámico (incluye nextDispenseAt) |

`GET /dispensators/{id}` Respuesta (ejemplo):

```json
{
  "id": 1,
  "dispensatorId": 1,
  "isActive": true,
  "currentCapacity": 3200,
  "maxCapacity": 5000,
  "dailyTotal": 350,
  "nextDispenseAt": "2026-06-09T07:30:00Z"
}
```
**Cálculo de nextDispenseAt**: El backend examina todos los horarios activos del dispensador, para cada uno calcula la próxima ocurrencia futura según `scheduledTime` y `frequencyDays`, y devuelve la fecha más cercana. Ver sección "Lógica de negocio".

### 6. Horarios (/api/v1/schedules) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/schedules?dispensatorId=1` | Horarios activos de un dispensador |
| GET | `/schedules/{id}` | Un horario por ID |
| POST | `/schedules` | Crear nuevo horario |
| PUT | `/schedules/{id}` | Actualizar horario |
| DELETE | `/schedules/{id}` | Eliminar horario |
| PATCH | `/schedules/{id}/toggle` | Activar/desactivar horario |

Body de `POST /schedules`:

```json
{
  "dispensatorId": 1,
  "name": "Desayuno",
  "supplyType": "RICE",
  "amount": 150,
  "scheduledTime": "07:30",
  "frequencyDays": [1, 2, 3, 4, 5],
  "smartRefill": false,
  "isActive": true
}
```
- `supplyType`: RICE, LENTILS, BEANS, CORN, OTHER
- `frequencyDays`: array de enteros donde 0=Domingo, 1=Lunes ... 6=Sábado

### 7. Eventos de dispensación (/api/v1/dispenser-events) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/dispenser-events?dispensatorId=1` | Listar eventos (filtros opcionales: from, to, supplyType) |
| POST | `/dispenser-events` | Registrar un evento de dispensación |

Query para `GET`: `?dispensatorId=1&from=2026-05-01&to=2026-05-31&supplyType=RICE`

Body de `POST`:

```json
{
  "dispensatorId": 1,
  "scheduleId": 1,
  "trigger": "app",
  "supplyType": "RICE",
  "amountDispensed": 150,
  "dispensedAt": "2026-06-08T07:30:00Z"
}
```
- `trigger`: app o manual

### 8. Dispositivo (/api/v1/device) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/device` | Obtener información del dispositivo |
| PATCH | `/device` | Actualizar nombre/ubicación |
| POST | `/device/ping` | Registrar latido (actualiza lastSeen) |

Body de `PATCH`:

```json
{
  "name": "Dispensador Cocina",
  "location": "Cocina Principal"
}
```

### 9. Firmware (/api/v1/firmware) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/firmware` | Lista todas las versiones de firmware |
| GET | `/firmware/latest` | Obtener la versión más reciente (isLatest: true) |
| POST | `/firmware/{id}/install` | Iniciar instalación de un firmware (simulado) |

### 10. Notificaciones de Usuario (/api/v1/notifications) – Autenticado
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/notifications?userId=...` | Obtener notificaciones de un usuario |
| PATCH | `/notifications/{id}/read` | Marcar una notificación como leída |
| PATCH | `/notifications/read-all?userId=...` | Marcar todas como leídas |

Estructura de una notificación:

```json
{
  "id": "guid",
  "userId": "guid",
  "type": "ALERT",
  "title": "Stock bajo",
  "time": "2 minutes ago",
  "message": "El nivel de alpiste está al 8%",
  "action": "/dispensator/1",
  "unread": true,
  "createdAt": "2026-06-08T12:00:00Z"
}
```

## 🧠 Lógica de Negocio Destacada

**Cálculo de nextDispenseAt**
Se consultan todos los schedules activos del dispensador.
Para cada schedule, se itera sobre sus `frequencyDays` y se calcula la próxima fecha/hora futura combinando el día de la semana con `scheduledTime`.
Si la hora actual ya pasó hoy, se toma el mismo día de la próxima semana.
Se devuelve la fecha más cercana de todas las calculadas.

**dailyTotal**
Es la suma de `amountDispensed` de todos los `DispenserEvents` del día actual (desde las 00:00 hasta las 23:59:59.999) para el dispensador solicitado.

**Capacidad actual**
`currentCapacity` se descuenta cada vez que se registra un `DispenserEvent`. Se inicializa con `maxCapacity` al crear el `DispensatorStatus`.

**Smart Refill**
Si un schedule tiene `smartRefill: true`, la cantidad despachada (`amount`) se puede ajustar automáticamente según la capacidad restante (implementación pendiente en backend; actualmente se registra el evento con la cantidad fija).

## 📂 Estructura del Proyecto (simplificada)
```text
Backend-DispenXCore.Api/
├── Shared/                   # Kernel DDD, extensiones
├── Infrastructure/           # DispenXDbContext
├── src/
│   ├── IAM/                  # Auth & JWT
│   ├── Inventario/           # Sensores y stock
│   ├── AlertasStock/         # Umbrales y push
│   ├── Dispensadores/        # Dispensators, Schedules, Events
│   ├── Dispositivos/         # Device & Firmware
│   ├── NotificacionesUsuario/ # Notificaciones bandeja
│   └── Usuarios/             # Perfil de usuario (heredado)
├── Program.cs
└── appsettings.json
```

## 🤝 Notas para Desarrolladores
- Respetar las capas DDD: lógica de negocio en Domain, orquestación en Application, persistencia en Infrastructure.
- No referenciar directamente entidades de otro contexto; usar servicios de aplicación o integraciones vía interfaces.
- Los Value Objects como `FrequencyDays` se almacenan como JSON en una sola columna.
- Las migraciones se aplican automáticamente al iniciar la app si se deja la línea `app.ApplyMigrations()` en Program.cs.
- Para añadir nuevos endpoints, crear el use case en Application, exponerlo en un controlador y registrar en `ServiceCollectionExtensions`.
