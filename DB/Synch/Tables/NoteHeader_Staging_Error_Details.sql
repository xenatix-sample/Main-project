
-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[NoteHeader_Staging_Error_Details]
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

CREATE TABLE [Synch].[NoteHeader_Staging_Error_Details](
	[NoteHeaderID] [bigint]  NOT NULL,
	[ContactID] [bigint] NOT NULL,
	[NoteTypeID] [int] NOT NULL,
	[TakenBy] [int] NOT NULL,
	[TakenTime] [datetime] NULL,
	[IsActive] [bit] NOT NULL ,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL ,
	[CreatedBy] [int] NOT NULL ,
	[CreatedOn] [datetime] NOT NULL ,
	[SystemCreatedOn] [datetime] NOT NULL ,
	[SystemModifiedOn] [datetime] NOT NULL ,
	ErrorSource nvarchar(50) NULL
) ON [PRIMARY]

GO
