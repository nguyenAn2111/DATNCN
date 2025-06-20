USE [master]
GO
/****** Object:  Database [HTQLTBYT]    Script Date: 12/06/2025 1:05:52 SA ******/
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
/****** Object:  Table [dbo].[tbl_account]    Script Date: 12/06/2025 1:05:52 SA ******/
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
/****** Object:  Table [dbo].[tbl_contact]    Script Date: 12/06/2025 1:05:52 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_contact](
	[contact_id] [varchar](255) NOT NULL,
	[contact_type] [nvarchar](50) NOT NULL,
	[contact_address] [nvarchar](50) NOT NULL,
	[contact_finance] [bigint] NOT NULL,
 CONSTRAINT [tbl_contact_contact_id_primary] PRIMARY KEY CLUSTERED 
(
	[contact_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_device]    Script Date: 12/06/2025 1:05:52 SA ******/
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
/****** Object:  Table [dbo].[tbl_maintain]    Script Date: 12/06/2025 1:05:52 SA ******/
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
/****** Object:  Table [dbo].[tbl_repair]    Script Date: 12/06/2025 1:05:52 SA ******/
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
 CONSTRAINT [tbl_repair_repair_id_primary] PRIMARY KEY CLUSTERED 
(
	[repair_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_room]    Script Date: 12/06/2025 1:05:52 SA ******/
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
/****** Object:  Table [dbo].[tbl_status]    Script Date: 12/06/2025 1:05:52 SA ******/
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
/****** Object:  Table [dbo].[tbl_storage]    Script Date: 12/06/2025 1:05:52 SA ******/
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
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'leminhha', N'1')
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'user1', N'password123')
INSERT [dbo].[tbl_account] ([account_username], [account_password]) VALUES (N'user2', N'password456')
GO
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'0', N'0', N'le_minh_ha_-_20213894_-_bao_cao_đo_an_1_1.9m.pdf', 1000000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'1', N'1', N'123 Main St', 5000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'2', N'2', N'456 Elm St', 3000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'27', N'Mua bán', N'doan1_3.sql', 1500000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'3', N'0', N'SQLQuery1.sql', 1000000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'38', N'Mua bán', N'2024.1-bvtn-ds-hoi-dong.pdf', 2000000000)
INSERT [dbo].[tbl_contact] ([contact_id], [contact_type], [contact_address], [contact_finance]) VALUES (N'39', N'Mua bán', N'thiet_bi_ton_kho.xlsx', 1500000000)
GO
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'CT-GEHe-5623', N'Máy chụp CT GE', N'GE Healthcare', N'78945623', N'CT', N'Chẩn đoán', 12, CAST(N'2025-07-09' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-09' AS Date), N'1', N'38', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'CT-GEHe-5679', N'Máy chụp CT GE', N'GE Healthcare', N'12345679', N'CT', N'Chẩn đoán', 12, CAST(N'2025-07-09' AS Date), NULL, N'Mua mới', CAST(N'2025-06-09' AS Date), N'1', N'38', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'MRI-GEHe-1262', N'Máy chụp CHT MRI 3.0 T', N'GE Healthcare', N'20031262', N'MRI', N'Chẩn đoán', 12, CAST(N'2025-07-09' AS Date), CAST(N'1900-01-01' AS Date), N'Mua mới', CAST(N'2025-06-10' AS Date), N'1', N'39', N'20', N'D-101')
INSERT [dbo].[tbl_device] ([device_id], [device_name], [device_manufacturer], [device_seri], [device_type], [device_group], [device_maintenance_cycle], [device_maintenance_start], [device_stockout_date], [device_condition], [device_received_date], [device_note], [FK_contact_id], [FK_status_id], [FK_room_id]) VALUES (N'MRI-GEHe-6174', N'Máy chụp CHT MRI 3.0 T', N'GE Healthcare', N'20836174', N'MRI', N'Chẩn đoán', 12, CAST(N'2025-07-09' AS Date), NULL, N'Mua mới', CAST(N'2025-06-10' AS Date), N'1', N'39', N'20', N'D-101')
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
ALTER DATABASE [HTQLTBYT] SET  READ_WRITE 
GO
