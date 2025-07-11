USE [master]
GO
/****** Object:  Database [HTQLTBYT]    Script Date: 18/06/2025 2:59:16 SA ******/
CREATE DATABASE [HTQLTBYT]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HTQLTBYT', FILENAME = N'D:\Visual Studio\MSSQL16.MSSQLSERVER\MSSQL\DATA\HTQLTBYT.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HTQLTBYT_log', FILENAME = N'D:\Visual Studio\MSSQL16.MSSQLSERVER\MSSQL\DATA\HTQLTBYT_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [HTQLTBYT] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HTQLTBYT].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HTQLTBYT] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HTQLTBYT] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HTQLTBYT] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HTQLTBYT] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HTQLTBYT] SET ARITHABORT OFF 
GO
ALTER DATABASE [HTQLTBYT] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HTQLTBYT] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HTQLTBYT] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HTQLTBYT] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HTQLTBYT] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HTQLTBYT] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HTQLTBYT] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HTQLTBYT] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HTQLTBYT] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HTQLTBYT] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HTQLTBYT] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HTQLTBYT] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HTQLTBYT] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HTQLTBYT] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HTQLTBYT] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HTQLTBYT] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HTQLTBYT] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HTQLTBYT] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HTQLTBYT] SET  MULTI_USER 
GO
ALTER DATABASE [HTQLTBYT] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HTQLTBYT] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HTQLTBYT] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HTQLTBYT] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HTQLTBYT] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HTQLTBYT] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'HTQLTBYT', N'ON'
GO
ALTER DATABASE [HTQLTBYT] SET QUERY_STORE = ON
GO
ALTER DATABASE [HTQLTBYT] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [HTQLTBYT]
GO
/****** Object:  Table [dbo].[tbl_account]    Script Date: 18/06/2025 2:59:17 SA ******/
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
/****** Object:  Table [dbo].[tbl_contact]    Script Date: 18/06/2025 2:59:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_contact](
	[contact_id] [varchar](255) NOT NULL,
	[contact_type] [int] NOT NULL,
	[contact_address] [nvarchar](50) NOT NULL,
	[contact_finance] [bigint] NOT NULL,
 CONSTRAINT [tbl_contact_contact_id_primary] PRIMARY KEY CLUSTERED 
(
	[contact_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_device]    Script Date: 18/06/2025 2:59:17 SA ******/
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
	[device_stockout_date] [date] NULL,
	[device_condition] [nvarchar](50) NOT NULL,
	[device_received_date] [date] NOT NULL,
	[device_note] [nvarchar](50) NOT NULL,
	[FK_contact_id] [varchar](255) NOT NULL,
	[FK_status_id] [varchar](255) NOT NULL,
	[FK_room_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_device_device_id_primary] PRIMARY KEY CLUSTERED 
(
	[device_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_maintain]    Script Date: 18/06/2025 2:59:17 SA ******/
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
	[FK_contact_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_maintain_maintain_id_primary] PRIMARY KEY CLUSTERED 
(
	[maintain_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_repair]    Script Date: 18/06/2025 2:59:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_repair](
	[repair_id] [varchar](255) NOT NULL,
	[repair_broken] [date] NOT NULL,
	[repair_priority] [int] NOT NULL,
	[repair_date] [date] NOT NULL,
	[repair_update_date] [date] NOT NULL,
	[repair_update_status] [nvarchar](max) NOT NULL,
	[repair_note] [text] NOT NULL,
	[repair_picture] [varchar](max) NULL,
	[repair_update_note] [text] NOT NULL,
	[FK_contact_id] [varchar](255) NOT NULL,
	[FK_room_id] [varchar](255) NOT NULL,
	[FK_device_id] [varchar](255) NOT NULL,
	[FK_status_id] [varchar](255) NOT NULL,
 CONSTRAINT [tbl_repair_repair_id_primary] PRIMARY KEY CLUSTERED 
(
	[repair_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_room]    Script Date: 18/06/2025 2:59:17 SA ******/
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
/****** Object:  Table [dbo].[tbl_status]    Script Date: 18/06/2025 2:59:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_status](
	[status_id] [varchar](255) NOT NULL,
	[status_name] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_status_status_id_primary] PRIMARY KEY CLUSTERED 
(
	[status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_storage]    Script Date: 18/06/2025 2:59:17 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_storage](
	[storage_id] [int] IDENTITY(1,1) NOT NULL,
	[storage_date] [datetime] NOT NULL,
	[FK_device_id] [varchar](255) NOT NULL,
	[FK_room_id_from] [varchar](255) NOT NULL,
	[FK_room_id_to] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[storage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'leminhha', N'1')
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'nguyenan', N'21')
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'user1', N'password123')
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'user2', N'password456')
GO
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'1', 1, N'le_minh_ha_-_20213894_-_bao_cao_đo_an_1_1.9m.pdf', 2000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'13', 3, N'2024.1-bvtn-ds-hoi-dong.pd', 1200000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'16', 3, N'báo_cáo_2.2.pdf', 2500000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'17', 1, N'báo_cáo_2.3.pdf', 21000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'2', 1, N'Nháp - Tech.docx', 5000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'3', 1, N'2024.1-bvtn-ds-hoi-dong.pdf', 15000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'4', 1, N'báo_cáo _1.pdf', 6000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'7', 1, N'Nháp - Tech.docx', 7000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'8', 2, N'báo_cáo_2.2.pdf', 1000000)
GO
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'BIO-Siem-1266', N'Máy xét nghiệm sinh hóa', N'Siemens Healthineers', N'20031266', N'BIO', N'Điều trị', 10, CAST(N'2025-07-07' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-07' AS Date), N'1', N'7', N'03', N'KHO')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'CT-GEHe-5678', N'Máy chụp CT GE', N'GE Healthcare', N'12345678', N'CT', N'Chẩn đoán', 12, CAST(N'2025-07-09' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-08' AS Date), N'1', N'1', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'CT-GEHe-5679', N'Máy chụp CT GE', N'GE Healthcare', N'12345679', N'CT', N'Chẩn đoán', 12, CAST(N'2025-07-09' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-09' AS Date), N'1', N'1', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'HEMO-Siem-8702', N'Máy xét nghiệm huyết học', N'Siemens Healthineers', N'33958702', N'HEMO', N'Xét nghiệm', 10, CAST(N'2025-05-03' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-05-02' AS Date), N'Máy xét nghiệm huyết học của Siemens Healthineers', N'4', N'13', N'KHO')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'MRI-GEHe-3526', N'Máy chụp CHT MRI 3.0 T', N'GE Healthcare', N'78943526', N'MRI', N'Chẩn đoán', 12, CAST(N'2025-07-12' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-10' AS Date), N'test', N'3', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'MRI-GEHe-5623', N'Máy chụp CHT MRI 3.0 T', N'GE Healthcare', N'78945623', N'MRI', N'Chẩn đoán', 12, CAST(N'2025-07-12' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-11' AS Date), N'test', N'3', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'O2-Inog-3210', N'Máy tạo oxy di động', N' Inogen', N'76543210', N'O2', N'Hỗ trợ bệnh nhân', 20, CAST(N'2026-03-21' AS Date), CAST(N'1900-01-01' AS Date), N'Mua cũ', CAST(N'2025-04-21' AS Date), N'Máy tạo oxy di động Inogen One G5', N'17', N'20', N'T1-02')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'VENT-Phil-6174', N'Máy thở Philips', N'Philips healthcare', N'20836174', N'VENT', N'Điều trị', 10, CAST(N'2025-07-12' AS Date), NULL, N'Mua mới', CAST(N'2025-07-07' AS Date), N'2', N'2', N'20', N'E-101')
GO
INSERT [dbo].[tbl_maintain] ([maintain_id], [maintain_date], [maintain_maintenance], [maintain_maintenance_phone], [maintain_delivery], [maintain_delivery_phone], [FK_device_id], [FK_status_id], [FK_room_id], [FK_contact_id]) VALUES (N'1', CAST(N'2025-06-17' AS Date), N'lê h', N'321654987', N'nGUYỄN A', N'123456789', N'BIO-Siem-1266', N'03', N'KHO', N'8')
GO
INSERT [dbo].[tbl_repair] ([repair_id], [repair_broken], [repair_priority], [repair_date], [repair_update_date], [repair_update_status], [repair_note], [repair_picture], [repair_update_note], [FK_contact_id], [FK_room_id], [FK_device_id], [FK_status_id]) VALUES (N'1', CAST(N'2025-06-10' AS Date), 1, CAST(N'2025-06-12' AS Date), CAST(N'2025-06-17' AS Date), N'13', N'test', N'không ', N'Ðã thêm thông tin c?a thi?t b? c?n s?a ch?a', N'13', N'KHO', N'HEMO-Siem-8702', N'13')
GO
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'A-101', N'Xét nghiệm', N'A-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'A-201', N'Phòng khám', N'A-201')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'B-101', N'Phòng mổ', N'B-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'C-101', N'Khoa nội', N'C-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'D-101', N'Chẩn đoán hình ảnh', N'D-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'E-101', N'Hồi sức tích cực', N'E-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'E-201', N'Khoa nhi', N'E-201')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'H-101', N'Kiểm soát nhiễm khuẩn', N'H-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'KHO', N'Kho cất đồ', N'Kho cất đồ')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'P-101', N'Dược', N'P-101')
INSERT [dbo].[tbl_room] ([room_id], [room_type], [room_name]) VALUES (N'T1-02', N'Cấp cứu', N'P102')
GO
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'00', N'Chưa xếp lịch')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'01', N'Đã xếp lịch bảo trì')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'02', N'Đang bảo trì')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'03', N'Đã bảo trì xong')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'10', N'Yêu cầu sửa chữa')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'11', N'Đã xếp lịch sửa chữa')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'12', N'Đang sửa chữa')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'13', N'Đã sửa xong')
INSERT [dbo].[tbl_status] ([status_id], [status_name]) VALUES (N'20', N'Đang sử dụng')
GO
ALTER TABLE [dbo].[tbl_storage] ADD  DEFAULT (getdate()) FOR [storage_date]
GO
ALTER TABLE [dbo].[tbl_device]  WITH CHECK ADD  CONSTRAINT [tbl_device_fk_contact_id_foreign] FOREIGN KEY([FK_contact_id])
REFERENCES [dbo].[tbl_contact] ([contact_id])
GO
ALTER TABLE [dbo].[tbl_device] CHECK CONSTRAINT [tbl_device_fk_contact_id_foreign]
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
ALTER TABLE [dbo].[tbl_storage]  WITH CHECK ADD FOREIGN KEY([FK_device_id])
REFERENCES [dbo].[tbl_device] ([device_id])
GO
ALTER TABLE [dbo].[tbl_storage]  WITH CHECK ADD FOREIGN KEY([FK_room_id_from])
REFERENCES [dbo].[tbl_room] ([room_id])
GO
ALTER TABLE [dbo].[tbl_storage]  WITH CHECK ADD FOREIGN KEY([FK_room_id_to])
REFERENCES [dbo].[tbl_room] ([room_id])
GO
USE [master]
GO
ALTER DATABASE [HTQLTBYT] SET  READ_WRITE 
GO
