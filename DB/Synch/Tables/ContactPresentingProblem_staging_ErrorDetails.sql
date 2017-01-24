 -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[ContactPresentingProblem_Staging_ErrorDetails]
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


CREATE TABLE [Synch].[ContactPresentingProblem_Staging_ErrorDetails](
	[ContactPresentingProblemID] [bigint] NULL,
	[ContactID] [bigint] NULL,
	[PresentingProblemTypeID] [varchar](504) NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpirationDate] [varchar](255) NULL,
	[IsActive] [varchar](24) NULL,
	[ModifiedBy] [varchar](18) NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](18) NULL,
	[CreatedOn] [datetime] NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	ErrorSource varchar(50) NULL
) ON [PRIMARY]

GO


