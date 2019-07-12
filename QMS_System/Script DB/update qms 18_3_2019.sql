/*tính năng đọc thông báo thời gian phục vụ sắp hết*/

alter table q_dailyrequire_detail add ServeOverCounter int not null default(0)
alter table q_hisdailyrequire_de add ServeOverCounter int not null default(0)
  
INSERT INTO [dbo].[Q_Config]([Code],[Value],[Note],[IsActived],[IsDeleted])
     VALUES ('8cUseFor','1',N'lệnh 8c sử dụng => 1.login_logout | 2.đổi TG phục vụ',1,0)
GO

alter table [dbo].[Q_RecieverSMS] add [UserIds] varchar(50) null