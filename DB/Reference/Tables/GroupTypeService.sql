-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[GroupTypeServices]
-- Author:		Sumana Sangapu
-- Date:		02/09/2016
--
-- Purpose:		To hold the services for under group types for scheduling group Sessions
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 02/09/2016	Sumana Sangapu	  Initital Creation . 
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[GroupTypeServices](
	[GroupTypeServiceID] [int] IDENTITY(1,1) NOT NULL,
	[GroupTypeID] int NULL,
	[ServicesID] int NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_GroupTypeServiceID] PRIMARY KEY CLUSTERED 
(
	[GroupTypeServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

ALTER TABLE [Reference].[GroupTypeServices]  WITH CHECK ADD  CONSTRAINT [FK_GroupTypeService_GroupTypeID] FOREIGN KEY([GroupTypeID])
REFERENCES [Reference].[GroupType]  ([GroupTypeID])
GO

ALTER TABLE [Reference].[GroupTypeServices] CHECK CONSTRAINT [FK_GroupTypeService_GroupTypeID]
GO

ALTER TABLE [Reference].[GroupTypeServices]  WITH CHECK ADD  CONSTRAINT [FK_GroupTypeService_ServiceID] FOREIGN KEY([ServicesID])
REFERENCES [Reference].[Services]  ([ServicesID])
GO

ALTER TABLE [Reference].[GroupTypeServices] CHECK CONSTRAINT [FK_GroupTypeService_ServiceID]
GO

ALTER TABLE Reference.GroupTypeServices WITH CHECK ADD CONSTRAINT [FK_GroupTypeServices_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.GroupTypeServices CHECK CONSTRAINT [FK_GroupTypeServices_UserModifedBy]
GO
ALTER TABLE Reference.GroupTypeServices WITH CHECK ADD CONSTRAINT [FK_GroupTypeServices_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.GroupTypeServices CHECK CONSTRAINT [FK_GroupTypeServices_UserCreatedBy]
GO
