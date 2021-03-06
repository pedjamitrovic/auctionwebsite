USE [master]
GO
/****** Object:  Database [AuctionWebsiteDB]    Script Date: 31-Aug-18 18:05:57 ******/
CREATE DATABASE [AuctionWebsiteDB]
GO
ALTER DATABASE [AuctionWebsiteDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AuctionWebsiteDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AuctionWebsiteDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AuctionWebsiteDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AuctionWebsiteDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [AuctionWebsiteDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET  MULTI_USER 
GO
ALTER DATABASE [AuctionWebsiteDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AuctionWebsiteDB] SET ENCRYPTION ON
GO
ALTER DATABASE [AuctionWebsiteDB] SET QUERY_STORE = OFF
GO
USE [AuctionWebsiteDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET DISABLE_BATCH_MODE_ADAPTIVE_JOINS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET DISABLE_BATCH_MODE_MEMORY_GRANT_FEEDBACK = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET DISABLE_INTERLEAVED_EXECUTION_TVF = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ELEVATE_ONLINE = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ELEVATE_RESUMABLE = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ISOLATE_SECURITY_POLICY_CARDINALITY = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET OPTIMIZE_FOR_AD_HOC_WORKLOADS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET XTP_PROCEDURE_EXECUTION_STATISTICS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET XTP_QUERY_EXECUTION_STATISTICS = OFF;
GO
USE [AuctionWebsiteDB]
GO
/****** Object:  Table [dbo].[Auction]    Script Date: 31-Aug-18 18:05:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auction](
	[ID] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Duration] [int] NOT NULL,
	[Currency] [nvarchar](50) NOT NULL,
	[TokenValue] [decimal](16, 4) NOT NULL,
	[StartingPrice] [decimal](16, 4) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[OpenedOn] [datetime] NULL,
	[ClosedOn] [datetime] NULL,
	[Owner] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Auction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bid]    Script Date: 31-Aug-18 18:05:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bid](
	[ID] [uniqueidentifier] NOT NULL,
	[Bidder] [uniqueidentifier] NOT NULL,
	[OnAuction] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[TokenAmount] [decimal](16, 4) NOT NULL,
 CONSTRAINT [PK_Bid] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemParameters]    Script Date: 31-Aug-18 18:06:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemParameters](
	[ID] [uniqueidentifier] NOT NULL,
	[AuctionCount] [int] NOT NULL,
	[AuctionDuration] [int] NOT NULL,
	[SilverCount] [decimal](16, 4) NOT NULL,
	[GoldCount] [decimal](16, 4) NOT NULL,
	[PlatinumCount] [decimal](16, 4) NOT NULL,
	[Currency] [nvarchar](50) NOT NULL,
	[TokenValue] [decimal](16, 4) NOT NULL,
 CONSTRAINT [PK_SystemParameters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TokenOrder]    Script Date: 31-Aug-18 18:06:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TokenOrder](
	[ID] [uniqueidentifier] NOT NULL,
	[Buyer] [uniqueidentifier] NOT NULL,
	[TokenAmount] [decimal](16, 4) NOT NULL,
	[TokenValue] [decimal](16, 4) NOT NULL,
	[Currency] [nvarchar](50) NOT NULL,
	[State] [int] NOT NULL,
 CONSTRAINT [PK_TokenOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 31-Aug-18 18:06:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[TokenAmount] [decimal](16, 4) NOT NULL,
	[Administrator] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Auction] ([ID], [Title], [Duration], [Currency], [TokenValue], [StartingPrice], [CreatedOn], [OpenedOn], [ClosedOn], [Owner]) VALUES (N'685f3f59-692f-4e0a-a48a-23ce17054bb5', N'Laptop', 30, N'RSD', CAST(0.7000 AS Decimal(16, 4)), CAST(100.0000 AS Decimal(16, 4)), CAST(N'2018-08-30T11:40:03.437' AS DateTime), CAST(N'2018-08-30T11:42:45.780' AS DateTime), CAST(N'2018-08-30T11:43:18.323' AS DateTime), N'1650f957-c691-e811-86db-9822ef756dc4')
INSERT [dbo].[Auction] ([ID], [Title], [Duration], [Currency], [TokenValue], [StartingPrice], [CreatedOn], [OpenedOn], [ClosedOn], [Owner]) VALUES (N'36c45816-dff8-446f-bf5b-5aefcd61e309', N'Monitor', 999999, N'RSD', CAST(0.7000 AS Decimal(16, 4)), CAST(10.0000 AS Decimal(16, 4)), CAST(N'2018-08-30T11:47:20.190' AS DateTime), CAST(N'2018-08-30T11:47:45.977' AS DateTime), NULL, N'4eecc7f0-0cd0-4a8b-bf0b-c6695af814a5')
INSERT [dbo].[Bid] ([ID], [Bidder], [OnAuction], [CreatedOn], [TokenAmount]) VALUES (N'0352c7b8-4ff5-4473-9762-a444d8aba394', N'4eecc7f0-0cd0-4a8b-bf0b-c6695af814a5', N'685f3f59-692f-4e0a-a48a-23ce17054bb5', CAST(N'2018-08-30T11:42:57.270' AS DateTime), CAST(101.0000 AS Decimal(16, 4)))
INSERT [dbo].[SystemParameters] ([ID], [AuctionCount], [AuctionDuration], [SilverCount], [GoldCount], [PlatinumCount], [Currency], [TokenValue]) VALUES (N'adaa4a91-0398-e811-86db-9822ef756dc4', 10, 3600, CAST(10.0000 AS Decimal(16, 4)), CAST(11.0000 AS Decimal(16, 4)), CAST(12.0000 AS Decimal(16, 4)), N'RSD', CAST(0.7000 AS Decimal(16, 4)))
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [Email], [Password], [TokenAmount], [Administrator]) VALUES (N'1650f957-c691-e811-86db-9822ef756dc4', N'Predrag', N'Mitrovic', N'pre11mit@yahoo.com', N'592e495150aa6e8bd75d2c5433fddfa1', CAST(20142.0000 AS Decimal(16, 4)), 1)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [Email], [Password], [TokenAmount], [Administrator]) VALUES (N'4eecc7f0-0cd0-4a8b-bf0b-c6695af814a5', N'Probni', N'Korisnik', N'proba@gmail.com', N'592e495150aa6e8bd75d2c5433fddfa1', CAST(19899.0000 AS Decimal(16, 4)), 0)
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF_Auction_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF_Auction_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Bid] ADD  CONSTRAINT [DF_Bid_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[SystemParameters] ADD  CONSTRAINT [DF_System_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[TokenOrder] ADD  CONSTRAINT [DF_TokenOrder_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[TokenOrder] ADD  CONSTRAINT [DF_TokenOrder_State]  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_ID]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_TokenCount]  DEFAULT ((0)) FOR [TokenAmount]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_Administrator]  DEFAULT ((0)) FOR [Administrator]
GO
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK_Auction_User] FOREIGN KEY([Owner])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK_Auction_User]
GO
ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_Bid_Auction] FOREIGN KEY([OnAuction])
REFERENCES [dbo].[Auction] ([ID])
GO
ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_Bid_Auction]
GO
ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_Bid_User] FOREIGN KEY([Bidder])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_Bid_User]
GO
ALTER TABLE [dbo].[TokenOrder]  WITH CHECK ADD  CONSTRAINT [FK_TokenOrder_User] FOREIGN KEY([Buyer])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[TokenOrder] CHECK CONSTRAINT [FK_TokenOrder_User]
GO
USE [master]
GO
ALTER DATABASE [AuctionWebsiteDB] SET  READ_WRITE 
GO
