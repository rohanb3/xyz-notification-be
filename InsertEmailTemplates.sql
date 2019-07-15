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
           ,'<p>Your VSP last time was online at {lastheartbeat}</p><p></p><p>The VSP is located at this address:</p><p>Address: {address}<p>Town: {town}, Post Code: {postcode}<p>Country: {country}<p>Notes: {notes}<p></p>
<p>This may be due to a power outage, the VSP being disconnected from power or the internet, or an outage of your ISP.
Feedback can be sent to support form on mailto:<a href = "mailto: {mailto}">{mailto}</a></p>'
           ,'A1E4925F-D2E3-450C-8A99-27EC5F6899B2'),
		   ('3CB8C0ED-61F7-4A8F-A220-048E55E4D3D6'
           ,0
           , GETUTCDATE()
           ,'online'
           ,'VSP {udid} is online'
           ,'<p>VSP has come back online at {lastheartbeat}</p><p></p><p>Before that we received a heartbeat from your router last time at {previousheartbeat}</p>
		   <p>The VSP is located at this address:</p><p>Address: {address}<p>Town: {town}, Post Code: {postcode}<p>Country: {country}<p>Notes: {notes}<p></p>'
           ,'A1E4925F-D2E3-450C-8A99-27EC5F6899B2'),
		   ('0D106E34-B607-452C-867A-B7FC31398722'
           ,0
           , GETUTCDATE()
           ,'outoflocation'
           ,'VSP {udid} is out of location'
           ,'<p>VSP was out of location at {lastheartbeat}</p><p></p>
		   <p>The VSP is located at this address:</p><p>Address: {address}<p>Town: {town}, Post Code: {postcode}<p>Country: {country}<p>Notes: {notes}<p></p>
		   <p>Feedback can be sent to support form on mailto:<a href = "mailto: {mailto}">{mailto}</a></p>'
           ,'A1E4925F-D2E3-450C-8A99-27EC5F6899B2'),
		   ('183C8929-ABF4-4837-87DD-C322AD37B19E'
           ,0
           , GETUTCDATE()
           ,'inlocation'
           ,'VSP {udid} is back to location'
           ,'<p>VSP was back to location at {lastheartbeat}</p><p></p>
		   <p>The VSP is located at this address:</p><p>Address: {address}<p>Town: {town}, Post Code: {postcode}<p>Country: {country}<p>Notes: {notes}<p></p>
		   <p>Feedback can be sent to support form on mailto:<a href = "mailto: {mailto}">{mailto}</a></p>'
           ,'A1E4925F-D2E3-450C-8A99-27EC5F6899B2')
GO
---------------------------------------------------------------------------------------