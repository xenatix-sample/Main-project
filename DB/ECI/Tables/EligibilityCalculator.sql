 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [ECI].[EligibilityCalculator]
-- Author:		Sumana Sangapu
-- Date:		10/16/2015
--
-- Purpose:		Holds the ELigibility Calculations
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		ECI.Eligibility
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/16/2015	Sumana Sangapu	 TFS:2700		Added a column
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [ECI].[EligibilityCalculator](
	[EligibilityCalculatorID] [bigint] IDENTITY(1,1) NOT NULL,
	[EligibilityID] [bigint] NOT NULL,
	[UseAdjustedAge] [bit] NULL,
	[TestingDate] [date] NULL,
	[GestationAge] [decimal](6, 2) NULL,
	[SCRawScore] [decimal](6, 2) NULL,
	[SCAEMonths] [decimal](6, 2) NULL,
	[PRRawScore] [decimal](6, 2) NULL,
	[PRAEMonths] [decimal](6, 2) NULL,
	[AdpAE] [decimal](6, 2) NULL,
	[AdpMonthsDelay] [decimal](6, 2) NULL,
	[AdpPCTDelay] [decimal](3, 0) NULL,
	[AIRawScore] [decimal](6, 2) NULL,
	[AIAEMonths] [decimal](6, 2) NULL,
	[PIRawScore] [decimal](6, 2) NULL,
	[PIAEMonths] [decimal](6, 2) NULL,
	[SRRawScore] [decimal](6, 2) NULL,
	[SRAEMonths] [decimal](6, 2) NULL,
	[PSAE] [decimal](6, 2) NULL,
	[PSMonthsDelay] [decimal](6, 2) NULL,
	[PSPCTDelay] [decimal](6, 2) NULL,
	[RCRawScore] [decimal](6, 2) NULL,
	[ECRawScore] [decimal](6, 2) NULL,
	[RCAEMonths] [decimal](6, 2) NULL,
	[ECAEMonths] [decimal](6, 2) NULL,
	[ECMonths] [decimal](6, 2) NULL,
	[ECPCTDelay] [decimal](6, 2) NULL,
	[CommAE] [decimal](6, 2) NULL,
	[CommMonthsDelay] [decimal](6, 2) NULL,
	[CommPCTDelay] [decimal](6, 2) NULL,
	[GMRawScore] [decimal](6, 2) NULL,
	[GMAE] [decimal](6, 2) NULL,
	[GMMonthsDelay] [decimal](6, 2) NULL,
	[GMPCTDelay] [decimal](6, 2) NULL,
	[FMRawScore] [decimal](6, 2) NULL,
	[FMAEMonths] [decimal](6, 2) NULL,
	[PMRawScore] [decimal](6, 2) NULL,
	[PMAEMonths] [decimal](6, 2) NULL,
	[FMPMAE] [decimal](6, 2) NULL,
	[FMPMMonthsDelay] [decimal](6, 2) NULL,
	[FMPMPCTDelay] [decimal](6, 2) NULL,
	[AMRS] [decimal](6, 2) NULL,
	[AMAEMonths] [decimal](6, 2) NULL,
	[RARawScore] [decimal](6, 2) NULL,
	[RAAEMonths] [decimal](6, 2) NULL,
	[PCRawScore] [decimal](6, 2) NULL,
	[PCAEMonths] [decimal](6, 2) NULL,
	[CogAE] [decimal](6, 2) NULL,
	[CogMonthsDelay] [decimal](6, 2) NULL,
	[CogPCTDelay] [decimal](6, 2) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
) ON [PRIMARY]

GO

ALTER TABLE [ECI].[EligibilityCalculator]  WITH CHECK ADD  CONSTRAINT [FK_EligibilityCalculator_EligibilityID] FOREIGN KEY([EligibilityID])
REFERENCES [ECI].[Eligibility] ([EligibilityID])
GO

ALTER TABLE [ECI].[EligibilityCalculator] CHECK CONSTRAINT [FK_EligibilityCalculator_EligibilityID]
GO