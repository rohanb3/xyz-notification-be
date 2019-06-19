USE [db.xyzies.notification]
GO

DELETE FROM MessageTemplates
DELETE FROM TypeOfMessages

---------------------------------------------------------------------------------------
---------------------- TypeOfMessages  ------------------------------------------------
INSERT INTO [dbo].[TypeOfMessages]
           ([Id]
           ,[IsDeleted]
           ,[Type])
     VALUES
           ('A1E4925F-D2E3-450C-8A99-27EC5F6899B2'
           ,0
           ,'email')
GO
---------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------
---------------------- MessageTemplates  ----------------------------------------------
INSERT INTO [dbo].[MessageTemplates]
           ([Id]
           ,[IsDeleted]
           ,[CreateOn]
           ,[Cause]
           ,[Subject]
           ,[MessageBody]
           ,[TypeOfMessageId])
     VALUES
           ('58A6C1D8-FA41-4B0C-9C11-F6C20429BDA7'
           ,0
           , GETUTCDATE()
           ,'offline'
           ,'VSP {udid} is offline'
           ,'<p>Your VSP last time was online at {lastheartbeat}</p><p></p><p>The VSP is located at this address:</p><p>{address}<p>{town}, {postcode}<p>{country}<p>{notes}.<p></p>
<p>This may be due to a power outage, the VSP being disconnected from power or the internet, or an outage of your ISP.
Feedback can be sent to support form on mailto:<a href = "mailto: {mailto}">{mailto}</a></p>'
           ,'A1E4925F-D2E3-450C-8A99-27EC5F6899B2')
GO
---------------------------------------------------------------------------------------