-----------------------------------------------------------------------------------------------------------------------
-- Tble:		Synch.SSISConfigurations 
-- Author:		Sumana Sangapu
-- Date:		06/30/2016
--
-- Purpose:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 06/30/2016	Sumana Sangapu SSIS PackageConfigurations 
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [Synch].[SSISConfigurations](
	[ConfigurationFilter] [nvarchar](255) NOT NULL,
	[ConfiguredValue] [nvarchar](255) NULL,
	[PackagePath] [nvarchar](255) NOT NULL,
	[ConfiguredValueType] [nvarchar](20) NOT NULL
) ON [PRIMARY]

GO


