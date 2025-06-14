USE [master]
GO
/****** Object:  Database [BDCOLEGIO]    Script Date: 6/1/2025 1:04:06 PM ******/
CREATE DATABASE [BDCOLEGIO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BDCOLEGIO', FILENAME = N'C:\SqlsData\MSSQL16.SQLEXPRESS\MSSQL\DATA\BDCOLEGIO.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BDCOLEGIO_log', FILENAME = N'C:\SqlsData\MSSQL16.SQLEXPRESS\MSSQL\DATA\BDCOLEGIO_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BDCOLEGIO] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BDCOLEGIO].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BDCOLEGIO] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET ARITHABORT OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BDCOLEGIO] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BDCOLEGIO] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BDCOLEGIO] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BDCOLEGIO] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BDCOLEGIO] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BDCOLEGIO] SET  MULTI_USER 
GO
ALTER DATABASE [BDCOLEGIO] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BDCOLEGIO] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BDCOLEGIO] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BDCOLEGIO] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BDCOLEGIO] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BDCOLEGIO] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BDCOLEGIO] SET QUERY_STORE = ON
GO
ALTER DATABASE [BDCOLEGIO] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BDCOLEGIO]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[State] [bit] NULL,
	[RegistrationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StateTuition]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateTuition](
	[StateId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[State] [bit] NULL,
	[RegistrationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Age] [int] NULL,
	[State] [bit] NULL,
	[RegistrationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tuition]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tuition](
	[TuitionId] [int] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NOT NULL,
	[CourseId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[State] [bit] NULL,
	[RegistrationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[TuitionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Course] ON 

INSERT [dbo].[Course] ([CourseId], [Name], [State], [RegistrationDate]) VALUES (1, N'Matemáticas', 1, CAST(N'2025-06-01T06:01:03.050' AS DateTime))
INSERT [dbo].[Course] ([CourseId], [Name], [State], [RegistrationDate]) VALUES (2, N'Historia', 1, CAST(N'2025-06-01T06:01:03.050' AS DateTime))
INSERT [dbo].[Course] ([CourseId], [Name], [State], [RegistrationDate]) VALUES (3, N'Biología', 1, CAST(N'2025-06-01T06:01:03.050' AS DateTime))
INSERT [dbo].[Course] ([CourseId], [Name], [State], [RegistrationDate]) VALUES (4, N'Fisica', 1, CAST(N'2025-06-01T08:20:51.827' AS DateTime))
SET IDENTITY_INSERT [dbo].[Course] OFF
GO
SET IDENTITY_INSERT [dbo].[StateTuition] ON 

INSERT [dbo].[StateTuition] ([StateId], [Name], [State], [RegistrationDate]) VALUES (1, N'Activa', 1, CAST(N'2025-06-01T06:02:01.900' AS DateTime))
INSERT [dbo].[StateTuition] ([StateId], [Name], [State], [RegistrationDate]) VALUES (2, N'Cancelada', 1, CAST(N'2025-06-01T06:02:01.900' AS DateTime))
INSERT [dbo].[StateTuition] ([StateId], [Name], [State], [RegistrationDate]) VALUES (3, N'Finalizada', 1, CAST(N'2025-06-01T06:02:01.900' AS DateTime))
SET IDENTITY_INSERT [dbo].[StateTuition] OFF
GO
SET IDENTITY_INSERT [dbo].[Student] ON 

INSERT [dbo].[Student] ([StudentId], [Name], [LastName], [Age], [State], [RegistrationDate]) VALUES (1, N'Juan', N'Pérez', 16, 1, CAST(N'2025-06-01T06:01:16.907' AS DateTime))
INSERT [dbo].[Student] ([StudentId], [Name], [LastName], [Age], [State], [RegistrationDate]) VALUES (2, N'Ana', N'Gómez', 17, 1, CAST(N'2025-06-01T06:01:16.907' AS DateTime))
INSERT [dbo].[Student] ([StudentId], [Name], [LastName], [Age], [State], [RegistrationDate]) VALUES (3, N'Luis', N'Ramírez', 15, 1, CAST(N'2025-06-01T06:01:16.907' AS DateTime))
INSERT [dbo].[Student] ([StudentId], [Name], [LastName], [Age], [State], [RegistrationDate]) VALUES (4, N'Alfred', N'Huarcaya', 12, 1, CAST(N'2025-06-01T08:26:54.277' AS DateTime))
SET IDENTITY_INSERT [dbo].[Student] OFF
GO
SET IDENTITY_INSERT [dbo].[Tuition] ON 

INSERT [dbo].[Tuition] ([TuitionId], [StudentId], [CourseId], [StateId], [State], [RegistrationDate]) VALUES (2, 2, 2, 1, 1, CAST(N'2025-06-01T06:02:38.727' AS DateTime))
INSERT [dbo].[Tuition] ([TuitionId], [StudentId], [CourseId], [StateId], [State], [RegistrationDate]) VALUES (3, 3, 3, 3, 1, CAST(N'2025-06-01T06:02:38.727' AS DateTime))
INSERT [dbo].[Tuition] ([TuitionId], [StudentId], [CourseId], [StateId], [State], [RegistrationDate]) VALUES (4, 1, 1, 1, 1, CAST(N'2025-06-01T11:42:14.210' AS DateTime))
SET IDENTITY_INSERT [dbo].[Tuition] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__StateTui__737584F63F8A08BC]    Script Date: 6/1/2025 1:04:06 PM ******/
ALTER TABLE [dbo].[StateTuition] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Tuition_Student_Course]    Script Date: 6/1/2025 1:04:06 PM ******/
ALTER TABLE [dbo].[Tuition] ADD  CONSTRAINT [UQ_Tuition_Student_Course] UNIQUE NONCLUSTERED 
(
	[StudentId] ASC,
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Course] ADD  DEFAULT (getdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[StateTuition] ADD  DEFAULT (getdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[Student] ADD  DEFAULT (getdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[Tuition] ADD  DEFAULT (getdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[Tuition]  WITH CHECK ADD  CONSTRAINT [FK_Tuition_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([CourseId])
GO
ALTER TABLE [dbo].[Tuition] CHECK CONSTRAINT [FK_Tuition_Course]
GO
ALTER TABLE [dbo].[Tuition]  WITH CHECK ADD  CONSTRAINT [FK_Tuition_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateTuition] ([StateId])
GO
ALTER TABLE [dbo].[Tuition] CHECK CONSTRAINT [FK_Tuition_State]
GO
ALTER TABLE [dbo].[Tuition]  WITH CHECK ADD  CONSTRAINT [FK_Tuition_Student] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[Tuition] CHECK CONSTRAINT [FK_Tuition_Student]
GO
/****** Object:  StoredProcedure [dbo].[spActualizarEstadoMatricula]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spActualizarEstadoMatricula]
    @TuitionId INT,
    @StateId INT,
    @OutputMessage NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EstadoActual NVARCHAR(20);
    DECLARE @NuevoEstado NVARCHAR(20);

    -- Obtener el estado actual
    SELECT @EstadoActual = st.Name
    FROM Tuition t
    JOIN StateTuition st ON t.StateId = st.StateId
    WHERE t.TuitionId = @TuitionId;

    IF @EstadoActual IS NULL
    BEGIN
        SET @OutputMessage = 'Matrícula no encontrada.';
        RETURN -1;
    END

    -- Obtener el nuevo estado
    SELECT @NuevoEstado = Name FROM StateTuition WHERE StateId = @StateId;

    IF @NuevoEstado IS NULL
    BEGIN
        SET @OutputMessage = 'Estado nuevo inválido.';
        RETURN -1;
    END

    -- Validar reglas de transición
    IF @EstadoActual = 'Finalizada' AND @NuevoEstado = 'Cancelada'
    BEGIN
        SET @OutputMessage = 'No se puede cancelar una matrícula finalizada.';
        RETURN -1;
    END

    -- Actualizar el estado
    UPDATE Tuition
    SET StateId = @StateId
    WHERE TuitionId = @TuitionId;

    SET @OutputMessage = 'Estado actualizado correctamente.';
    RETURN 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[spCrearMatricula]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCrearMatricula]
    @StudentId INT,
    @CourseId INT,
    @StateId INT, -- por ejemplo: 1 para "Activa"
    @OutputMessage NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar existencia y estado activo del estudiante
    IF NOT EXISTS (SELECT 1 FROM Student WHERE StudentId = @StudentId AND State = 1)
    BEGIN
        SET @OutputMessage = 'El estudiante no existe o no está activo.';
        RETURN -1;
    END

    -- Validar existencia y estado activo del curso
    IF NOT EXISTS (SELECT 1 FROM Course WHERE CourseId = @CourseId AND State = 1)
    BEGIN
        SET @OutputMessage = 'El curso no existe o no está activo.';
        RETURN -1;
    END

    -- Validar que no haya matrícula duplicada (StudentId y CourseId únicos)
    IF EXISTS (SELECT 1 FROM Tuition WHERE StudentId = @StudentId AND CourseId = @CourseId AND State = 1)
    BEGIN
        SET @OutputMessage = 'El estudiante ya está matriculado en este curso.';
        RETURN -1;
    END

    -- Insertar nueva matrícula
    INSERT INTO Tuition (StudentId, CourseId, StateId, State, RegistrationDate)
    VALUES (@StudentId, @CourseId, @StateId, 1, GETDATE());

    SET @OutputMessage = 'Matrícula creada correctamente.';
    RETURN 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[spCreateEnrollment]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCreateEnrollment]
    @StudentId INT,
    @CourseId INT,
    @StateId INT, -- por ejemplo: 1 para "Activa"
    @OutputMessage NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar existencia y estado activo del estudiante
    IF NOT EXISTS (SELECT 1 FROM Student WHERE StudentId = @StudentId AND State = 1)
    BEGIN
        SET @OutputMessage = 'Student does not exist or is not active.';
        RETURN -1;
    END

    -- Validar existencia y estado activo del curso
    IF NOT EXISTS (SELECT 1 FROM Course WHERE CourseId = @CourseId AND State = 1)
    BEGIN
        SET @OutputMessage = 'Course does not exist or is not active.';
        RETURN -1;
    END

    -- Validar que no haya matrícula duplicada (StudentId y CourseId únicos)
    IF EXISTS (SELECT 1 FROM Tuition WHERE StudentId = @StudentId AND CourseId = @CourseId AND State = 1)
    BEGIN
        SET @OutputMessage = 'Student is already enrolled in this course.';
        RETURN -1;
    END

    -- Insertar nueva matrícula
    INSERT INTO Tuition (StudentId, CourseId, StateId, State, RegistrationDate)
    VALUES (@StudentId, @CourseId, @StateId, 1, GETDATE());

    SET @OutputMessage = 'Enrollment created successfully.';
    RETURN 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[spEliminarMatricula]    Script Date: 6/1/2025 1:04:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spEliminarMatricula]
    @TuitionId INT,
    @OutputMessage NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EstadoActual NVARCHAR(20);

    SELECT @EstadoActual = st.Name
    FROM Tuition t
    JOIN StateTuition st ON t.StateId = st.StateId
    WHERE t.TuitionId = @TuitionId;

    IF @EstadoActual IS NULL
    BEGIN
        SET @OutputMessage = 'Matrícula no encontrada.';
        RETURN -1;
    END

    IF @EstadoActual != 'Cancelada'
    BEGIN
        SET @OutputMessage = 'Solo se pueden eliminar matrículas con estado Cancelada.';
        RETURN -1;
    END

    DELETE FROM Tuition WHERE TuitionId = @TuitionId;

    SET @OutputMessage = 'Matrícula eliminada correctamente.';
    RETURN 0;
END;
GO
USE [master]
GO
ALTER DATABASE [BDCOLEGIO] SET  READ_WRITE 
GO
