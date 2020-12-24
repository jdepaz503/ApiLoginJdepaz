# ApiLoginJdepaz
Johnny De Paz
ApiLoginJdepaz 👨‍💻
==========
**Índice 🚩**
----------
1. [Introducción](#intro)
2. [Endpoint expuestos](#end)
3. [Elementos técnicos utilizados](#elem)
4. [Buenas prácticas utilizadas](#pract)
5. [Instalación-Despliegue local](#inst)
6. [Prueba de Web Api con Postman (con detalles e imagenes)](#prueba)
    1. [Login](#login)
    2. [Usuarios](#user)
        1. [Agregar usuarios](#add)
        2. [Obtener usuarios](#obt)
        3. [Desactivar usuarios](#desac)
        4. [Modificando usuarios](#upd)
        5. [Password Reset](#pwd)
        6. [Cambiar Contraseña](#pwdupd)
    3. [Productos](#prod)
        1. [Obtener productos (filtrado y paginación)](#obtprod)

# 1. Introducción 🎯 <div id='intro' />
- Es un proyecto Web API REST, construido con *C# .net Core 3.1*. 

- Esta API expone los *endpoints* de **2 funcionalidades** las cuales son, *Login de usuario*, *CRUD de Usuarios* y *CRUD de Productos*. Alojados en una base de datos *MSSQL 2012*.

- Esta API se encuentra desplegada en [esta página web](http://jdepaz2012-001-site1.ftempurl.com/)

- No es recomendable exponer una API de esta manera en ambientes productivos y esta práctica debe de ser utilizada en ambientes de desarrollo, pero tomando en cuenta que es un proyecto no productivo. Con fines de comodidad para la visualización,  ejecución de endpoints o a falta de postman u otra herramienta,  se incluyó Swagger en el proyecto. [Se puede visualizar el proyecto en esta página.🔽](http://jdepaz2012-001-site1.ftempurl.com/swagger/index.html)

# 2. Endpoint expuestos ⭕ <div id='end' />
- **Login**
    - **Auth**: Es el encargado de validar si el usuario existe en la base de datos mediante el username y passuser.
    
    
- **Usuarios**
    - **Agregar**: Es el encargado de registrar nuevos usuarios para poder acceder a este endpoint es necesario hacerlo mediante un token el cual se obtienen al hacer un login valido. Se puede usar el usuario jdepaz con contraseña 12345 (siguiendo el orden de los migrations)
    - **Modificar**: Es el encargado de modificar información de usuario, únicamente de los campos nombre, telefono y fecha de nacimiento. 
    - **Desactivar**: Se utiliza para hacer un borrado lógico de un usuario, cambiando a 0 su estado mediante el username. 
    - **ObtenerUsuarios**: Es el encargado de listar todos los usuarios registrados, no requiere un JWT debido a que solo muestra la información de clientes exceptuando la contraseña. 
    - **PasswordReset**: Es el encargado de iniciar proceso de reestablecer contraseña, únicamente solicita correo electrónico, él valida que el correo electrónico exista en la base de datos, en caso de existir, manda un correo electrónico con el nombre de usuario, un párrafo indicando el proceso a seguir (dar clic a un enlace) y que sólo dispone de 15 minutos. En este caso para retomar el flujo es necesario especificar un enlace de un front encargado de recibir el token que el endpoint recibe y pedirle al usuario la nueva contraseña, para llamar al siguiente endoint a continuación. 
    - **cambiarContraseña**:  Es el encargado de recibir el token y nueva contraseña para poder cambiar la clave de usuario que viene inmerso en los claims del token. 


- **Productos**: Para poder tener acceso al consumo de los métodos de este Endpoint es necesario proporcionar el token obtenido de un login valido para poder hacer Authorize, de lo contrario no se podrán consumir 
    - **AgregarProducto**: Es el encargado de registrar los productos en la base de datos.
    - **ObtenerProductos**: Es el encargado de mostrar todos los productos reflejados, el listado que retorna tiene la opción de poder ser filtrado y paginado. 
    - **ActualizarProductos**: Es el encargado de actualizar los campos de la tabla producto mediante el SKU del producto
    - **EliminarProducto**: Es el encargado de realizar un borrado físico mediante el SKU del producto

# 3. Elementos técnicos utilizados <div id='elem' />

Entity Framework Core ⚙
--------------------
[![preview version](https://img.shields.io/badge/nuget-v3.1.0-blue)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.1.0) [![downloads](https://img.shields.io/badge/download-7M-green)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.1.0)

AutoMapper 🗺
-------------------
[![preview version](https://img.shields.io/badge/nuget-v10.1.1-blue)](https://www.nuget.org/packages/AutoMapper/10.1.1/) [![downloads](https://img.shields.io/badge/download-1M-green)](https://www.nuget.org/packages/AutoMapper/10.1.1/)

AutoMapper.Extensions.Microsoft.DependencyInjection 💉
-------------------
[![preview version](https://img.shields.io/badge/nuget-v8.1.0-blue)](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/8.1.0/) [![downloads](https://img.shields.io/badge/download-1M-green)](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/8.1.0/)

Microsoft.IdentityModel.Tokens 🎟
-------------------
[![preview version](https://img.shields.io/badge/nuget-v6.8.0-blue)](https://www.nuget.org/packages/Microsoft.IdentityModel.Tokens/6.8.0/) [![downloads](https://img.shields.io/badge/download-1M-green)](https://www.nuget.org/packages/Microsoft.IdentityModel.Tokens/6.8.0/)

System.IdentityModel.Tokens.Jwt 🆔
-------------------
[![preview version](https://img.shields.io/badge/nuget-v6.8.0-blue)](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/) [![downloads](https://img.shields.io/badge/download-1M-green)](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/)

MailKit 📫
-------------------
[![preview version](https://img.shields.io/badge/nuget-v2.10.0-blue)](https://www.nuget.org/packages/MailKit/2.10.0/) [![downloads](https://img.shields.io/badge/download-%2B116.000-green)](https://www.nuget.org/packages/MailKit/2.10.0/)

# 4. Buenas prácticas utilizadas 💻⌨👌 <div id='pract' />
- **Arquitectura Limpia**.
- Principios **SOLID**.
- Orientación **DDD** (*Domain Driven Design*)
- **Reutilización** de librerías ya testeadas y optimizadas.
- **Seguridad** mediante *JWT* para poder consultar endpoints de Productos. 
- **Documentación** de código y solución.
- **Versionamiento** de peticiones.
- **Filtrado y paginación** de listados mediante la peticiones web.

# 5. Instalación-Despliegue local 🛠 <div id='inst' />
1. Disponer de *VisualStudio* 2017/2019 o *Visual Studio Code* con el *[SDK .net core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)*
2. Clonar [este repositorio](https://github.com/jdepaz503/ApiLoginJdepaz)
3. Utlizar la rama ***development***
4. Correr los ***Script de migrations*** en un *MSSQL 2012*
5. Modificar la propiedad ***DefaultConnection*** ubicado en el proyecto *ApiLoginJdepaz.web*  en el archivo ***appsettings.json*** en la línea 12.
6. No debería ser necesario, dar clic derecho en proyecto *ApiLoginJdepaz.web* y luego en Establacer como proyecto de inicio
7. Verificar que en botón de depuración (botón play) tenga seleccionado el perfil ***ApiLoginJdepaz.web*** y no *IIS Express* u otro
8. Importar [colección de postman](https://www.getpostman.com/collections/08162aca2634897b8693)
9. Hacer las pruebas mediante *postman* ([seguir los siguientes pasos]()) o *swagger*.

# 6. Prueba de Web Api con Postman (con detalles e imagenes) <div id='prueba' />
## 6.1. Login 🚪 <div id='login' />
### 6.1.1. Login
#### Ejemplo:

```
http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/Auth

{
  "username": "jdepaz",
  "pass_user": "123456"
}
```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

## 6.2. Usuario 👥 <div id='user' />
### 6.2.1. Agregar Usuario <div id='add' />
#### Este método está protegido con authorize por lo cual es necesario utilizar antes el login para poder obtener un token y consumir esta funcionalidad. 

### 6.2.1.1. Copiar el token de la petición anterior del [paso 6.1.1](#login)
![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

### 6.2.1.2. Preparar petición con Authorization
![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)
#### En caso no aparezca la pestaña Authorization, se puede agregar en la pestaña headers de la siguiente manera: 
```
Key: Authorization
Value: Bearer <token_obtenido_de_login>
```
![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

## 6.2.1.3. Preparar petición 
#### Ejemplo:

```
http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/AgregarUsuario

{
  "nombre_user": "Elaniin Tech Company",
  "username": "elaniin",
  "email_user": "jdepaz@elaniin.com",
  "pass_user": "elaniin",
  "telefono_user": "22004519",
  "fnac_user": "2007-01-01T22:25:53.900Z"
}

```
![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

### 6.2.2. Obtener Usuarios <div id='obt' />
### 6.2.2.1. Hacer petición
#### Ejemplo:
```http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/ObtenerUsuarios```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

### 6.2.3. Desactivar Usuarios <div id='desac' />
#### 1. Preparar Authorize con Token de login
#### 2. Preparar petición
#### Ejemplo:
```
http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/DesactivarUsuario


{
  "username": "string"
}

```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

#### 3. Ejecutar el [paso 6.2.2](#obt)

### 6.2.4. Modificando Usuarios <div id='upd' />
#### 1. Preparar Authorize con Token de login
#### 2. Preparar petición
#### Ejemplo:
```
http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/ModificarUsuario

{
  "userId": 2,
  "nombre_user": "Johnny update test from Postman",
  "telefono_user": "61436459",
  "fnac_user": "2020-10-08T00:00:00.000Z"
}

```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

#### 3. Ejecutar el [paso 6.2.2](#obt)
![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

### 6.2.5. Password Reset 🔐 <div id='pwd' />
#### 1. Preparar petición
#### Ejemplo:
```
http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/PasswordReset

{
  "user": "jdepaz",
  "correo": "jdepaz2012@gmail.com"
}

```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

Lo que se hablaba anteriormente, actualmente el enlace del correo en este caso es la página oficial de Elaniin. Pero debe ser modificado por un enlace de un front que permita hacer la siguiente petición del paso 5.7 y recibir el token del paso 5.6 donde va inmerso el correo y user del usuario, además de enviar el nuevo password que le fue solicitado al usuario, para poder modificar la propiedad LinkFrontforEmail ubicado en el proyecto ApiLoginJdepaz.web  en el archivo appsettings.json en la línea 17.

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

### 6.2.5. Cambiar contraseña 🔏 <div id='pwdupd' />
#### 1. Preparar Authorize y copiar el token del paso [6.1.1](#login)
#### 2. Preparar petición
#### Ejemplo:
```
http://jdepaz2012-001-site1.ftempurl.com/api/v1.0/cambiarContraseña/

{
  "token": " ",
  "newPassword": "ElaniinTech"
}


```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)

## 6.3. Productos 📦 <div id='prod' />
#### Todos los métodos de este endpoint requieren de Authorize con token login
### 6.3.1. Obtener productos (filtrado y paginación) <div id='obtprod' />

**Clave**: valor string para indicar el nombre de la Columna por la cual filtrar, puede ser Nombre o SKU

**PatronOperador**:  valor INT dispone de una lista de filtro donde:

0-	Equals

1-	StartsWith

2-	EndsWith

3-	Contains

#### **Valor**: 

**Pagina**: valor INT para indicar que pagina queremos ver

**elementosPorPagina**: Valor 

#### Ejemplo:
```
{
  "filtros": [
{
"Clave": "Nombre",
"patronOperador": 3,
"Valor": ""
}],
  "pagina": 2,
  "elementosPorPagina": 2
}

```

![login](https://www.campusmvp.es/recursos/image.axd?picture=/Logos-Banners/Entity-Framework-Core.png)
