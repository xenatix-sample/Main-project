  -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[ContactRelationship_Staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		05/19/2016
--
-- Purpose:		Table to hold the error details from the validation of lookup data
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/19/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Synch].[ContactRelationship_Staging_ErrorDetails](
	[ContactRelationshipID] [bigint] NULL,
	[ParentContactID] [bigint] NULL,
	[ChildContactID] [bigint] NULL,
	[ContactTypeID] [varchar](110) NULL,
	[PhonePermissionID] [varchar](255) NULL,
	[EmailPermissionID] [varchar](255) NULL,
	[ReceiveCorrespondenceID] [varchar](255) NULL,
	[IsEmergency] [varchar](55) NULL,
	[EducationStatusID] [varchar](255) NULL,
	[SchoolAttended] [varchar](255) NULL,
	[SchoolBeginDate] [varchar](255) NULL,
	[SchoolEndDate] [varchar](255) NULL,
	[EmploymentStatusID] [varchar](255) NULL,
	[LivingWithClientStatus] [varchar](255) NULL,
	[IsActive] [varchar](44) NULL,
	[ModifiedBy] [varchar](33) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]

GO


