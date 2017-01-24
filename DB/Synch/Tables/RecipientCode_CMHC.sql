-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [Synch].[[RecipientCode]]
-- Author:		John Crossen
-- Date:		05/03/2016
--
-- Purpose:		Mappigng table for CMHC
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 05/03/2016	John Crossen	 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [Synch].[RecipientCode](
	RecipentCodeID INT NOT NULL IDENTITY(1,1),
	[aXis] [VARCHAR](50) NULL,
	[CMHCCode] [VARCHAR](50) NULL,
	[CMHCDescription] [VARCHAR](50) NULL,
	CONSTRAINT [PK_CMHCRecipientCode] PRIMARY KEY CLUSTERED 
(
	RecipentCodeID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO



