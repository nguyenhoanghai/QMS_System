--insert feature
--check user's service limit in day

/****** Object:  Table [dbo].[Q_ServiceLimit]    Script Date: 06/08/2019 10:09:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Q_ServiceLimit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CurrentQuantity] [int] NOT NULL,
	[CurrentDay] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Q_ServiceLimit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Q_ServiceLimit] ADD  CONSTRAINT [DF_Q_ServiceLimit_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO

ALTER TABLE [dbo].[Q_ServiceLimit] ADD  CONSTRAINT [DF_Q_ServiceLimit_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Q_ServiceLimit] ADD  CONSTRAINT [DF_Q_ServiceLimit_Quantity1]  DEFAULT ((0)) FOR [CurrentQuantity]
GO

ALTER TABLE [dbo].[Q_ServiceLimit]  WITH CHECK ADD  CONSTRAINT [FK_Q_ServiceLimit_Q_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Q_Service] ([Id])
GO

ALTER TABLE [dbo].[Q_ServiceLimit] CHECK CONSTRAINT [FK_Q_ServiceLimit_Q_Service]
GO

ALTER TABLE [dbo].[Q_ServiceLimit]  WITH CHECK ADD  CONSTRAINT [FK_Q_ServiceLimit_Q_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Q_User] ([Id])
GO

ALTER TABLE [dbo].[Q_ServiceLimit] CHECK CONSTRAINT [FK_Q_ServiceLimit_Q_User]
GO

-- add config
INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('CheckServiceLimit','0',N'Kiểm tra giới hạn lấy phiếu của nhân viên trong ngày',1,0)
GO 
