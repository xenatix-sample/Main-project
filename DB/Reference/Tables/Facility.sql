-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Facility]
-- Author:		John Crossen
-- Date:		09/10/2015
--
-- Purpose:		Facility data
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/10/2015	John Crossen	TFS# 2229 Initital Creation .
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[Facility](
	[FacilityID] [INT] NOT NULL IDENTITY(1,1),
	[FacilityName] [NVARCHAR] (255) NOT NULL,
	[FacilityAddressID] BIGINT NULL,
	[FacilityPhoneID] BIGINT NULL,
	[FacilityEmailID] BIGINT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),

 CONSTRAINT [PK_FacilityID] PRIMARY KEY CLUSTERED 
(
	[FacilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE UNIQUE NONCLUSTERED INDEX [IX_Facility_FacilityName] ON [Reference].[Facility]
(
	[FacilityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO



ALTER TABLE [Reference].[Facility]  WITH CHECK ADD  CONSTRAINT [FK_Facility_Addresses] FOREIGN KEY([FacilityAddressID])
REFERENCES [Core].[Addresses] ([AddressID])
GO

ALTER TABLE [Reference].[Facility] CHECK CONSTRAINT [FK_Facility_Addresses]
GO


ALTER TABLE [Reference].[Facility]  WITH CHECK ADD  CONSTRAINT [FK_Facility_Email] FOREIGN KEY([FacilityEmailID])
REFERENCES [Core].[Email] ([EmailID])
GO

ALTER TABLE [Reference].[Facility] CHECK CONSTRAINT [FK_Facility_Email]
GO



ALTER TABLE [Reference].[Facility]  WITH CHECK ADD  CONSTRAINT [FK_Facility_Phone] FOREIGN KEY([FacilityPhoneID])
REFERENCES [Core].[Phone] ([PhoneID])
GO

ALTER TABLE [Reference].[Facility] CHECK CONSTRAINT [FK_Facility_Phone]
GO

ALTER TABLE Reference.Facility WITH CHECK ADD CONSTRAINT [FK_Facility_UserModifedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.Facility CHECK CONSTRAINT [FK_Facility_UserModifedBy]
GO
ALTER TABLE Reference.Facility WITH CHECK ADD CONSTRAINT [FK_Facility_UserCreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE Reference.Facility CHECK CONSTRAINT [FK_Facility_UserCreatedBy]
GO
