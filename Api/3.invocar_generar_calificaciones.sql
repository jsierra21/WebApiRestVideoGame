-- Declaración de variables para manejar el código y mensaje de error
DECLARE @CodigoError INT;                     -- Variable para almacenar el código de error que puede devolver el procedimiento
DECLARE @MensajeError NVARCHAR(255);          -- Variable para almacenar el mensaje de error que puede devolver el procedimiento

-- Ejecución del procedimiento almacenado GenerarCalificaciones
-- Se pasa el parámetro @Cantidad con un valor de 1000000
-- Las variables @CodigoError y @MensajeError se utilizan para capturar cualquier error que ocurra durante la ejecución
EXEC GenerarCalificaciones 
    @Cantidad = 1000000,                      -- Número de calificaciones a generar
    @CodigoError = @CodigoError OUTPUT,       -- Variable de salida para el código de error
    @MensajeError = @MensajeError OUTPUT;     -- Variable de salida para el mensaje de error

-- Verifica el resultado de la ejecución del procedimiento
-- Devuelve el código y mensaje de error capturados
SELECT @CodigoError AS CodigoError,           -- Muestra el código de error
       @MensajeError AS MensajeError;        -- Muestra el mensaje de error
