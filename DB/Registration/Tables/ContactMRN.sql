-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Registration].[ContactMRN]
-- Author:		Sumana Sangapu
-- Date:		10/08/2015
--
-- Purpose:		Holds the MRN for the Contact 
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/06/2015	Sumana Sangapu	 TFS:2497		Added a column
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/26/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Registration].[ContactMRN](
	[ContactID] [bigint] NOT NULL,
	[MRN] [bigint] IDENTITY(1,1) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
CONSTRAINT [PK_ContactMRN_ContactID] PRIMARY KEY CLUSTERED 
( 
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ContactIDMRN] UNIQUE NONCLUSTERED 
(
		[ContactID] ASC,
		[MRN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

) ON [PRIMARY]
GO

ALTER TABLE [Registration].[ContactMRN] WITH CHECK ADD  CONSTRAINT [FK_ContactMRN_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [Registration].[ContactMRN] CHECK CONSTRAINT [FK_ContactMRN_Contact]
GO

ALTER TABLE Registration.ContactMRN WITH CHECK ADD CONSTRAINT [FK_ContactMRN_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactMRN CHECK CONSTRAINT [FK_ContactMRN_UserModifedBy]
GO
ALTER TABLE Registration.ContactMRN WITH CHECK ADD CONSTRAINT [FK_ContactMRN_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Registration.ContactMRN CHECK CONSTRAINT [FK_ContactMRN_UserCreatedBy]
GO

