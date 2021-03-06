
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[dbo].[usp_SSISExtractDrugs] 
-- Author:		Sumana Sangapu	
-- Date:		12/1/2015
--
-- Purpose:		Extract Drugs details through SSIS
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/1/2015	Sumana Sangapu		Initial creation.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROC [dbo].[usp_SSISExtractDrugs] 
AS
BEGIN
     			------ Retrieve DrugInformation ------------------
				;WITH XMLNAMESPACES (DEFAULT 'urn:hl7-org:v3')
				INSERT INTO [dbo].[DrugInformationStage]
				SELECT
					   d.value('(setId/@root)[1]', 'uniqueidentifier') as SetId
					   ,d.value('(code/@displayName)[1]', 'varchar(100)') as Category
					   ,d.value('(//representedOrganization/name)[1]', 'varchar(100)') as Packager
					   ,m.value('(code/@code)[1]', 'varchar(100)') as NDCCode
					   ,m.value('name[1]', 'varchar(100)') as ProductName
					   ,m.value('(formCode/@displayName)[1]', 'varchar(100)') as ProductForm
					   ,m.value('(//genericMedicine/name)[1]', 'varchar(255)') as GenericMedicine
					   ,m.query('declare default element namespace "urn:hl7-org:v3"; <ingredients> { for $ingredient in ingredient return $ingredient} </ingredients>') as Ingredients
					   ,m.query('declare default element namespace "urn:hl7-org:v3"; <asContent> { for $asContent in asContent return $asContent} </asContent>') as AsContents
					   , 1 as IsActive
					   ,GETUTCDATE() as ImportDate
					   ,CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT
				FROM dbo.XMLData
				CROSS APPLY XMLData.nodes('document') n(d)
				CROSS APPLY XMLData.nodes('//manufacturedProduct/manufacturedProduct') d(m)

     			------ Retrieve Ingredients ------------------
				;WITH XMLNAMESPACES (DEFAULT 'urn:hl7-org:v3')
				INSERT INTO  [dbo].[IngredientsStage]
				SELECT setid,C.value('(@classCode)[1]','varchar(50)') AS ClassCode,
							 C.value('(quantity/numerator/@value)[1]','varchar(50)') AS NumeratorValue,
							 C.value('(quantity/numerator/@unit)[1]','varchar(50)') AS NumeratorUnit,
							 C.value('(quantity/denominator/@value)[1]','varchar(50)') AS DenominatorValue,
							 C.value('(quantity/denominator/@unit)[1]','varchar(50)') AS DenominatorUnit,
							 C.value('(ingredientSubstance/code/@code)[1]','varchar(50)') AS SubstanceCode,
							 C.value('(ingredientSubstance/name)[1]','varchar(50)') AS SubstanceName,
							 1 as IsActive,
						     GETUTCDATE() as ImportDate,
						     CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT
				FROM  [dbo].[DrugInformationStage] 
				CROSS APPLY Ingredients.nodes('//ingredient') as t(c)

				---- Retrieve PackagedProduct ----------------
				;WITH XMLNAMESPACES (DEFAULT 'urn:hl7-org:v3')
				INSERT INTO [dbo].[PackageContentsStage]
				SELECT Setid,
							 C.value('(quantity/numerator/@value)[1]','varchar(50)') AS NumeratorValue,
							 C.value('(quantity/numerator/@unit)[1]','varchar(50)') AS NumeratorUnit,
							 C.value('(quantity/denominator/@value)[1]','varchar(50)') AS DenominatorValue,
							 C.value('(quantity/denominator/@unit)[1]','varchar(50)') AS DenominatorUnit,
							 C.value('(containerPackagedProduct/code/@code)[1]','varchar(50)') AS ProductCode,
							 C.value('(containerPackagedProduct/code/@codeSystem)[1]','varchar(50)') AS ProductCodeSystem,
							 C.value('(containerPackagedProduct/formCode/@code)[1]','varchar(50)') AS ProductformCode,
							 C.value('(containerPackagedProduct/formCode/@codeSystem)[1]','varchar(50)') AS ProductformCodeSystem,
							 C.value('(containerPackagedProduct/formCode/@displayName)[1]','varchar(50)') AS ProductformdisplayName,
							 1 as IsActive,
						     GETUTCDATE() as ImportDate,
						     CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT							  
				FROM  [dbo].[DrugInformationStage]
				CROSS APPLY AsContents.nodes('//asContent/asContent') as t(c)

END