/*tính năng đọc thông báo thời gian phục vụ sắp hết*/

alter table q_dailyrequire_detail add ServeOverCounter int not null default(0)
alter table q_hisdailyrequire_de add ServeOverCounter int not null default(0)
  
INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('8cUseFor','1',N'lệnh 8c sử dụng => 1.login_logout | 2.đổi TG phục vụ',1,0)
GO

alter table [dbo].[Q_RecieverSMS] add [UserIds] varchar(50) null


/****** Object:  Table [dbo].[Q_RequestTicket]    Script Date: 19/07/2019 3:07:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_RequestTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Q_RequireTicket] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_RequestTicket] ADD  CONSTRAINT [DF_Q_RequireTicket_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Q_RequestTicket] ADD  CONSTRAINT [DF_Q_RequestTicket_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
