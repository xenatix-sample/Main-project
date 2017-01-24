
 -----------------------------------------------------------------------------------------------------------------------
-- Table:		dbo.[Users_Staging_ErrorDetails]
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

CREATE TABLE [Synch].[Users_Staging_ErrorDetails](
	[UserID] [bigint] NULL,
	[UserName] [varchar](231) NULL,
	[FirstName] [varchar](121) NULL,
	[LastName] [varchar](187) NULL,
	[MiddleName] [varchar](187) NULL,
	[GenderID] [varchar](66) NULL,
	[DOB] [date] NULL,
	[Password] [varchar](255) NULL,
	[IsTemporaryPassword] [varchar](55) NULL,
	[EffectiveToDate] [varchar](255) NULL,
	[LoginAttempts] [bigint] NULL,
	[LoginCount] [bigint] NULL,
	[LastLogin] [varchar](255) NULL,
	[UserGUID] [varchar](352) NULL,
	[ADFlag] [varchar](44) NULL,
	PrintSignature varchar(50) NULL,
	DigitalPAssword varchar(50) NULL,
	IsInternal varchar(50) NULL,
	EffectiveFromDate varchar(50) NULL,
	[IsActive] [varchar](55) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](33) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](33) NULL,
	[SystemCreatedOn] [datetime] NULL,
	[SystemModifiedOn] [datetime] NULL,
	[ErrorSource] nvarchar(50) NULL
) ON [PRIMARY]  
GO
