USE [master]
GO
/****** Object:  Database [HTQLVTYT]    Script Date: 22/5/2025 4:54:02 PM ******/
CREATE DATABASE [HTQLVTYT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HTQLVTYT', FILENAME = N'E:\SSMS\MSSQL16.SQLEXPRESS\MSSQL\DATA\HTQLVTYT.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HTQLVTYT_log', FILENAME = N'E:\SSMS\MSSQL16.SQLEXPRESS\MSSQL\DATA\HTQLVTYT_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [HTQLVTYT] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HTQLVTYT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HTQLVTYT] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HTQLVTYT] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HTQLVTYT] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HTQLVTYT] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HTQLVTYT] SET ARITHABORT OFF 
GO
ALTER DATABASE [HTQLVTYT] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HTQLVTYT] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HTQLVTYT] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HTQLVTYT] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HTQLVTYT] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HTQLVTYT] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HTQLVTYT] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HTQLVTYT] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HTQLVTYT] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HTQLVTYT] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HTQLVTYT] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HTQLVTYT] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HTQLVTYT] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HTQLVTYT] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HTQLVTYT] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HTQLVTYT] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HTQLVTYT] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HTQLVTYT] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HTQLVTYT] SET  MULTI_USER 
GO
ALTER DATABASE [HTQLVTYT] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HTQLVTYT] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HTQLVTYT] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HTQLVTYT] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HTQLVTYT] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HTQLVTYT] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [HTQLVTYT] SET QUERY_STORE = ON
GO
ALTER DATABASE [HTQLVTYT] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [HTQLVTYT]
GO
/****** Object:  Table [dbo].[tbl_account]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_account](
	[account_username] [nvarchar](255) NOT NULL,
	[account_password] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_account_account_username_primary] PRIMARY KEY CLUSTERED 
(
	[account_username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_contact]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_contact](
	[contact_id] [varchar](255) NOT NULL,
	[contact_type] [int] NOT NULL,
	[contact_address] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_contact_contact_id_primary] PRIMARY KEY CLUSTERED 
(
	[contact_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_device]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_device](
	[device_id] [varchar](255) NOT NULL,
	[device_name] [nvarchar](255) NOT NULL,
	[device_manufacturer] [nvarchar](255) NOT NULL,
	[device_seri] [nvarchar](255) NOT NULL,
	[device_type] [nvarchar](255) NOT NULL,
	[device_group] [nvarchar](255) NOT NULL,
	[device_maintenance_cycle] [int] NOT NULL,
	[device_maintenance_start] [date] NOT NULL,
	[device_stockout_date] [datetime] NOT NULL,
	[device_condition] [int] NOT NULL,
	[device_received_date] [datetime] NOT NULL,
	[device_note] [text] NOT NULL,
	[FK_contract_id] [varchar](255) NOT NULL,
	[FK_status_id] [varchar](255) NOT NULL,
	[FK_room_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_device_device_id_primary] PRIMARY KEY CLUSTERED 
(
	[device_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_finance]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_finance](
	[finance_id] [varchar](255) NOT NULL,
	[finance_purchase] [int] NOT NULL,
	[finance_maintain] [int] NOT NULL,
	[finance_repair] [int] NOT NULL,
	[finance_type] [int] NOT NULL,
 CONSTRAINT [tbl_finance_finance_id_primary] PRIMARY KEY CLUSTERED 
(
	[finance_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_maintain]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_maintain](
	[maintain_id] [varchar](255) NOT NULL,
	[maintain_date] [date] NOT NULL,
	[maintain_maintenance] [nvarchar](255) NOT NULL,
	[maintain_maintenance_phone] [nvarchar](255) NOT NULL,
	[maintain_delivery] [nvarchar](255) NOT NULL,
	[maintain_delivery_phone] [nvarchar](255) NOT NULL,
	[FK_device_id] [varchar](255) NOT NULL,
	[FK_status_id] [varchar](255) NOT NULL,
	[FK_room_id] [varchar](255) NOT NULL,
	[FK_finace_id] [varchar](255) NOT NULL,
	[FK_contact_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_maintain_maintain_id_primary] PRIMARY KEY CLUSTERED 
(
	[maintain_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_repair]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_repair](
	[repair_id] [varchar](255) NOT NULL,
	[repair_broken] [date] NOT NULL,
	[repair_priority] [int] NOT NULL,
	[repair_date] [date] NOT NULL,
	[repair_update_date] [datetime] NOT NULL,
	[repair_update_status] [int] NOT NULL,
	[repair_note] [text] NOT NULL,
	[repair_picture] [varbinary](max) NOT NULL,
	[repair_update_note] [text] NOT NULL,
	[FK_contact_id] [varchar](255) NOT NULL,
	[FK_room_id] [varchar](255) NOT NULL,
	[FK_device_id] [varchar](255) NOT NULL,
	[FK_status_id] [varchar](255) NOT NULL,
	[FK_finance_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_repair_repair_id_primary] PRIMARY KEY CLUSTERED 
(
	[repair_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_room]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_room](
	[room_id] [varchar](255) NOT NULL,
	[room_type] [nvarchar](255) NOT NULL,
	[room_name] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_room_room_id_primary] PRIMARY KEY CLUSTERED 
(
	[room_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_status]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_status](
	[status_id] [varchar](255) NOT NULL,
	[status_maintain] [int] NOT NULL,
	[status_repair] [int] NOT NULL,
	[status_store] [int] NOT NULL,
 CONSTRAINT [tbl_status_status_id_primary] PRIMARY KEY CLUSTERED 
(
	[status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_storage]    Script Date: 22/5/2025 4:54:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_storage](
	[storage_id] [varchar](255) NOT NULL,
	[storage_quantity] [int] NOT NULL,
	[storage_min] [int] NOT NULL,
	[FK_device_id] [varchar](255) NOT NULL,
	[FK_status_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_storage_storage_id_primary] PRIMARY KEY CLUSTERED 
(
	[storage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'leminhha', N'123')
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'nguyenan', N'135')
GO
ALTER TABLE [dbo].[tbl_device]  WITH CHECK ADD  CONSTRAINT [tbl_device_fk_contract_id_foreign] FOREIGN KEY([FK_contract_id])
REFERENCES [dbo].[tbl_contact] ([contact_id])
GO
ALTER TABLE [dbo].[tbl_device] CHECK CONSTRAINT [tbl_device_fk_contract_id_foreign]
GO
ALTER TABLE [dbo].[tbl_device]  WITH CHECK ADD  CONSTRAINT [tbl_device_fk_room_id_foreign] FOREIGN KEY([FK_room_id])
REFERENCES [dbo].[tbl_room] ([room_id])
GO
ALTER TABLE [dbo].[tbl_device] CHECK CONSTRAINT [tbl_device_fk_room_id_foreign]
GO
ALTER TABLE [dbo].[tbl_device]  WITH CHECK ADD  CONSTRAINT [tbl_device_fk_status_id_foreign] FOREIGN KEY([FK_status_id])
REFERENCES [dbo].[tbl_status] ([status_id])
GO
ALTER TABLE [dbo].[tbl_device] CHECK CONSTRAINT [tbl_device_fk_status_id_foreign]
GO
ALTER TABLE [dbo].[tbl_maintain]  WITH CHECK ADD  CONSTRAINT [tbl_maintain_fk_contact_id_foreign] FOREIGN KEY([FK_contact_id])
REFERENCES [dbo].[tbl_contact] ([contact_id])
GO
ALTER TABLE [dbo].[tbl_maintain] CHECK CONSTRAINT [tbl_maintain_fk_contact_id_foreign]
GO
ALTER TABLE [dbo].[tbl_maintain]  WITH CHECK ADD  CONSTRAINT [tbl_maintain_fk_device_id_foreign] FOREIGN KEY([FK_device_id])
REFERENCES [dbo].[tbl_device] ([device_id])
GO
ALTER TABLE [dbo].[tbl_maintain] CHECK CONSTRAINT [tbl_maintain_fk_device_id_foreign]
GO
ALTER TABLE [dbo].[tbl_maintain]  WITH CHECK ADD  CONSTRAINT [tbl_maintain_fk_finace_id_foreign] FOREIGN KEY([FK_finace_id])
REFERENCES [dbo].[tbl_finance] ([finance_id])
GO
ALTER TABLE [dbo].[tbl_maintain] CHECK CONSTRAINT [tbl_maintain_fk_finace_id_foreign]
GO
ALTER TABLE [dbo].[tbl_maintain]  WITH CHECK ADD  CONSTRAINT [tbl_maintain_fk_room_id_foreign] FOREIGN KEY([FK_room_id])
REFERENCES [dbo].[tbl_room] ([room_id])
GO
ALTER TABLE [dbo].[tbl_maintain] CHECK CONSTRAINT [tbl_maintain_fk_room_id_foreign]
GO
ALTER TABLE [dbo].[tbl_maintain]  WITH CHECK ADD  CONSTRAINT [tbl_maintain_fk_status_id_foreign] FOREIGN KEY([FK_status_id])
REFERENCES [dbo].[tbl_status] ([status_id])
GO
ALTER TABLE [dbo].[tbl_maintain] CHECK CONSTRAINT [tbl_maintain_fk_status_id_foreign]
GO
ALTER TABLE [dbo].[tbl_repair]  WITH CHECK ADD  CONSTRAINT [tbl_repair_fk_contact_id_foreign] FOREIGN KEY([FK_contact_id])
REFERENCES [dbo].[tbl_contact] ([contact_id])
GO
ALTER TABLE [dbo].[tbl_repair] CHECK CONSTRAINT [tbl_repair_fk_contact_id_foreign]
GO
ALTER TABLE [dbo].[tbl_repair]  WITH CHECK ADD  CONSTRAINT [tbl_repair_fk_device_id_foreign] FOREIGN KEY([FK_device_id])
REFERENCES [dbo].[tbl_device] ([device_id])
GO
ALTER TABLE [dbo].[tbl_repair] CHECK CONSTRAINT [tbl_repair_fk_device_id_foreign]
GO
ALTER TABLE [dbo].[tbl_repair]  WITH CHECK ADD  CONSTRAINT [tbl_repair_fk_finance_id_foreign] FOREIGN KEY([FK_finance_id])
REFERENCES [dbo].[tbl_finance] ([finance_id])
GO
ALTER TABLE [dbo].[tbl_repair] CHECK CONSTRAINT [tbl_repair_fk_finance_id_foreign]
GO
ALTER TABLE [dbo].[tbl_repair]  WITH CHECK ADD  CONSTRAINT [tbl_repair_fk_room_id_foreign] FOREIGN KEY([FK_room_id])
REFERENCES [dbo].[tbl_room] ([room_id])
GO
ALTER TABLE [dbo].[tbl_repair] CHECK CONSTRAINT [tbl_repair_fk_room_id_foreign]
GO
ALTER TABLE [dbo].[tbl_repair]  WITH CHECK ADD  CONSTRAINT [tbl_repair_fk_status_id_foreign] FOREIGN KEY([FK_status_id])
REFERENCES [dbo].[tbl_status] ([status_id])
GO
ALTER TABLE [dbo].[tbl_repair] CHECK CONSTRAINT [tbl_repair_fk_status_id_foreign]
GO
ALTER TABLE [dbo].[tbl_storage]  WITH CHECK ADD  CONSTRAINT [tbl_storage_fk_device_id_foreign] FOREIGN KEY([FK_device_id])
REFERENCES [dbo].[tbl_device] ([device_id])
GO
ALTER TABLE [dbo].[tbl_storage] CHECK CONSTRAINT [tbl_storage_fk_device_id_foreign]
GO
ALTER TABLE [dbo].[tbl_storage]  WITH CHECK ADD  CONSTRAINT [tbl_storage_fk_status_id_foreign] FOREIGN KEY([FK_status_id])
REFERENCES [dbo].[tbl_status] ([status_id])
GO
ALTER TABLE [dbo].[tbl_storage] CHECK CONSTRAINT [tbl_storage_fk_status_id_foreign]
GO
USE [master]
GO
ALTER DATABASE [HTQLVTYT] SET  READ_WRITE 
GO
