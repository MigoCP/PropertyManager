USE [master]
GO
/****** Object:  Database [PropertyRentalManagement]    Script Date: 2024-12-04 11:47:45 PM ******/
CREATE DATABASE [PropertyRentalManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PropertyRentalManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQL2022EXPRESS\MSSQL\DATA\PropertyRentalManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PropertyRentalManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQL2022EXPRESS\MSSQL\DATA\PropertyRentalManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PropertyRentalManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PropertyRentalManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PropertyRentalManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [PropertyRentalManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PropertyRentalManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PropertyRentalManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PropertyRentalManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PropertyRentalManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PropertyRentalManagement] SET  MULTI_USER 
GO
ALTER DATABASE [PropertyRentalManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PropertyRentalManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PropertyRentalManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PropertyRentalManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PropertyRentalManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PropertyRentalManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PropertyRentalManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [PropertyRentalManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PropertyRentalManagement]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Apartments]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Apartments](
	[ApartmentId] [int] IDENTITY(1,1) NOT NULL,
	[BuildingId] [int] NOT NULL,
	[Address] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[NumberOfRooms] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentId] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[ManagerId] [int] NOT NULL,
	[ApartmentId] [int] NOT NULL,
	[ScheduledDate] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[BuildingId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ManagerId] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BuildingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[SenderId] [int] NOT NULL,
	[ReceiverId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[SentAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[ManagerId] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ReportDate] [datetime] NOT NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[ScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[ManagerId] [int] NOT NULL,
	[DayOfWeek] [nvarchar](20) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024-12-04 11:47:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Apartments] ON 

INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (1, 1, N'101 Maple St, Cityville', N'1-bedroom apartment with a balcony.', N'Available', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (2, 1, N'102 Maple St, Cityville', N'2-bedroom apartment with a garden view.', N'Occupied', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (3, 2, N'201 Oakwood Ave, Cityville', N'Studio apartment with city views.', N'Available', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (16, 1, N'107 Maple St, Cityville', N'1-bedroom apartment with a pool view.', N'Available', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (17, 1, N'108 Maple St, Cityville', N'2-bedroom apartment with a garden terrace.', N'Occupied', 2)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (18, 2, N'206 Oakwood Ave, Cityville', N'3-bedroom penthouse with a city view.', N'Available', 3)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (19, 2, N'207 Oakwood Ave, Cityville', N'Studio apartment with modern interiors.', N'Under Maintenance', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (20, 3, N'301 Pine Grove Rd, Cityville', N'2-bedroom apartment with a balcony.', N'Available', 2)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (21, 3, N'302 Pine Grove Rd, Cityville', N'1-bedroom studio near the garden.', N'Occupied', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (22, 4, N'401 Elmwood Blvd, Cityville', N'3-bedroom family apartment with parking.', N'Available', 3)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (23, 4, N'402 Elmwood Blvd, Cityville', N'2-bedroom apartment near the park.', N'Under Maintenance', 2)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (24, 5, N'501 Birchwood Ln, Cityville', N'1-bedroom apartment with gym access.', N'Available', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (25, 5, N'502 Birchwood Ln, Cityville', N'2-bedroom apartment with hardwood flooring.', N'Occupied', 2)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (26, 1, N'109 Maple St, Cityville', N'Studio apartment with modern furnishings.', N'Available', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (27, 2, N'208 Oakwood Ave, Cityville', N'2-bedroom apartment with private parking.', N'Occupied', 2)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (28, 3, N'303 Pine Grove Rd, Cityville', N'1-bedroom apartment with quick access to downtown.', N'Available', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (29, 4, N'403 Elmwood Blvd, Cityville', N'Studio apartment with large windows.', N'Under Maintenance', 1)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (30, 5, N'503 Birchwood Ln, Cityville', N'3-bedroom apartment with a home office.', N'Available', 3)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (36, 2, N'456 Oakwood Ave, Cityville', N'test', N'Occupied', 3)
INSERT [dbo].[Apartments] ([ApartmentId], [BuildingId], [Address], [Description], [Status], [NumberOfRooms]) VALUES (37, 2, N'456 Oakwood Ave, Cityville', N'testFinal', N'Available', 2)
SET IDENTITY_INSERT [dbo].[Apartments] OFF
GO
SET IDENTITY_INSERT [dbo].[Appointments] ON 

INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (1, 3, 2, 1, CAST(N'2024-12-01T10:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (2, 3, 5, 1, CAST(N'2024-12-09T09:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (3, 3, 5, 16, CAST(N'2024-12-09T09:30:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (4, 3, 5, 16, CAST(N'2024-12-10T15:30:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (5, 3, 5, 1, CAST(N'2024-12-10T14:30:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (6, 7, 6, 30, CAST(N'2024-12-06T09:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (7, 7, 6, 3, CAST(N'2024-12-09T09:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (8, 10, 2, 20, CAST(N'2024-12-04T16:30:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (9, 3, 6, 18, CAST(N'2024-12-06T10:30:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (10, 7, 5, 22, CAST(N'2024-12-09T11:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (11, 7, 6, 3, CAST(N'2024-12-06T11:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (12, 10, 2, 20, CAST(N'2024-12-07T10:30:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Appointments] ([AppointmentId], [TenantId], [ManagerId], [ApartmentId], [ScheduledDate], [Status]) VALUES (13, 3, 5, 1, CAST(N'2024-12-04T13:30:00.000' AS DateTime), N'Pending')
SET IDENTITY_INSERT [dbo].[Appointments] OFF
GO
SET IDENTITY_INSERT [dbo].[Buildings] ON 

INSERT [dbo].[Buildings] ([BuildingId], [Name], [Address], [Description], [ManagerId], [OwnerId]) VALUES (1, N'Maple Apartments', N'123 Maple St, Cityville', N'A modern apartment building with all amenities.', 5, 1)
INSERT [dbo].[Buildings] ([BuildingId], [Name], [Address], [Description], [ManagerId], [OwnerId]) VALUES (2, N'Oakwood Residences', N'456 Oakwood Ave, Cityville', N'Luxury apartments in a prime location.', 6, 9)
INSERT [dbo].[Buildings] ([BuildingId], [Name], [Address], [Description], [ManagerId], [OwnerId]) VALUES (3, N'Pinewood Towers', N'789 Pinewood Ln, Cityville', N'A serene complex with garden views.', 2, 4)
INSERT [dbo].[Buildings] ([BuildingId], [Name], [Address], [Description], [ManagerId], [OwnerId]) VALUES (4, N'Elmwood Heights', N'123 Elm St, Cityville', N'Elegant apartments in a quiet neighborhood.', 5, 1)
INSERT [dbo].[Buildings] ([BuildingId], [Name], [Address], [Description], [ManagerId], [OwnerId]) VALUES (5, N'Cedar Residences', N'456 Cedar Rd, Cityville', N'Modern apartments with gym access.', 6, 9)
INSERT [dbo].[Buildings] ([BuildingId], [Name], [Address], [Description], [ManagerId], [OwnerId]) VALUES (7, N'Willow Creek Apartments', N'123 Willow Creek Rd, Cityville', N'A beautiful apartment complex with modern amenities.', 2, 1)
SET IDENTITY_INSERT [dbo].[Buildings] OFF
GO
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([MessageId], [SenderId], [ReceiverId], [Content], [SentAt]) VALUES (1, 3, 2, N'Can I visit the 1-bedroom apartment?', CAST(N'2024-11-25T22:30:23.650' AS DateTime))
INSERT [dbo].[Messages] ([MessageId], [SenderId], [ReceiverId], [Content], [SentAt]) VALUES (2, 2, 3, N'teds', CAST(N'2024-12-04T22:31:47.803' AS DateTime))
INSERT [dbo].[Messages] ([MessageId], [SenderId], [ReceiverId], [Content], [SentAt]) VALUES (3, 2, 3, N'test', CAST(N'2024-12-04T22:32:23.537' AS DateTime))
INSERT [dbo].[Messages] ([MessageId], [SenderId], [ReceiverId], [Content], [SentAt]) VALUES (4, 2, 2, N'test', CAST(N'2024-12-04T22:35:31.280' AS DateTime))
INSERT [dbo].[Messages] ([MessageId], [SenderId], [ReceiverId], [Content], [SentAt]) VALUES (5, 2, 2, N'test FINAL', CAST(N'2024-12-04T23:34:20.313' AS DateTime))
SET IDENTITY_INSERT [dbo].[Messages] OFF
GO
SET IDENTITY_INSERT [dbo].[Reports] ON 

INSERT [dbo].[Reports] ([ReportId], [ManagerId], [OwnerId], [Title], [Description], [ReportDate], [Status]) VALUES (1, 2, 1, N'Building Safety Inspection', N'A safety inspection was completed in all buildings.', CAST(N'2024-11-28T17:54:30.863' AS DateTime), N'Pending')
INSERT [dbo].[Reports] ([ReportId], [ManagerId], [OwnerId], [Title], [Description], [ReportDate], [Status]) VALUES (2, 2, 1, N'Tenant Complaints Summary', N'Summary of tenant complaints received this week.', CAST(N'2024-11-28T17:54:30.863' AS DateTime), N'Reviewed')
INSERT [dbo].[Reports] ([ReportId], [ManagerId], [OwnerId], [Title], [Description], [ReportDate], [Status]) VALUES (3, 2, 1, N'test', N'testtest', CAST(N'2024-12-04T10:00:00.000' AS DateTime), N'Pending')
INSERT [dbo].[Reports] ([ReportId], [ManagerId], [OwnerId], [Title], [Description], [ReportDate], [Status]) VALUES (4, 2, 4, N'test', N'test', CAST(N'2024-12-04T10:00:00.000' AS DateTime), N'Pending')
SET IDENTITY_INSERT [dbo].[Reports] OFF
GO
SET IDENTITY_INSERT [dbo].[Schedules] ON 

INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (1, 5, N'Monday', CAST(N'09:00:00' AS Time), CAST(N'12:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (2, 5, N'Tuesday', CAST(N'13:00:00' AS Time), CAST(N'16:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (3, 5, N'Wednesday', CAST(N'10:00:00' AS Time), CAST(N'14:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (4, 6, N'Monday', CAST(N'09:00:00' AS Time), CAST(N'11:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (5, 6, N'Thursday', CAST(N'14:00:00' AS Time), CAST(N'18:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (6, 6, N'Friday', CAST(N'09:00:00' AS Time), CAST(N'12:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (7, 2, N'Tuesday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (8, 2, N'Wednesday', CAST(N'13:00:00' AS Time), CAST(N'17:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleId], [ManagerId], [DayOfWeek], [StartTime], [EndTime]) VALUES (9, 2, N'Saturday', CAST(N'10:00:00' AS Time), CAST(N'14:00:00' AS Time))
SET IDENTITY_INSERT [dbo].[Schedules] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (1, N'Alice Johnson', N'alice@rental.com', N'password1', N'Owner', CAST(N'2024-11-25T22:30:23.647' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (2, N'Bob Smith', N'bob@rental.com', N'password2', N'Manager', CAST(N'2024-11-25T22:30:23.647' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (3, N'Charlie Brown', N'charlie@rental.com', N'password3', N'Tenant', CAST(N'2024-11-25T22:30:23.647' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (4, N'test', N'test@rental.com', N'123', N'Owner', CAST(N'2024-11-27T20:49:18.443' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (5, N'Michael Scott', N'michael@rental.com', N'password5', N'Manager', CAST(N'2024-12-03T19:30:59.350' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (6, N'Pam Beesly', N'pam@rental.com', N'password6', N'Manager', CAST(N'2024-12-03T19:30:59.350' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (7, N'Jim Halpert', N'jim@rental.com', N'password7', N'Tenant', CAST(N'2024-12-03T19:30:59.350' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (8, N'Dwight Schrute', N'dwight@rental.com', N'password8', N'Tenant', CAST(N'2024-12-03T19:30:59.350' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (9, N'Angela Martin', N'angela@rental.com', N'password9', N'Owner', CAST(N'2024-12-03T19:30:59.350' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (10, N'Ryan Howard', N'ryan@rental.com', N'password10', N'Tenant', CAST(N'2024-12-03T19:30:59.350' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [Password], [Role], [CreatedAt]) VALUES (11, N'test', N'test@test.com', N'test', N'Manager', CAST(N'2024-12-04T20:46:38.690' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534C488525B]    Script Date: 2024-12-04 11:47:45 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Apartments] ADD  DEFAULT ((1)) FOR [NumberOfRooms]
GO
ALTER TABLE [dbo].[Messages] ADD  DEFAULT (getdate()) FOR [SentAt]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT (getdate()) FOR [ReportDate]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Apartments]  WITH CHECK ADD FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([BuildingId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([ApartmentId])
REFERENCES [dbo].[Apartments] ([ApartmentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([TenantId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD FOREIGN KEY([ReceiverId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD FOREIGN KEY([SenderId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Apartments]  WITH CHECK ADD CHECK  (([Status]='Under Maintenance' OR [Status]='Occupied' OR [Status]='Available'))
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD CHECK  (([Status]='Cancelled' OR [Status]='Completed' OR [Status]='Pending'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='Tenant' OR [Role]='Manager' OR [Role]='Owner'))
GO
USE [master]
GO
ALTER DATABASE [PropertyRentalManagement] SET  READ_WRITE 
GO
