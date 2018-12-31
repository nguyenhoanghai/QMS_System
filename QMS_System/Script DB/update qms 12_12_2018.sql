 
/****** Object:  Table [dbo].[Q_Video]    Script Date: 12/14/2018 4:40:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_Video](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[FakeName] [nvarchar](500) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_Video] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_Video] ADD  CONSTRAINT [DF_Q_Video_IsDeleted_1]  DEFAULT ((0)) FOR [IsDeleted]
GO

/****** Object:  Table [dbo].[Q_VideoTemplate]    Script Date: 12/14/2018 4:30:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_VideoTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [nvarchar](500) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_VideoTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_VideoTemplate] ADD  CONSTRAINT [DF_Q_VideoTemplate_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Q_VideoTemplate] ADD  CONSTRAINT [DF_Q_VideoTemplate_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


/****** Object:  Table [dbo].[Q_VideoTemplate_De]    Script Date: 12/14/2018 4:30:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Q_VideoTemplate_De](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Index] [int] NOT NULL,
	[TemplateId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Q_VideoTemplate_De] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Q_VideoTemplate_De] ADD  CONSTRAINT [DF_Table_1_IsActive]  DEFAULT ((1)) FOR [Index]
GO

ALTER TABLE [dbo].[Q_VideoTemplate_De] ADD  CONSTRAINT [DF_Q_VideoTemplate_De_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
