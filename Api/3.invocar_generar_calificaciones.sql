-- Declaraci�n de variables para manejar el c�digo y mensaje de error
DECLARE @CodigoError INT;                     -- Variable para almacenar el c�digo de error que puede devolver el procedimiento
DECLARE @MensajeError NVARCHAR(255);          -- Variable para almacenar el mensaje de error que puede devolver el procedimiento

-- Ejecuci�n del procedimiento almacenado GenerarCalificaciones
-- Se pasa el par�metro @Cantidad con un valor de 1000000
-- Las variables @CodigoError y @MensajeError se utilizan para capturar cualquier error que ocurra durante la ejecuci�n
EXEC GenerarCalificaciones 
    @Cantidad = 1000000,                      -- N�mero de calificaciones a generar
    @CodigoError = @CodigoError OUTPUT,       -- Variable de salida para el c�digo de error
    @MensajeError = @MensajeError OUTPUT;     -- Variable de salida para el mensaje de error

-- Verifica el resultado de la ejecuci�n del procedimiento
-- Devuelve el c�digo y mensaje de error capturados
SELECT @CodigoError AS CodigoError,           -- Muestra el c�digo de error
       @MensajeError AS MensajeError;        -- Muestra el mensaje de error
