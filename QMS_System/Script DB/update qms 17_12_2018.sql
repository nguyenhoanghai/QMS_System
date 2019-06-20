/*
version 3.0.0.0
cập nhật tính năng mới
1. API cấp phiếu từ bên thứ 3
2. API cập nhật thông tin phiếu từ bên thứ 3
3. thêm mã phongkham vào table Service
3. thêm STT_PhongKham & MaBenhNhan & MaPhongKham vào table DailyRequired
*/

alter table [dbo].[Q_Service] add [Code] nvarchar(250) null
GO

alter table [dbo].[Q_DailyRequire] add [CustomerName] nvarchar(250) null
GO
alter table [dbo].[Q_DailyRequire] add [CustomerDOB] nvarchar(250) null
GO
alter table [dbo].[Q_DailyRequire] add [CustomerAddress] nvarchar(250) null
GO

alter table [dbo].[Q_DailyRequire] add [STT_PhongKham] nvarchar(250) null
GO
alter table [dbo].[Q_DailyRequire] add [MaBenhNhan] nvarchar(250) null
GO
alter table [dbo].[Q_DailyRequire] add [MaPhongKham] nvarchar(250) null
GO

alter table [dbo].[Q_HisDailyRequire] add [CustomerName] nvarchar(250) null
GO
alter table [dbo].[Q_HisDailyRequire] add [CustomerDOB] nvarchar(250) null
GO
alter table [dbo].[Q_HisDailyRequire] add [CustomerAddress] nvarchar(250) null
GO
alter table [dbo].[Q_HisDailyRequire] add [STT_PhongKham] nvarchar(250) null
GO
alter table [dbo].[Q_HisDailyRequire] add [MaBenhNhan] nvarchar(250) null
GO
alter table [dbo].[Q_HisDailyRequire] add [MaPhongKham] nvarchar(250) null
GO

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('UseWithThirdPattern','0','Su Dung Chung Ben Thu 3',1,0)
GO

alter table [dbo].[Q_DailyRequire_Detail] add [IsSendSMS] bit not null default(0)
GO
alter table [dbo].[Q_HisDailyRequire_De] add [IsSendSMS] bit not null default(0)
GO

alter table [dbo].[Q_DailyRequire_Detail] add [SmsContent] nvarchar(1000) null
GO
alter table [dbo].[Q_HisDailyRequire_De] add [SmsContent] nvarchar(250) null
GO

INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('CheckTimeBeforePrintTicket','0','Kiểm tra thời gian ca làm việc trước khi cấp phiếu',1,0)
GO
INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('SoundLockPrintTicket','khoa.wav','file âm thanh thông báo ngưng cấp phiếu',1,0)
GO 