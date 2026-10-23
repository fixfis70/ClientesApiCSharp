# Testing del api clientes

## 1ero 
### Verificamos si el Auth funciona

![readme/img_1.png](readme/img_1.png)

_Se ve error 401_

## 2do
### Creamos un usuario
![readme/img_2.png](readme/img_2.png)

_status 200_

### Si ya hay uno
![readme/img_3.png](readme/img_3.png)

_error 409_

## 3ro
### Iniciamos session con el usuario creado
![readme/img_7.png](readme/img_7.png)

_Status 200 con el token_
## 4to
### Consultar la lista de clientes

![readme/img_5.png](readme/img_5.png)

_Status 200 y retorno de lista exitosa_

## 5to
### Intentamos eliminar un cliente

![readme/img_8.png](readme/img_8.png)

_Error 403_

## 6to
### Damos privilegios de administracion al usuario

![readme/img_9.png](readme/img_9.png)

_El Usuario se creó con el Id 103_

## 7to
### Eliminamos un cliente
> ⚠️ OJO
> 
> Tenenmos que volver a generar el token para refrescar los roles, esto ocurre porque en el token se estan enviando los roles, asi que al reutilizar el token antiguo se queda con los roles antiguos
> 
> Resumen: regenerar el token
> 
> ![readme/img_11.png](readme/img_11.png)
> 
> _Regeneracion de token_


![readme/img_10.png](readme/img_10.png)

_Status 200 y mensaje_

