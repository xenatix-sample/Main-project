-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Core].[ServiceRecordingHeader]
-- Author:		Scott Martin
-- Date:		06/16/2016
--
-- Purpose:		Header Table for Service Recording
--
-- Notes:		Used as a way to manage voiding records and associating service recording records with one another
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/16/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[ServiceRecordingHeader](
	[ServiceRecordingHeaderID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ServiceRecordingHeader] PRIMARY KEY CLUSTERED 
(
	[ServiceRecordingHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE Core.ServiceRecordingHeader WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingHeader_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingHeader CHECK CONSTRAINT [FK_ServiceRecordingHeader_UserModifedBy]
GO
ALTER TABLE Core.ServiceRecordingHeader WITH CHECK ADD CONSTRAINT [FK_ServiceRecordingHeader_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.ServiceRecordingHeader CHECK CONSTRAINT [FK_ServiceRecordingHeader_UserCreatedBy]
GO

