  -----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[Payor]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Lookup of Payor details for the Benefits Screens.
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TaskID#	  583 -	Initial creation.
-- 07/24/2015   John Crossen					Change isActive to NOT NULL and add default value
-- 07/30/2015	Sumana Sangapu			 1016	Change schema from dbo to Registration/Reference/Core
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[Payor](
	[PayorID] [int] IDENTITY(1,1) NOT NULL,
	[PayorCode] [int] NOT NULL,
	[PayorName] [nvarchar](250) NOT NULL,
	[PayorRank] [int] NOT NULL,
	[EffectiveDate] [date] NULL,
	[ExpirationDate] [date] NULL,
	[ExpirationReason] [nvarchar](500) NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_Payor_PayorID] PRIMARY KEY CLUSTERED 
(
	[PayorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


