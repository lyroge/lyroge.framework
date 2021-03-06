USE [BBSv0912]
GO
/****** 对象:  Table [dbo].[tb_ChatRecord]    脚本日期: 01/30/2013 17:13:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_ChatRecord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[UserName] [varchar](128) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[KfName] [varchar](128) COLLATE Chinese_PRC_CI_AS NULL,
	[Content] [varchar](8000) COLLATE Chinese_PRC_CI_AS NULL,
	[OffLineTime] [datetime] NOT NULL CONSTRAINT [DF_tb_ChatRecord_OffLineTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_tb_ChatRecord] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF