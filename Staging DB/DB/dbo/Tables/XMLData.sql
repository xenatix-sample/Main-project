 
 -----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[XMLData]
-- Author:		Sumana Sangapu
-- Date:		02/04/2016
--
-- Purpose:		Holds the XML data for drug staging info
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/03/2015	Sumana Sangapu	 Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[XMLData](
	[XMLData] [xml] NULL,
	[LoadedDateTime] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


