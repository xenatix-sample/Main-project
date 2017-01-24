-----------------------------------------------------------------------------------------------------------------------
-- Table:		[Synch].[ContactRelationshipType_Staging_Error_Details]
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


CREATE TABLE [Synch].[ContactRelationshipType_Staging_Error_Details](
	[ContactRelationshipTypeID] [bigint]  NOT NULL,
	[ContactRelationshipID] [bigint] NOT NULL,
	[RelationshipTypeID] [int] NOT NULL,
	[IsPolicyHolder] [bit] NULL,
	[OtherRelationship] [nvarchar](200) NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL ,
	[CreatedBy] [int] NOT NULL ,
	[CreatedOn] [datetime] NOT NULL ,
	[SystemCreatedOn] [datetime] NOT NULL,
	[SystemModifiedOn] [datetime] NOT NULL,

) ON [PRIMARY]

GO
