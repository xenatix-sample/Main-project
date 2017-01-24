-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ScreeningResults]
-- Author:		John Crossen
-- Date:		09/30/2015
--
-- Purpose:		ECI ScreeninTypeg Table
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	John Crossen     TFS:2542		Created .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[ScreeningResults](
	[ScreeningResultsID] [SMALLINT] IDENTITY(1,1) NOT NULL,
	[ScreeningResult] [NVARCHAR](255) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ScreeningResults] PRIMARY KEY CLUSTERED 
(
	[ScreeningResultsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE ECI.ScreeningResults WITH CHECK ADD CONSTRAINT [FK_ScreeningResults_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningResults CHECK CONSTRAINT [FK_ScreeningResults_UserModifedBy]
GO
ALTER TABLE ECI.ScreeningResults WITH CHECK ADD CONSTRAINT [FK_ScreeningResults_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningResults CHECK CONSTRAINT [FK_ScreeningResults_UserCreatedBy]
GO

