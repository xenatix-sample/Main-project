
-- exec dbo.usp_SSISExtractICD10PCSCodes

CREATE PROCEDURE dbo.usp_SSISExtractICD10PCSCodes
AS
BEGIN
			-- SET NOCOUNT ON added to prevent extra result sets from
			-- interfering with SELECT statements.
			SET NOCOUNT ON;

			-- Extract the part of the XML into BodySystemsXML and BodyPartsKeyXML
			INSERT INTO [dbo].[ICD10PCSXMLStage]
			SELECT
				 z.query('<pcsRows> { for $row in axis return $row } </pcsRows>') as BodySystemsXML,
				 z.query('<pcsRows> { for $row in pcsRow return $row } </pcsRows>') as BodyPartsKeyXML
			FROM [dbo].[ICD10CodesXMLData]
			CROSS APPLY XMLData.nodes('//ICD10PCS.tabular/pcsTable') x(z)
 

			-- Extract BodySystems Details
			INSERT INTO  [dbo].[ICD10PCSBodySystemsStage]
			(PCSCodeID, POS, Value, Title, Label, Code, LongDescription, AsofYear )
			SELECT
					PCSCodeID,
					d.value('(@pos)[1]', 'int') as POS
					,d.value('(@values)[1]', 'varchar(100)') as Value
					,d.value('(title)[1]', 'varchar(100)') as Title
					,d.value('(label)[1]', 'varchar(100)') as Label
					,d.value('(label/@code)[1]', 'varchar(50)') as Code
					,d.value('(definition)[1]', 'varchar(500)') as LongDescription,
					'2016' as AsOfYear
			FROM [dbo].[ICD10PCSXMLStage]
			CROSS APPLY BodySystemsXML.nodes('//axis') x(d)


			-- Extract BodyPartKey Details
			INSERT INTO	dbo.ICD10PCSBodyPartsKeyStage ( PCSCodeID, Codes, Pos, Value, Title,[BodyPartsDefinitionXML], AsofYear  ) 
			SELECT
					PCSCodeID  
					,s.value('(../@codes)[1]', 'int') as codes
					,s.value('(@pos)[1]', 'int') as pos
					,s.value('(@values)[1]', 'varchar(100)') as value
					,s.value('(title)[1]', 'varchar(100)') as Title
					,s.query('<label> { for $row in label return $row } </label>') as [BodyPartsDefinitionXML]
					,'2016' as AsofYear
			FROM  [dbo].[ICD10PCSXMLStage]
			CROSS APPLY BodyPartsKeyXML.nodes('//pcsRow/axis') v(s)  
			ORDER BY pcsCodeid


			-- Extract BodyPartsKeyDefinition Details 
			INSERT INTO dbo.ICD10PCSBodyPartsKeyDefinitionStage (AxisID, PCSCodeID, Codes, Pos, Value, Label, Labelcode, AsOfYear) 
			SELECT	AxisID,PCSCodeID , codes,pos,value
					,v.value('(.)[1]','varchar(100)') as Label
					,v.value('(@code)[1]', 'varchar(50)') as labelcode 
					,'2016' as AsofYear
			FROM   dbo.ICD10PCSBodyPartsKeyStage
			CROSS APPLY BodyPartsDefinitionXML.nodes('label/label') x(v)   
			ORDER BY pcsCodeid

END
GO
