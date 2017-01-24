-----------------------------------------------------------------------------------------------------------------------
-- Table:	    [dbo].[SSISPkgConfigurations]
-- Author:		Sumana Sangapu
-- Date:		03/01/2016
--
-- Purpose:		Holds the PackageConfigurations for SSIS Packages
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/01/2016	Sumana Sangapu	 - Initial creation.
-----------------------------------------------------------------------------------------------------------------------


CREATE TABLE [dbo].[SSISPkgConfigurations]
(
	ConfigurationFilter NVARCHAR(255) NOT NULL,
	ConfiguredValue NVARCHAR(255) NULL,
	PackagePath NVARCHAR(255) NOT NULL,
	ConfiguredValueType NVARCHAR(20) NOT NULL
)