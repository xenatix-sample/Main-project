 
 -----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[dbo].[SSISPkgConfigurations]
-- Author:		Sumana Sangapu
-- Date:		03/16/2016
--
-- Purpose:		Seed Data for SSIS Pkg Configurations
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/16/2016	Sumana Sangapu		TFS#  - Initial creation.
-----------------------------------------------------------------------------------------------------------------------

DECLARE @SSISPkgConfigurations
 TABLE (
 ConfigurationFilter nvarchar(255) NOT NULL,
 ConfiguredValue nvarchar(255) NULL,
 PackagePath nvarchar(255) NOT NULL,
 ConfiguredValueType nvarchar(20) NOT NULL
)
INSERT INTO @SSISPkgConfigurations ( [ConfigurationFilter],[ConfiguredValue],[PackagePath],[ConfiguredValueType] ) VALUES 
('Axis','.','\Package.Connections[Axis].Properties[ServerName]','String'), 
('Axis','Axis','\Package.Connections[Axis].Properties[InitialCatalog]','String'), 
('Axis','Data Source=.;Initial Catalog=Axis;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;','\Package.Connections[Axis].Properties[ConnectionString]','String'), 
('AxisStaging','.','\Package.Connections[AxisStaging].Properties[ServerName]','String'), 
('AxisStaging','AxisStaging','\Package.Connections[AxisStaging].Properties[InitialCatalog]','String'), 
('AxisStaging','Data Source=.;Initial Catalog=AxisStaging;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;','\Package.Connections[AxisStaging].Properties[ConnectionString]','String') 

MERGE INTO  [dbo].[SSISPkgConfigurations]  AS TARGET USING ( SELECT * FROM @SSISPkgConfigurations) AS SOURCE 
ON		SOURCE.[ConfigurationFilter] = TARGET.[ConfigurationFilter] 
AND		SOURCE.[ConfiguredValue] = TARGET.[ConfiguredValue] 
AND		SOURCE.[PackagePath] = TARGET.[PackagePath]
AND		SOURCE.[ConfiguredValueType] = TARGET.[ConfiguredValueType]
 WHEN MATCHED THEN UPDATE SET   [ConfigurationFilter] = SOURCE.[ConfigurationFilter] ,
								[ConfiguredValue] = SOURCE.[ConfiguredValue] ,
								[PackagePath] = SOURCE.[PackagePath],
								[ConfiguredValueType] = SOURCE.[ConfiguredValueType]
 WHEN NOT MATCHED THEN INSERT ([ConfigurationFilter],[ConfiguredValue],[PackagePath],[ConfiguredValueType] )
 VALUES ( SOURCE.[ConfigurationFilter],SOURCE.[ConfiguredValue],SOURCE.[PackagePath],SOURCE.[ConfiguredValueType]) WHEN NOT MATCHED BY SOURCE THEN DELETE ;
  