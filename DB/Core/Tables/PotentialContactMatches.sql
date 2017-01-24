-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Core].[PotentialContactMatches]
-- Author:		Scott Martin
-- Date:		12/19/2016
--
-- Purpose:		Staging table for potential client merge
--
-- Notes:		Table to be truncated and re-populated
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/19/2016	Scott Martin	Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Core].[PotentialContactMatches](
	[ContactID] [BIGINT] NULL,
	[SSNMatch] BIT NOT NULL DEFAULT(0),
	[DOBMatch] BIT NOT NULL DEFAULT(0),
	[DLMatch] BIT NOT NULL DEFAULT(0),
	[PhoneMatch] BIT NOT NULL DEFAULT(0),
	[EmailMatch] BIT NOT NULL DEFAULT(0),
	[MedicaidIDMatch] BIT NOT NULL DEFAULT(0),
	[JailIDMatch] BIT NOT NULL DEFAULT(0),
	[IsActive] BIT NOT NULL DEFAULT(1),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE())
)

GO
ALTER TABLE Core.PotentialContactMatches WITH CHECK ADD CONSTRAINT [FK_PotentialContactMatches_ContactID] FOREIGN KEY ([ContactID]) REFERENCES [Registration].[Contact] ([ContactID])
GO
ALTER TABLE Core.PotentialContactMatches CHECK CONSTRAINT [FK_PotentialContactMatches_ContactID]
GO
ALTER TABLE Core.PotentialContactMatches WITH CHECK ADD CONSTRAINT [FK_PotentialContactMatches_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Core.PotentialContactMatches CHECK CONSTRAINT [FK_PotentialContactMatches_UserCreatedBy]
GO

