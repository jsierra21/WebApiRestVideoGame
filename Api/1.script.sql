USE [VideoGameTest]
GO
/****** Object:  User [[App_User]]    Script Date: 10/3/2024 3:54:33 PM ******/
CREATE USER [App_User] FOR LOGIN [App_User] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [App_User]
GO
ALTER ROLE [db_datareader] ADD MEMBER [App_User]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [App_User]
GO
/****** Object:  Table [dbo].[Videojuegos]    Script Date: 10/3/2024 3:54:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Videojuegos](
	[VideojuegoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[Compania] [nvarchar](255) NOT NULL,
	[AnioLanzamiento] [int] NOT NULL,
	[Precio] [numeric](20, 2) NULL,
	[PuntajePromedio] [decimal](3, 2) NOT NULL,
	[FechaActualizacion] [datetime] NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VideojuegoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Calificaciones]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calificaciones](
	[CalificacionID] [uniqueidentifier] NOT NULL,
	[Nickname] [nvarchar](100) NOT NULL,
	[VideojuegoID] [int] NOT NULL,
	[Puntuacion] [decimal](3, 2) NOT NULL,
	[FechaActualizacion] [datetime] NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CalificacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vw_VideojuegosCalificaciones]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Vw_VideojuegosCalificaciones] AS
SELECT 
    v.VideojuegoID,
    v.Nombre,
    v.Compania,
    v.AnioLanzamiento,
    v.Precio,
    AVG(c.Puntuacion) AS PuntuacionPromedio
FROM 
    dbo.Videojuegos v
LEFT JOIN 
    dbo.Calificaciones c ON v.VideojuegoID = c.VideojuegoID
GROUP BY 
    v.VideojuegoID, v.Nombre, v.Compania, v.AnioLanzamiento, v.Precio;
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Usr_IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Usr_nombre_usuario] [varchar](100) NOT NULL,
	[Usr_correo_electronico] [nvarchar](256) NOT NULL,
	[Usr_Password] [nvarchar](150) NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[CodUserUpdate] [varchar](30) NOT NULL,
	[FechaRegistroUpdate] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Calificaciones] ADD  DEFAULT (newid()) FOR [CalificacionID]
GO
ALTER TABLE [dbo].[Calificaciones] ADD  DEFAULT (getdate()) FOR [FechaActualizacion]
GO
ALTER TABLE [dbo].[Videojuegos] ADD  DEFAULT ((0)) FOR [PuntajePromedio]
GO
ALTER TABLE [dbo].[Videojuegos] ADD  DEFAULT (getdate()) FOR [FechaActualizacion]
GO
ALTER TABLE [dbo].[Calificaciones]  WITH CHECK ADD FOREIGN KEY([VideojuegoID])
REFERENCES [dbo].[Videojuegos] ([VideojuegoID])
GO
ALTER TABLE [dbo].[Calificaciones]  WITH CHECK ADD CHECK  (([Puntuacion]>=(0) AND [Puntuacion]<=(5)))
GO
/****** Object:  StoredProcedure [dbo].[SpAutenticacion]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jorge Sierra
-- Create date: <Create Date,,>
-- Description:	Gestion de Usuarios
-- =============================================

--EXEC  [SpAutenticacion] @Usr_correo_electronico	 = 'string',  @Usr_Password = 'aqui'
CREATE PROCEDURE [dbo].[SpAutenticacion] 
  
      @Usr_correo_electronico	VARCHAR(200)		= NULL
    , @Usr_Password                VARCHAR(200)	= NULL

AS
BEGIN
    DECLARE @storedProcedureName VARCHAR(MAX)
    SELECT @storedProcedureName = OBJECT_NAME(@@PROCID)

	DECLARE @resp INT = 0, 
			@RecibirErrorMessage VARCHAR(MAX) = ''

        DECLARE @fechaUltimaPassw_ DATE,
                @valorParam_ VARCHAR(200)

        SELECT top(1)
             u.[Usr_IdUsuario] as Usr_IdUsuario
            , u.[Usr_nombre_usuario]
            , u.[Usr_correo_electronico]
            , u.[FechaRegistro]
            , u.[CodUserUpdate]
            , u.[FechaRegistroUpdate]
			, u.[Usr_Password]
        FROM Usuario u (NOLOCK)
        WHERE u.Usr_correo_electronico = @Usr_correo_electronico 
		---> and [Usr_Password] = @Usr_Password
      

END
GO
/****** Object:  StoredProcedure [dbo].[SpGenerarCalificaciones]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SpGenerarCalificaciones]
    @Cantidad INT = 0, -- Parámetro de entrada con valor por defecto 0
    @CodigoError INT OUTPUT, -- Parámetro de salida para el código de error
    @MensajeError NVARCHAR(255) OUTPUT -- Parámetro de salida para el mensaje de error
AS
BEGIN
    SET NOCOUNT ON;

    -- Inicializar parámetros de salida
    SET @CodigoError = 0;
    SET @MensajeError = NULL;

    BEGIN TRY
        -- Validar el parámetro de entrada
        IF @Cantidad <= 0
        BEGIN
            SET @CodigoError = 1;
            SET @MensajeError = 'El valor ingresado no es válido.';
            RETURN;
        END

        -- Variables para la generación de datos
        DECLARE @i INT = 0;
        DECLARE @Nickname NVARCHAR(30);
        DECLARE @Puntuacion DECIMAL(3, 2);
        DECLARE @VideojuegoID INT;

        -- Bucle para generar la cantidad de calificaciones
        WHILE @i < @Cantidad
        BEGIN
            -- Generar Nickname aleatorio (letras y números)
            SET @Nickname = SUBSTRING(CONVERT(NVARCHAR(36), NEWID()), 1, 30); 

            -- Generar Puntuación aleatoria entre 0 y 5 con 2 decimales
            SET @Puntuacion = ROUND((RAND() * 5), 2);

            -- Asignar un VideojuegoID aleatorio existente
            SELECT TOP 1 @VideojuegoID = VideojuegoID FROM Videojuegos ORDER BY NEWID();

            -- Insertar la calificación en la tabla
            INSERT INTO Calificaciones (Nickname, VideojuegoID, Puntuacion, FechaActualizacion, Usuario)
            VALUES (@Nickname, @VideojuegoID, @Puntuacion, GETDATE(), 'Admin');

            SET @i = @i + 1; -- Incrementar contador
        END
    END TRY
    BEGIN CATCH
        -- Capturar errores y asignar a los parámetros de salida
        SET @CodigoError = ERROR_NUMBER();
        SET @MensajeError = ERROR_MESSAGE();
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[SpListarVideoJuegos]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpListarVideoJuegos]
AS
BEGIN
    SET NOCOUNT ON;

    -- Selecciona todos los registros de la tabla Videojuegos
    SELECT 
        VideojuegoID,
        Nombre,
        Compania,
        AnioLanzamiento,
        Precio,
        PuntajePromedio,
        FechaActualizacion,
        Usuario
    FROM 
        [dbo].[Videojuegos]
    ORDER BY 
        Nombre ASC; -- Ordena los videojuegos por nombre
END
GO
/****** Object:  StoredProcedure [dbo].[SpRegistrarUser]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpRegistrarUser]
    @Usr_nombre_usuario VARCHAR(100),  -- Nombre de usuario a registrar (máximo 100 caracteres)
    @Usr_correo_electronico NVARCHAR(256), -- Correo electrónico del usuario (máximo 256 caracteres)
    @Usr_Password NVARCHAR(150) -- Contraseña del usuario (máximo 150 caracteres)
AS
BEGIN
    -- Inserta un nuevo registro en la tabla Usuario
    INSERT INTO [Usuario] 
        (Usr_nombre_usuario, Usr_correo_electronico, Usr_Password, FechaRegistro, CodUserUpdate, FechaRegistroUpdate)
    VALUES 
        (@Usr_nombre_usuario,                -- Valor del nombre de usuario
         @Usr_correo_electronico,            -- Valor del correo electrónico
         @Usr_Password,                       -- Valor de la contraseña
         GETDATE(),                          -- Fecha y hora actual como FechaRegistro
         '1',                                 -- Código de usuario que actualiza el registro (placeholder)
         GETDATE())                          -- Fecha y hora actual como FechaRegistroUpdate
END
GO
/****** Object:  StoredProcedure [usr].[SpUsuario]    Script Date: 10/3/2024 3:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

GO
