# Práctica petición HTTP
Utilizando la siguiente API: https://test-deploy-12.onrender.com/estudiantes/,Enlaces a un sitio externo. la cual recibe como parámetro el número de carnet (Eje. 53390-00-1223).  Deberá construir una interfaz con React, que contenga los siguientes datos:

Consulta de alumnos: Carnet, Nombres, Correo electrónico y Sección.

El carnet debe ser ingresado en su formato natural, hay que tener cuidado de la tercera sección, ya que está diseñado para tener 5 digitos como máximo, en caso que el carnet tenga solo 4 digitos, se debe dejar un espacio después del segundo guión (-).

Ejemplos:
- 5390-15-12345
- 5390-15- 1234
- 5390-15-  123