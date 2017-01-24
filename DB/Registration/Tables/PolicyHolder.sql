-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Reference].[PolicyHolder]
-- Author:		Sumana Sangapu
-- Date:		07/21/2015
--
-- Purpose:		Reference of PolicyHolder details for Benefits Screen
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/21/2015	Sumana Sangapu	TaskID# 675 -	Initial creation.
-- 07/24/2015   John Crossen				Change isActive to NOT NULL and add default value
-- 07/30/2015   Sumana Sangapu  1016		Change schema from dbo to Reference
-- 01/13/2016	Scott Martin	Added columns CreatedBy, CreatedOn, SystemCreatedOn, SystemModifiedOn
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Reference].[PolicyHolder](
	[PolicyHolderID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyHolder] [nvarchar](50) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
    [ModifiedBy] INT NOT NULL,
    [ModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[CreatedBy] INT NOT NULL DEFAULT(1),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemCreatedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[SystemModifiedOn] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
 CONSTRAINT [PK_PolicyHolder_PolicyHolderID] PRIMARY KEY CLUSTERED 
(
	[PolicyHolderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PolicyHolder] UNIQUE NONCLUSTERED 
(
	[PolicyHolder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


