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

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('AutoCallFollowMajorOrder','0',N'AutoCall lấy theo thứ tự ưu tiên nghiệp vụ',1,0)
GO   

INSERT INTO [dbo].[Q_ActionParameter] ([ActionId] ,[ParameterCode]  ,[Note]  ,[IsDeleted])
     VALUES (5 ,'HUY_DKY_LAYSO' ,N'Hủy đăng ký lấy số tự động' ,0)
GO  

ALTER TABLE [dbo].[Q_HisUserEvaluate] ADD [CreatedDate] datetime  DEFAULT ((getdate())) 
GO

--update lai lich su danh gia ko co createdDAte
update Q_HisUserEvaluate set CreatedDate = (select top 1 EndProcessTime from Q_HisDailyRequire_De where Id = HisDailyRequireDeId)

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('SilenceTime','0',N'khoảng lặng giữa mỗi file âm thanh',1,0)
GO   
INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('PrintTicketReturnCurrentNumberOrServiceCode','1',N'Sau khi cấp phiếu trả về số đang gọi hay mã dịch vụ ? 1 -> số đang gọi, 2 -> mã dịch vụ',1,0)
GO   