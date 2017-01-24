-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[ContactTypeHeirarchy]
-- Author:		John Crossen
-- Date:		01/15/2016
--
-- Purpose:		Table to group contact types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2016	John Crossen	TFS#5411 - Initial creation.


CREATE TABLE [Reference].[ContactTypeHeirarchy](
	[ContactTypeHeirarchy] [int] IDENTITY(1,1) NOT NULL,
	[ParentContactTypeID] [int] NOT NULL,
	[ChildContactTypeID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactTypeHeirarchy] PRIMARY KEY CLUSTERED 
(
	[ContactTypeHeirarchy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] ADD  CONSTRAINT [DF_CallCenterTypeIsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] ADD  CONSTRAINT [DF_CallCenterTypeModifiedOn]  DEFAULT (getutcdate()) FOR [ModifiedOn]
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] ADD  CONSTRAINT [DF_CallCenterTypeCreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] ADD  CONSTRAINT [DF_CallCenterTypeSystemCreatedOn]  DEFAULT (getutcdate()) FOR [SystemCreatedOn]
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] ADD  CONSTRAINT [DF_CallCenterTypeSystemModifiedOn]  DEFAULT (getutcdate()) FOR [SystemModifiedOn]
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy]  WITH CHECK ADD  CONSTRAINT [FK_ContactTypeHeirarchy_ContactType] FOREIGN KEY([ChildContactTypeID])
REFERENCES [Reference].[ContactType] ([ContactTypeID])
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] CHECK CONSTRAINT [FK_ContactTypeHeirarchy_ContactType]
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy]  WITH CHECK ADD  CONSTRAINT [FK_ContactTypeHeirarchy_ContactType1] FOREIGN KEY([ParentContactTypeID])
REFERENCES [Reference].[ContactType] ([ContactTypeID])
GO

ALTER TABLE [Reference].[ContactTypeHeirarchy] CHECK CONSTRAINT [FK_ContactTypeHeirarchy_ContactType1]
GO

ALTER TABLE Reference.ContactTypeHeirarchy WITH CHECK ADD CONSTRAINT [FK_ContactTypeHeirarchy_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ContactTypeHeirarchy CHECK CONSTRAINT [FK_ContactTypeHeirarchy_UserModifedBy]
GO
ALTER TABLE Reference.ContactTypeHeirarchy WITH CHECK ADD CONSTRAINT [FK_ContactTypeHeirarchy_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.ContactTypeHeirarchy CHECK CONSTRAINT [FK_ContactTypeHeirarchy_UserCreatedBy]
GO
