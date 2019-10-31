
ALTER TABLE [dbo].[Q_HisUserEvaluate] ADD [Comment] [nvarchar](max) NULL  
GO
ALTER TABLE [dbo].[Q_UserEvaluate] ADD [Comment] [nvarchar](max) NULL  
GO
 
alter table [dbo].[Q_Config] alter column [Value] nvarchar(max) NULL
GO
INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('TicketTemplate','',N'Mẫu vé',1,0)
GO

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('NumberOfLinePerTime','1',N'so liên in phiếu mỗi lần',1,0)
GO
 