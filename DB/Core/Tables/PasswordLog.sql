-----------------------------------------------------------------------------------------------------------------------
-- Table:	    Core.PasswordLog
-- Author:		Scott Martin
-- Date:		06/07/2016
--
-- Purpose:		To log all of the passwords a user has used
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/07/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[PasswordLog](
	[PasswordLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[Password] [nvarchar](128) NULL,
	[UserID] [int] NOT NULL,
	[AuditTimeStamp] [datetime] NOT NULL DEFAULT (GETUTCDATE())
 CONSTRAINT [PK_PasswordLog_PasswordLogID] PRIMARY KEY CLUSTERED 
(
	[PasswordLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Core].[PasswordLog] WITH CHECK ADD CONSTRAINT [FK_PasswordLog_UserID] FOREIGN KEY([UserID]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE [Core].[PasswordLog] CHECK CONSTRAINT [FK_PasswordLog_UserID]
GO

CREATE TRIGGER Core.tgr_LogPassword ON Core.Users
FOR INSERT, UPDATE
AS
BEGIN
IF UPDATE (Password)
	BEGIN
	INSERT INTO Core.PasswordLog
	(
		Password,
		UserID,
		AuditTimeStamp
	)
	SELECT
		I.Password,
		I.UserID,
		GETUTCDATE()
	FROM
		Inserted I
	END
END