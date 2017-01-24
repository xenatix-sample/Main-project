

-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[usp_ValidateDataContactDischargeNote_Staging_ErrorDetails]
-- Author:		Sumana Sangapu
-- Date:		06/24/2016
--
-- Purpose:		Validate lookup data in the Staging files used for Data Conversion
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/24/2016	Sumana Sangapu		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[ContactDischargeNote_Staging_ErrorDetails](
	[ContactDischargeNoteID] [bigint]  NOT NULL,
	[NoteHeaderID] [bigint] NOT NULL,
	[ContactAdmissionID] [bigint] NULL,
	[DischargeReasonID] [int] NOT NULL,
	[DischargeDate] [datetime] NOT NULL,
	[SignatureStatusID] [int] NOT NULL,
	[NoteText] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,
	ErrorSource nvarchar(50) NULL
) ON [PRIMARY] 

GO
