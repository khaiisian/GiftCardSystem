-- Schema script for database [GiftCardSystem]
-- Generated 2026-09-25 14:33 from OTGEXPERTBOOKB5\SQLEXPRESS
IF DB_ID(N'GiftCardSystem') IS NULL CREATE DATABASE [GiftCardSystem];
GO
USE [GiftCardSystem];
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftCard]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GiftCard](
	[GiftCardId] [int] IDENTITY(1,1) NOT NULL,
	[GiftCardNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Title] [varchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ExpiryDate] [datetime2](7) NOT NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[MaxPurchaseLimit] [int] NULL,
	[GiftPerUserLimit] [int] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[GiftCardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_GiftCard_GiftCardNo] UNIQUE NONCLUSTERED 
(
	[GiftCardNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_GiftCard_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftCard] ADD  CONSTRAINT [DF_GiftCard_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftCardPaymentMethod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GiftCardPaymentMethod](
	[GiftCardPaymentMethodId] [int] IDENTITY(1,1) NOT NULL,
	[GiftCardId] [int] NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[Discount] [decimal](5, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[GiftCardPaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_GCPM_Card_Method] UNIQUE NONCLUSTERED 
(
	[GiftCardId] ASC,
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_GCPM_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftCardPaymentMethod] ADD  CONSTRAINT [DF_GCPM_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PaymentMethod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PaymentMethod](
	[PaymentMethodId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_PaymentMethod_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_PaymentMethod_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PaymentMethod] ADD  CONSTRAINT [DF_PaymentMethod_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ticket]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ticket](
	[TicketId] [bigint] IDENTITY(1,1) NOT NULL,
	[GiftCardId] [int] NOT NULL,
	[PromoCode] [char](11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[QrPath] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Status] [tinyint] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Ticket_PromoCode] UNIQUE NONCLUSTERED 
(
	[PromoCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Ticket_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ticket] ADD  CONSTRAINT [DF_Ticket_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Transaction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionNo] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[GiftCardId] [int] NOT NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[TransactionDate] [datetime2](7) NOT NULL,
	[TypeOfBuying] [tinyint] NOT NULL,
	[RecipientName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RecipientPhone] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Transaction_No] UNIQUE NONCLUSTERED 
(
	[TransactionNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Transaction_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TransactionDetails](
	[TransactionDetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[TicketId] [bigint] NOT NULL,
	[UnitPrice] [decimal](10, 2) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_TxnDetail_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TransactionDetails] ADD  CONSTRAINT [DF_TxnDetail_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PhNumber] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [varchar](150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PasswordHash] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Role] [tinyint] NOT NULL,
	[RefreshToken] [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RefreshTokenExpiry] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Customer_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customer_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_Customer_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GCPM_GiftCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftCardPaymentMethod]'))
ALTER TABLE [dbo].[GiftCardPaymentMethod]  WITH CHECK ADD  CONSTRAINT [FK_GCPM_GiftCard] FOREIGN KEY([GiftCardId])
REFERENCES [dbo].[GiftCard] ([GiftCardId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GCPM_GiftCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftCardPaymentMethod]'))
ALTER TABLE [dbo].[GiftCardPaymentMethod] CHECK CONSTRAINT [FK_GCPM_GiftCard]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GCPM_PaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftCardPaymentMethod]'))
ALTER TABLE [dbo].[GiftCardPaymentMethod]  WITH CHECK ADD  CONSTRAINT [FK_GCPM_PaymentMethod] FOREIGN KEY([PaymentMethodId])
REFERENCES [dbo].[PaymentMethod] ([PaymentMethodId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GCPM_PaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftCardPaymentMethod]'))
ALTER TABLE [dbo].[GiftCardPaymentMethod] CHECK CONSTRAINT [FK_GCPM_PaymentMethod]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ticket_GiftCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ticket]'))
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_GiftCard] FOREIGN KEY([GiftCardId])
REFERENCES [dbo].[GiftCard] ([GiftCardId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ticket_GiftCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ticket]'))
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_Ticket_GiftCard]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transaction_Customer]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transaction]'))
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Customer] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transaction_Customer]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transaction]'))
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Customer]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transaction_GiftCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transaction]'))
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_GiftCard] FOREIGN KEY([GiftCardId])
REFERENCES [dbo].[GiftCard] ([GiftCardId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transaction_GiftCard]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transaction]'))
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_GiftCard]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transaction_PaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transaction]'))
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_PaymentMethod] FOREIGN KEY([PaymentMethodId])
REFERENCES [dbo].[PaymentMethod] ([PaymentMethodId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Transaction_PaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[Transaction]'))
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_PaymentMethod]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TxnDetail_Ticket]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransactionDetails]'))
ALTER TABLE [dbo].[TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_TxnDetail_Ticket] FOREIGN KEY([TicketId])
REFERENCES [dbo].[Ticket] ([TicketId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TxnDetail_Ticket]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransactionDetails]'))
ALTER TABLE [dbo].[TransactionDetails] CHECK CONSTRAINT [FK_TxnDetail_Ticket]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TxnDetail_Transaction]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransactionDetails]'))
ALTER TABLE [dbo].[TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_TxnDetail_Transaction] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([TransactionId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TxnDetail_Transaction]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransactionDetails]'))
ALTER TABLE [dbo].[TransactionDetails] CHECK CONSTRAINT [FK_TxnDetail_Transaction]
GO
