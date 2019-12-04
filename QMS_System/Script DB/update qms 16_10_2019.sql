
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

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('UsePrintBoard','1',N'sử dụng máy in theo board',1,0)
GO

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('COMPrint','COM',N'COM máy in không dùng board',1,0)
GO

alter table Q_service add [AutoEnd] bit default(0) not null
alter table Q_service add [TimeAutoEnd] datetime  null
alter table Q_service add [showBenhVien] bit default(0) not null
 
 INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('TVReadSound','0',N'TV đọc âm thanh',1,0)
GO


/****** Object:  Table [dbo].[Q_TVReadSound]    Script Date: 11/29/2019 10:42:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_TVReadSound](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sounds] [nvarchar](max) NULL,
	[UsersReaded] [nvarchar](250) NULL,
	[CounterId] [int] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_Q_TVReadSound] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_TVReadSound] ADD  CONSTRAINT [DF_Q_TVReadSound_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[Q_TVReadSound]  WITH CHECK ADD  CONSTRAINT [FK_Q_TVReadSound_Q_Counter] FOREIGN KEY([CounterId])
REFERENCES [dbo].[Q_Counter] ([Id])
GO

ALTER TABLE [dbo].[Q_TVReadSound] CHECK CONSTRAINT [FK_Q_TVReadSound_Q_Counter]
GO