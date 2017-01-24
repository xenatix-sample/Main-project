-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[ECIIFSPSpecialist]
-- Author:		John Crossen
-- Date:		09/03/2015
--
-- Purpose:		ECIIFSPSpecialist functionality
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/03/2015	John Crossen		TFS# 1277 - Initial creation.
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-- 02/25/2016	Kyle Campbell	Added Foreign Keys to Core.Users (UserID) for ModifiedBy/CreatedBy columns

CREATE TABLE [ECI].[ECIIFSPSpecialist](
	[ECIIFSPSpecialistID] [bigint] NOT NULL,
	[ECIIFSPID] [bigint] NOT NULL,
	[SpecialistID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Present] [bit] NOT NULL,
	[DateReviewed] [date] NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_ECIIFSPSpecialist] PRIMARY KEY CLUSTERED 
(
	[ECIIFSPSpecialistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[ECIIFSPSpecialist] ADD  CONSTRAINT [DF_ECIIFSPSpecialist_Present]  DEFAULT ((1)) FOR [Present]
GO


ALTER TABLE [ECI].[ECIIFSPSpecialist]  WITH CHECK ADD  CONSTRAINT [FK_ECIIFSPSpecialist_ECIIFSP] FOREIGN KEY([ECIIFSPID])
REFERENCES [ECI].[IFSP] ([IFSPID])
GO

ALTER TABLE [ECI].[ECIIFSPSpecialist] CHECK CONSTRAINT [FK_ECIIFSPSpecialist_ECIIFSP]
GO


ALTER TABLE [ECI].[ECIIFSPSpecialist]  WITH CHECK ADD  CONSTRAINT [FK_ECIIFSPSpecialist_Specialist] FOREIGN KEY([SpecialistID])
REFERENCES [ECI].[Specialist] ([SpecialistID])
GO

ALTER TABLE [ECI].[ECIIFSPSpecialist] CHECK CONSTRAINT [FK_ECIIFSPSpecialist_Specialist]
GO

ALTER TABLE [ECI].[ECIIFSPSpecialist]  WITH CHECK ADD  CONSTRAINT [FK_ECIIFSPSpecialist_Users] FOREIGN KEY([UserID])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[ECIIFSPSpecialist] CHECK CONSTRAINT [FK_ECIIFSPSpecialist_Users]
GO


ALTER TABLE [ECI].[ECIIFSPSpecialist]  WITH CHECK ADD  CONSTRAINT [FK_ECIIFSPSpecialist_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [Core].[Users] ([UserID])
GO

ALTER TABLE [ECI].[ECIIFSPSpecialist] CHECK CONSTRAINT [FK_ECIIFSPSpecialist_Users1]
GO

ALTER TABLE ECI.ECIIFSPSpecialist WITH CHECK ADD CONSTRAINT [FK_ECIIFSPSpecialist_UserModifedBy] FOREIGN KEY([ModifiedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ECIIFSPSpecialist CHECK CONSTRAINT [FK_ECIIFSPSpecialist_UserModifedBy]
GO
ALTER TABLE ECI.ECIIFSPSpecialist WITH CHECK ADD CONSTRAINT [FK_ECIIFSPSpecialist_UserCreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [Core].[Users] ([UserID])
GO
ALTER TABLE ECI.ECIIFSPSpecialist CHECK CONSTRAINT [FK_ECIIFSPSpecialist_UserCreatedBy]
GO
