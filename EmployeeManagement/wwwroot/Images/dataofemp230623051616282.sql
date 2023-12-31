USE [master]
GO
/****** Object:  Database [EmployeeManagement]    Script Date: 22-06-2023 13:28:29 ******/
CREATE DATABASE [EmployeeManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EmployeeManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmployeeManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EmployeeManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmployeeManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [EmployeeManagement] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EmployeeManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EmployeeManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EmployeeManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EmployeeManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EmployeeManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EmployeeManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [EmployeeManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EmployeeManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EmployeeManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EmployeeManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EmployeeManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EmployeeManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EmployeeManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EmployeeManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EmployeeManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EmployeeManagement] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EmployeeManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EmployeeManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EmployeeManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EmployeeManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EmployeeManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EmployeeManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EmployeeManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EmployeeManagement] SET RECOVERY FULL 
GO
ALTER DATABASE [EmployeeManagement] SET  MULTI_USER 
GO
ALTER DATABASE [EmployeeManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EmployeeManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EmployeeManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EmployeeManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EmployeeManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EmployeeManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EmployeeManagement', N'ON'
GO
ALTER DATABASE [EmployeeManagement] SET QUERY_STORE = OFF
GO
USE [EmployeeManagement]
GO
/****** Object:  User [sa1]    Script Date: 22-06-2023 13:28:29 ******/
CREATE USER [sa1] FOR LOGIN [sa1] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[EmployeeDesignation]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeDesignation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Designation] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeDetails]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NULL,
	[DesignationId] [int] NOT NULL,
	[ProfilePicturePath] [varchar](2000) NULL,
	[Salary] [money] NULL,
	[DateOfBirth] [date] NULL,
	[Email] [varchar](255) NULL,
	[Address] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EmployeeDesignation] ON 

INSERT [dbo].[EmployeeDesignation] ([Id], [Designation]) VALUES (1, N'Jr.Developer')
INSERT [dbo].[EmployeeDesignation] ([Id], [Designation]) VALUES (2, N'Sr.Developer')
INSERT [dbo].[EmployeeDesignation] ([Id], [Designation]) VALUES (1002, N'Sr.DevOps')
SET IDENTITY_INSERT [dbo].[EmployeeDesignation] OFF
GO
SET IDENTITY_INSERT [dbo].[EmployeeDetails] ON 

INSERT [dbo].[EmployeeDetails] ([Id], [Name], [DesignationId], [ProfilePicturePath], [Salary], [DateOfBirth], [Email], [Address]) VALUES (2023, N'Akash', 1002, N'C:\Users\om\source\repos\Practical 27\EmployeeManagementWebApplication\Image\20221118-pexels-oliver-sjöström-1433052.jpg', 57000.0000, CAST(N'1996-11-06' AS Date), N'abc@gmail.com', N'Mumbai')
INSERT [dbo].[EmployeeDetails] ([Id], [Name], [DesignationId], [ProfilePicturePath], [Salary], [DateOfBirth], [Email], [Address]) VALUES (2024, N'Mayur', 1, N'C:\Users\om\source\repos\Practical 27\EmployeeManagementWebApplication\Image\20221118-roman-synkevych-wX2L8L-fGeA-unsplash.jpg', 17000.0000, CAST(N'1997-07-16' AS Date), N'pqr@gmail.com', N'Surat')
SET IDENTITY_INSERT [dbo].[EmployeeDetails] OFF
GO
ALTER TABLE [dbo].[EmployeeDetails]  WITH CHECK ADD FOREIGN KEY([DesignationId])
REFERENCES [dbo].[EmployeeDesignation] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[DeleteEmployeeDetails]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteEmployeeDetails]
@Id int
AS
BEGIN

	DELETE FROM EmployeeDetails WHERE Id=@Id;

END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeDesignation]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEmployeeDesignation]
AS
BEGIN

	SELECT Id,Designation from EmployeeDesignation

END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeDetails]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEmployeeDetails]
AS
BEGIN

SELECT EmployeeDesignation.Id, EmployeeDesignation.Designation as Designation,
EmployeeDetails.Id,
EmployeeDetails.Name,
EmployeeDetails.DesignationId,
EmployeeDetails.ProfilePicturePath,
EmployeeDetails.Salary,
EmployeeDetails.DateOfBirth,
EmployeeDetails.Email,
EmployeeDetails.Address
FROM EmployeeDesignation
Right JOIN EmployeeDetails ON EmployeeDesignation.Id = EmployeeDetails.DesignationId;

END
GO
/****** Object:  StoredProcedure [dbo].[InsertEmployeeDetails]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertEmployeeDetails]
@Name varchar(255),
@DesignationId int,
@ProfilePicturePath varchar(2000),
@Salary money,
@DateOfBirth date,
@Email varchar(255),
@Address varchar(255)
AS
BEGIN

	INSERT INTO EmployeeDetails values(@Name,@DesignationId,@ProfilePicturePath,@Salary,@DateOfBirth,@Email,@Address)

END
GO
/****** Object:  StoredProcedure [dbo].[SelectEmployeeDetails]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectEmployeeDetails]
@Id int
AS
BEGIN

SELECT EmployeeDesignation.Id, EmployeeDesignation.Designation as Designation,
EmployeeDetails.Id,
EmployeeDetails.Name,
EmployeeDetails.DesignationId,
EmployeeDetails.ProfilePicturePath,
EmployeeDetails.Salary,
EmployeeDetails.DateOfBirth,
EmployeeDetails.Email,
EmployeeDetails.Address
FROM EmployeeDesignation
Right JOIN EmployeeDetails ON EmployeeDesignation.Id = EmployeeDetails.DesignationId WHERE EmployeeDetails.Id=@Id;

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateEmployeeDetails]    Script Date: 22-06-2023 13:28:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateEmployeeDetails]
@ID int,
@Name varchar(255),
@DesignationId int,
@ProfilePicturePath varchar(2000),
@Salary money,
@DateOfBirth date,
@Email varchar(255),
@Address varchar(255)
AS
BEGIN
		
		UPDATE EmployeeDetails SET Name = @Name,DesignationId = @DesignationId,ProfilePicturePath = @ProfilePicturePath,Salary = @Salary,
		DateOfBirth = @DateOfBirth,Email = @Email,Address = @Address WHERE Id = @ID

END
GO
USE [master]
GO
ALTER DATABASE [EmployeeManagement] SET  READ_WRITE 
GO
