 /****** Object:  Table [dbo].[Q_WorkType]    Script Date: 27/02/2020 9:49:46 SA ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_WorkType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](500) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_WorkType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_WorkType] ADD  CONSTRAINT [DF_Q_WorkType_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

/****** Object:  Table [dbo].[Q_Works]    Script Date: 27/02/2020 9:49:51 SA ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_Works](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](500) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_ServiceDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_Works] ADD  CONSTRAINT [DF_Q_ServiceDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

/****** Object:  Table [dbo].[Q_WorkDetail]    Script Date: 27/02/2020 9:49:55 SA ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_WorkDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkId] [int] NOT NULL,
	[TimeProcess] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[WorkTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Q_WorkDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_WorkDetail] ADD  CONSTRAINT [DF_Q_WorkDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Q_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_WorkDetail_Q_Works] FOREIGN KEY([WorkId])
REFERENCES [dbo].[Q_Works] ([Id])
GO

ALTER TABLE [dbo].[Q_WorkDetail] CHECK CONSTRAINT [FK_Q_WorkDetail_Q_Works]
GO

ALTER TABLE [dbo].[Q_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_WorkDetail_Q_WorkType] FOREIGN KEY([WorkTypeId])
REFERENCES [dbo].[Q_WorkType] ([Id])
GO

ALTER TABLE [dbo].[Q_WorkDetail] CHECK CONSTRAINT [FK_Q_WorkDetail_Q_WorkType]
GO
/****** Object:  Table [dbo].[Q_DailyRequire_WorkDetail]    Script Date: 02/03/2020 6:02:18 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_DailyRequire_WorkDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DailyRequireId] [int] NOT NULL,
	[WorkDetailId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_DailyRequireJob] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail] ADD  CONSTRAINT [DF_Q_Q_DailyRequire_WorkDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_DailyRequire_WorkDetail_Q_DailyRequire] FOREIGN KEY([DailyRequireId])
REFERENCES [dbo].[Q_DailyRequire] ([Id])
GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail] CHECK CONSTRAINT [FK_Q_DailyRequire_WorkDetail_Q_DailyRequire]
GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_DailyRequire_WorkDetail_Q_ServiceDetail] FOREIGN KEY([WorkDetailId])
REFERENCES [dbo].[Q_Works] ([Id])
GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail] CHECK CONSTRAINT [FK_Q_DailyRequire_WorkDetail_Q_ServiceDetail]
GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_DailyRequire_WorkDetail_Q_WorkDetails] FOREIGN KEY([WorkDetailId])
REFERENCES [dbo].[Q_WorkDetail] ([Id])
GO

ALTER TABLE [dbo].[Q_DailyRequire_WorkDetail] CHECK CONSTRAINT [FK_Q_DailyRequire_WorkDetail_Q_WorkDetails]
GO

/****** Object:  Table [dbo].[Q_HisDailyRequire_WorkDetail]    Script Date: 02/03/2020 6:02:22 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_HisDailyRequire_WorkDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HisDailyRequireId] [int] NOT NULL,
	[WorkDetailId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_HisDailyRequireJob] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail] ADD  CONSTRAINT [DF_Q_HisDailyRequire_WorkDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_HisDailyRequire_WorkDetail_Q_HisDailyRequire] FOREIGN KEY([HisDailyRequireId])
REFERENCES [dbo].[Q_HisDailyRequire] ([Id])
GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail] CHECK CONSTRAINT [FK_Q_HisDailyRequire_WorkDetail_Q_HisDailyRequire]
GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_HisDailyRequire_WorkDetail_Q_ServiceDetail] FOREIGN KEY([WorkDetailId])
REFERENCES [dbo].[Q_Works] ([Id])
GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail] CHECK CONSTRAINT [FK_Q_HisDailyRequire_WorkDetail_Q_ServiceDetail]
GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail]  WITH CHECK ADD  CONSTRAINT [FK_Q_HisDailyRequire_WorkDetail_Q_WorkDetails] FOREIGN KEY([WorkDetailId])
REFERENCES [dbo].[Q_WorkDetail] ([Id])
GO

ALTER TABLE [dbo].[Q_HisDailyRequire_WorkDetail] CHECK CONSTRAINT [FK_Q_HisDailyRequire_WorkDetail_Q_WorkDetails]
GO

/****** Object:  Table [dbo].[Q_CounterDayInfo]    Script Date: 10/02/2020 8:33:27 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_CounterDayInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CounterId] [int] NOT NULL,
	[STT_QMS] [int] NOT NULL,
	[STT_3] [nvarchar](500) NOT NULL,
	[STT] [int] NOT NULL,
	[STT_UT] [int] NOT NULL,
	[PrintTime] [datetime] NULL,
	[StartTime] [datetime] NULL,
	[ServeTime] [datetime] NULL,
	[StatusSTT] [int] NOT NULL,
	[StatusSTT_UT] [int] NOT NULL,
	[EquipCode] [int] NOT NULL,
 CONSTRAINT [PK_Q_CounterDayInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_STT_QMS]  DEFAULT ((0)) FOR [STT_QMS]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_STT1]  DEFAULT ((0)) FOR [STT_3]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_STT]  DEFAULT ((0)) FOR [STT]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_STT_UT]  DEFAULT ((0)) FOR [STT_UT]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_StatusSTT]  DEFAULT ((0)) FOR [StatusSTT]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_StatusSTT_UT]  DEFAULT ((0)) FOR [StatusSTT_UT]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] ADD  CONSTRAINT [DF_Q_CounterDayInfo_EquipCode]  DEFAULT ((0)) FOR [EquipCode]
GO

ALTER TABLE [dbo].[Q_CounterDayInfo]  WITH CHECK ADD  CONSTRAINT [FK_Q_CounterDayInfo_Q_Counter] FOREIGN KEY([CounterId])
REFERENCES [dbo].[Q_Counter] ([Id])
GO

ALTER TABLE [dbo].[Q_CounterDayInfo] CHECK CONSTRAINT [FK_Q_CounterDayInfo_Q_Counter]
GO

-- Update Q_Video table
ALTER TABLE [Q_Video] ADD  [Duration] time(7) default('00:00:00') not null


-- 3/8/2020
-- Update q_service table 
-- dung cho huu nghi
alter table [Q_Service] add [ServiceType] int not null default(1)
alter table [Q_DailyRequire] add [Type] int not null default(1)
alter table [Q_DailyRequire] add [TGDKien] datetime   null  
alter table [Q_HisDailyRequire] add [Type] int not null default(1)
alter table [Q_HisDailyRequire] add [TGDKien] datetime   null  

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('StartWork','07:00:00',N'Thời gian bắt đầu làm việc',1,0)
GO

--25/12/2020
-- tien thu
alter table [Q_CounterSoftRequire] add [CreatedDate] datetime  not null default(getDate())

/****** Object:  Table [dbo].[Q_PrintTicket]    Script Date: 25/12/2020 10:19:58 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_PrintTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[PrintTemplate] [nvarchar](max) NOT NULL,
	[PrintIndex] [int] NOT NULL,
	[PrintPages] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Q_PrintTicket] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_PrintTicket] ADD  CONSTRAINT [DF_Q_PrintTicket_PrintPages]  DEFAULT ((1)) FOR [PrintPages]
GO

ALTER TABLE [dbo].[Q_PrintTicket] ADD  CONSTRAINT [DF_Q_PrintTicket_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
