
-- exec dbo.usp_SSISExtractICD10CMDrugs

CREATE PROCEDURE dbo.usp_SSISExtractICD10CMDrugs
AS
BEGIN
				--Extract Level1 Information
				INSERT INTO [dbo].[ICD10DrugsCMLevel1Stage]
				SELECT
					Parent.Elm.value('(title)[1]', 'varchar(100)') AS 'Title',
					Parent.Elm.value('(cell)[1]', 'varchar(100)') AS 'cell1',
					Parent.Elm.value('(cell)[2]', 'varchar(100)') AS 'cell2',
					Parent.Elm.value('(cell)[3]', 'varchar(50)') as 'Cell3',
					Parent.Elm.value('(cell)[4]', 'varchar(50)') as 'Cell4',
					Parent.Elm.value('(cell)[5]', 'varchar(50)') as 'Cell5',
					Parent.Elm.value('(cell)[6]',  'varchar(50)') as 'Cell6',

					Child.Elm.value('(@level)[1]', 'int') AS 'level1',
					Child.Elm.value('(title)[1]', 'varchar(100)') AS 'term1Title1',
					Child.Elm.value('(cell)[1]', 'varchar(100)') AS 'term1cell1',
					Child.Elm.value('(cell)[2]', 'varchar(100)') AS 'term1cell2',
					Child.Elm.value('(cell)[3]', 'varchar(100)') AS 'term1cell3',
					Child.Elm.value('(cell)[4]', 'varchar(100)') AS 'term1cell4',
					Child.Elm.value('(cell)[5]', 'varchar(100)') AS 'term1cell5',
					
					YEAR(GETDATE()) AS AsofYear,
										--'2016' as AsofYear,
				    GETUTCDATE() as ImportDate,
					CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT

				FROM [ICD10CodesXMLData]
				CROSS APPLY XMLData.nodes('ICD10CM.index/letter/mainTerm') AS Parent(Elm)
				CROSS APPLY
					Parent.Elm.nodes('term') AS Child(Elm) 

				--Extract Level2 Information
				INSERT INTO [dbo].[ICD10DrugsCMLevel2Stage]
				SELECT
					Parent.Elm.value('(title)[1]', 'varchar(100)') AS 'Title',
	
					Child1.Elm.value('(title)[1]', 'varchar(100)') AS 'term1Title1',

					Child2.Elm.value('(@level)[1]', 'int') AS 'level2',
					Child2.Elm.value('(title)[1]', 'varchar(100)') AS 'term2Title2',
					Child2.Elm.value('(cell)[1]', 'varchar(100)') AS 'term2cell1',
					Child2.Elm.value('(cell)[2]', 'varchar(100)') AS 'term2cell2',
					Child2.Elm.value('(cell)[3]', 'varchar(100)') AS 'term2cell3',
					Child2.Elm.value('(cell)[4]', 'varchar(100)') AS 'term2cell4',
					Child2.Elm.value('(cell)[5]', 'varchar(100)') AS 'term2cell5',

					YEAR(GETDATE()) AS AsofYear,
										--'2016' as AsofYear,
				    GETUTCDATE() as ImportDate,
					CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT

				FROM [ICD10CodesXMLData]
				CROSS APPLY XMLData.nodes('ICD10CM.index/letter/mainTerm') AS Parent(Elm)
				CROSS APPLY
					Parent.Elm.nodes('term') AS Child1(Elm) 
				CROSS APPLY 
					Child1.Elm.nodes('term') AS Child2(Elm) 

				--Extract Level3 Information
				INSERT INTO [dbo].[ICD10DrugsCMLevel3Stage]
				SELECT
					Parent.Elm.value('(title)[1]', 'varchar(100)') AS 'Title',
	
					Child1.Elm.value('(title)[1]', 'varchar(100)') AS 'term1Title1',

					Child2.Elm.value('(@level)[1]', 'int') AS 'level2',
					Child2.Elm.value('(title)[1]', 'varchar(100)') AS 'term2Title2',

					Child3.Elm.value('(@level)[1]', 'int') AS 'level3',
					Child3.Elm.value('(title)[1]', 'varchar(100)') AS 'term3Title3',
					Child3.Elm.value('(cell)[1]', 'varchar(100)') AS 'term3cell1',
					Child3.Elm.value('(cell)[2]', 'varchar(100)') AS 'term3cell2',
					Child3.Elm.value('(cell)[3]', 'varchar(100)') AS 'term3cell3',
					Child3.Elm.value('(cell)[4]', 'varchar(100)') AS 'term3cell4',
					Child3.Elm.value('(cell)[5]', 'varchar(100)') AS 'term3cell5',
					Child3.Elm.value('(cell)[6]', 'varchar(100)') AS 'term3cell6',

					YEAR(GETDATE()) AS AsofYear,
										--'2016' as AsofYear,
				    GETUTCDATE() as ImportDate,
					CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT

				FROM [ICD10CodesXMLData]
				CROSS APPLY	XMLData.nodes('ICD10CM.index/letter/mainTerm') AS Parent(Elm)
				CROSS APPLY
					Parent.Elm.nodes('term') AS Child1(Elm) 
				CROSS APPLY 
					Child1.Elm.nodes('term') AS Child2(Elm) 
				CROSS APPLY 
					Child2.Elm.nodes('term') AS Child3(Elm) 
	

				--Extract Level4 Information
				INSERT INTO [dbo].[ICD10DrugsCMLevel4Stage]
				SELECT
					Parent.Elm.value('(title)[1]', 'varchar(100)') AS 'Title',
	
					Child1.Elm.value('(title)[1]', 'varchar(100)') AS 'term1Title1',

					Child2.Elm.value('(@level)[1]', 'int') AS 'level2',
					Child2.Elm.value('(title)[1]', 'varchar(100)') AS 'term2Title2',

					Child3.Elm.value('(@level)[1]', 'int') AS 'level3',
					Child3.Elm.value('(title)[1]', 'varchar(100)') AS 'term3Title3',

					Child4.Elm.value('(@level)[1]', 'int') AS 'level4',
					Child4.Elm.value('(title)[1]', 'varchar(100)') AS 'term4Title4',
					Child4.Elm.value('(cell)[1]', 'varchar(100)') AS 'term4cell1',
					Child4.Elm.value('(cell)[2]', 'varchar(100)') AS 'term4cell2',
					Child4.Elm.value('(cell)[3]', 'varchar(100)') AS 'term4cell3',
					Child4.Elm.value('(cell)[4]', 'varchar(100)') AS 'term4cell4',
					Child4.Elm.value('(cell)[5]', 'varchar(100)') AS 'term4cell5',
					Child4.Elm.value('(cell)[6]', 'varchar(100)') AS 'term4cell6',

					YEAR(GETDATE()) AS AsofYear,
										--'2016' as AsofYear,
				    GETUTCDATE() as ImportDate,
					CAST(CONVERT(varchar(20),GETUTCDATE(),112) as INT) as ImportDateINT

				FROM [ICD10CodesXMLData]
				CROSS APPLY XMLData.nodes('ICD10CM.index/letter/mainTerm') AS Parent(Elm)
				CROSS APPLY
					Parent.Elm.nodes('term') AS Child1(Elm) 
				CROSS APPLY 
					Child1.Elm.nodes('term') AS Child2(Elm) 
				CROSS APPLY 
					Child2.Elm.nodes('term') AS Child3(Elm) 
				CROSS APPLY 
					Child3.Elm.nodes('term') AS Child4(Elm)  


END