-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[Specialist]
-- Author:		John Crossen
-- Date:		09/03/2015
--
-- Purpose:		Specialist functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/03/2015	John Crossen		TFS# 1277 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns


CREATE TABLE [ECI].[Specialist](
	[SpecialistID] [int] IDENTITY(1,1) NOT NULL,
	[Specialist] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Specialist_IsActive]  DEFAULT ((1)),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Specialist] PRIMARY KEY CLUSTERED 
(
	[SpecialistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE NONCLUSTERED INDEX [IX_Specialist_Specialist] ON [ECI].[Specialist]
(
	[Specialist] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER TABLE ECI.Specialist WITH CHECK ADD CONSTRAINT [FK_Specialist_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Specialist CHECK CONSTRAINT [FK_Specialist_UserModifedBy]
GO
ALTER TABLE ECI.Specialist WITH CHECK ADD CONSTRAINT [FK_Specialist_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.Specialist CHECK CONSTRAINT [FK_Specialist_UserCreatedBy]
GO
