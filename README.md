# Prueba Técnica - API Bancaria (Clean Architecture)

Este proyecto es una API construida en .NET 8 siguiendo principios de Clean Architecture. Permite gestionar clientes, cuentas bancarias y transacciones básicas.

# Requisitos
- .NET 8 SDK
- Visual Studio, VS Code o terminal
- Postman (opcional, para pruebas manuales)

# Ejecución del proyecto
## Desde la carpeta raíz del proyecto:

- cd src/Web
- dotnet run

## La API se expondrá por defecto en:
- http://localhost:5116

## Podés acceder a la documentación Swagger en:
- http://localhost:5116/swagger

# Ejecutar pruebas unitarias
## Desde la raíz del proyecto (prueba_lafise):
### Asegurarse de tener comentareadas las líneas que dicen 'USAR EN PRODUCCIÓN' para las pruebas unitarias y sin comentarios las que indican '// USAR EN TESTING (para que funcione con mocks)'
- dotnet test test/Application.Tests/Application.Tests.csproj -v n

Esto ejecutará todas las pruebas del proyecto Application.Tests.

# Endpoints disponibles
| Entidad       | Acción                        | Método | Ruta                                                  |
|---------------|-------------------------------|--------|-------------------------------------------------------|
| Customer      | Crear                         | POST   | /customers                                            |
|               | Actualizar                    | PATCH  | /customers/{id}                                       |
|               | Eliminar                      | DELETE | /customers/{id}                                       |
| BankAccount   | Crear                         | POST   | /bankaccounts                                         |
|               | Eliminar                      | DELETE | /bankaccounts/{id}                                    |
|               | Obtener balance               | GET    | /bankaccounts/balance?accountNumber=AC123456          |
| Transaction   | Crear depósito o retiro       | POST   | /transactions                                         |
|               | Historial y saldo final       | GET    | /transactions/summary/{bankAccountId}                 |

# Estado del proyecto
- Clean Architecture aplicada
- Validaciones con FluentValidation
- Eventos de dominio
- Pruebas unitarias de comandos y queries
- Base de datos SQLite

# Autor
José Palacios
Prueba técnica .NET - Abril 2025




