USE [master]
GO
/****** Object:  Database [Db_Production]    Script Date: 03-09-2024 01:46:57 PM ******/
CREATE DATABASE [Db_Production] ON  PRIMARY 
( NAME = N'Db_Production', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\Db_Production.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Db_Production_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\Db_Production_log.LDF' , SIZE = 576KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Db_Production] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Db_Production].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Db_Production] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Db_Production] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Db_Production] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Db_Production] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Db_Production] SET ARITHABORT OFF 
GO
ALTER DATABASE [Db_Production] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Db_Production] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Db_Production] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Db_Production] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Db_Production] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Db_Production] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Db_Production] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Db_Production] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Db_Production] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Db_Production] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Db_Production] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Db_Production] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Db_Production] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Db_Production] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Db_Production] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Db_Production] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Db_Production] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Db_Production] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Db_Production] SET  MULTI_USER 
GO
ALTER DATABASE [Db_Production] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Db_Production] SET DB_CHAINING OFF 
GO
USE [Db_Production]
GO
/****** Object:  Table [dbo].[EmailConfig]    Script Date: 03-09-2024 01:46:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailConfig](
	[Config_id] [int] IDENTITY(1,1) NOT NULL,
	[Config_port] [int] NULL,
	[Config_host] [nvarchar](max) NULL,
	[from_mail_id] [nvarchar](max) NULL,
	[from_password] [nvarchar](max) NULL,
 CONSTRAINT [PK_EmailConfig] PRIMARY KEY CLUSTERED 
(
	[Config_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplate]    Script Date: 03-09-2024 01:46:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplate](
	[template_id] [int] IDENTITY(1,1) NOT NULL,
	[template_subject] [nvarchar](max) NULL,
	[template_body] [nvarchar](max) NULL,
	[flag] [nvarchar](max) NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[template_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 03-09-2024 01:46:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[ErrorId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorName] [nvarchar](max) NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Registration]    Script Date: 03-09-2024 01:46:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Registration](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Email_Id] [nvarchar](max) NULL,
	[User_Password] [nvarchar](max) NULL,
	[Verify] [nvarchar](max) NULL,
	[Profile_Image] [nvarchar](max) NULL,
	[Created_Date] [datetime] NULL,
 CONSTRAINT [PK_User_Registration] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[UpdateOtp]    Script Date: 03-09-2024 01:46:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UpdateOtp]
@Email_Id nvarchar(MAX),
@Otp nvarchar(MAX)
as 
begin
Declare 
@UserId int;
 select @UserId =UserId from User_Registration where Email_Id=@Email_Id

Update User_Registration set 
User_Password = @Otp,
Created_Date = GETDATE()
where UserId=@UserId
end
GO
USE [master]
GO
ALTER DATABASE [Db_Production] SET  READ_WRITE 
GO
