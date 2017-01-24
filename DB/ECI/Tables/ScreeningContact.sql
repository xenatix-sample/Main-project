-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ScreeningContact]
-- Author:		John Crossen
-- Date:		09/30/2015
--
-- Purpose:		ECI ScreeningContact Table
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

CREATE TABLE [ECI].[ScreeningContact](
	[ScreeningContactID] [BIGINT] IDENTITY(1,1) NOT NULL,
	[ScreeningID] [BIGINT] NOT NULL,
	[ContactID] [BIGINT] NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ScreeningContact] PRIMARY KEY CLUSTERED 
(
	[ScreeningContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ScreeningContact]  WITH CHECK ADD  CONSTRAINT [FK_ScreeningContact_Contact] FOREIGN KEY([ContactID])
REFERENCES [Registration].[Contact] ([ContactID])
GO

ALTER TABLE [ECI].[ScreeningContact] CHECK CONSTRAINT [FK_ScreeningContact_Contact]
GO

ALTER TABLE [ECI].[ScreeningContact]  WITH CHECK ADD  CONSTRAINT [FK_ScreeningContact_Screening] FOREIGN KEY([ScreeningID])
REFERENCES [ECI].[Screening] ([ScreeningID])
GO

ALTER TABLE [ECI].[ScreeningContact] CHECK CONSTRAINT [FK_ScreeningContact_Screening]
GO

ALTER TABLE ECI.ScreeningContact WITH CHECK ADD CONSTRAINT [FK_ScreeningContact_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningContact CHECK CONSTRAINT [FK_ScreeningContact_UserModifedBy]
GO
ALTER TABLE ECI.ScreeningContact WITH CHECK ADD CONSTRAINT [FK_ScreeningContact_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ScreeningContact CHECK CONSTRAINT [FK_ScreeningContact_UserCreatedBy]
GO

