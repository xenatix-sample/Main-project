-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_GetClientSearchResults]
-- Author:		Sumana Sangapu
-- Date:		09/24/2015
--
-- Purpose:		Search the results of the contacts
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Highly dependent on Registration.vw_ContactAdmissionDischarge
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/24/2015	Sumana Sangapu  TFS# 1511 - Initial creation.
-- 10/01/2015	Suresh Pandey	Added ContactTypeID in select list to verify contact type in UI.
-- 10/27/2015	Sumana Sangapu	3029 Added IsPrimary filter to ContactAddress table
-- 01/05/2016   Justin Spalti   Added ClientTypeID to the result set
-- 01/08/2015	Vishal Joshi 	Removed AlternateID and ClientIndentifierTypeID
-- 02/01/2016	Sumana Sangapu	Optimised the search proc
-- 02/09/2016	Rajiv Ranjan	Able to search all contact type if @ContactType is null/blank
-- 03/09/2016	Sumana Sangapu	Refactor Search results using FullTextSearch on SearchableFields
-- 04/25/2016	Arun Choudhary	Division populated to the search grid
-- 05/04/2016	Arun Choudhary	Added ClientTypeID to the return object (needed for some methods to work)
-- 06/29/2016	Kyle Campbell	TFS #9982	Modified to return contacts admitted to user PUs and also contacts only admitted at company level
-- 07/20/2016	Kyle Campbell	TFS #12484	Trim leading space from DOB in #WordToSe
-- 07/23/2016	Sumana SAngapu  TFS #12577	Return contacts when user is not assigned any programunits
-- 08/03/2016	Scott Martin	Added logic for searching based on a specific field (SSN, DOB, DL, and MRN)
-- 09/07/2016	Scott Martin	Modified the advanced search logic and messages: Made SSN searchable on encrypted value, removed SSN length of 4, made DL a like match, changed the error message for MRN and SSN
-- 09/12/2016	Deepak Kumar	Modified the advanced search logic for SSN to search last 4 digits or complete 9 digits
-- 09/12/2016	Kyle Campbell	TFS #14454	Modify proc to return all search results regardless of program unit assignments
-- 09/16/2016	Scott Martin	SSN length was 10 instead of 9
-- 09/19/2016	Scott Martin	Length of SSN was based on converted INT value and not string value which caused preceeding 0's to drop off
-- 11/08/2016	Scott Martin	Added WITH(NOLOCK) to Contact table and removed ContactAdmissionDischarge temp table
-- 11/10/2016	Scott Martin	Refactored how the search results are color coded for better performance
-- 11/11/2016	Scott Martin	Fixed an issue where SSN=NNNNNNNNN was not returning results
-- 12/30/2016	Scott Martin	Added fields for client merge
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetClientSearchResults]
	@SearchCriteria nvarchar(1000),
	@ContactType nvarchar(50),
	@UserID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN	
	BEGIN TRY
		SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed successfully' 	
		 
		DECLARE @WordCount int
	
		--The table that maps customer data, holding each word, and its location
		CREATE TABLE #WordOccurence
		(
			ContactID	BIGINT not null,
			Item		VARCHAR(255) not null ,
			SortOrder	BIGINT not null
		)

		-- Create Table to store results

		CREATE TABLE #tmpContactData
		(
			[ContactID] [bigint] NOT NULL,
			[ContactTypeID] [int] NULL,
			[ClientTypeText] [nvarchar](100) NULL,
			[SearchableFields] [nvarchar](987) NOT NULL
		)

		DECLARE @string TABLE ( c1 NVARCHAR(100))

		-- Holds the words to look for
		CREATE TABLE #WordsToLookUp
		(
			Item VARCHAR(255),
			SearchItem VARCHAR(255),
			SortOrder INT PRIMARY KEY
		)

		DECLARE @SearchCriteriaOR nvarchar(1000),
				@SearchTag nvarchar(5),
				@UseSearchTagFilter BIT = 0,
				@DOB DATE,
				@MRN BIGINT,
				@SSNLen INT,
				@SSNLastFour NVARCHAR(4)

		SET @SearchTag = LEFT(@SearchCriteria, 4)

		IF @SearchTag = 'SSN='
			BEGIN
			SET @UseSearchTagFilter = 1;
			SET @SearchCriteria = REPLACE(@SearchCriteria, @SearchTag, '');
			
			BEGIN TRY
			DECLARE @SSN BIGINT;
			SET @SSN = CONVERT(BIGINT, @SearchCriteria);
			SET @SSNLen = LEN(@SearchCriteria);

			IF @SSNLen IN (4, 9)				
				BEGIN		
				SET @SSNLastFour = RIGHT(@SearchCriteria, 4);
				END
			ELSE
				BEGIN
				RAISERROR('Incorrect SSN format', 16, 1)
				END
			END TRY

			BEGIN CATCH
			SELECT @ResultCode = -1,
					@ResultMessage = 'Please use the correct format for your search criteria: SSN=NNNNNNNNN or SSN=NNNN';
			RETURN
			END CATCH
			END
		
		IF @SearchTag = 'DOB='
			BEGIN
			SET @UseSearchTagFilter = 1;
			SET @SearchCriteria = REPLACE(@SearchCriteria, @SearchTag, '');

			BEGIN TRY
			SET @DOB = CONVERT(DATE, @SearchCriteria, 101);

			IF ISNULL(@DOB, '') = ''
				BEGIN
				RAISERROR ('No Date', 16, 1);
				END
			END TRY
			BEGIN CATCH
			SELECT @ResultCode = -1,
					@ResultMessage = 'Please use the correct format for your search criteria: DOB=MM/DD/YYYY';
			RETURN
			END CATCH
			END

		IF @SearchTag = 'DL#='
			BEGIN
			SET @UseSearchTagFilter = 1;
			SET @SearchCriteria = REPLACE(@SearchCriteria, @SearchTag, '');
			END

		IF @SearchTag = 'MRN='
			BEGIN
			SET @UseSearchTagFilter = 1;
			SET @SearchCriteria = REPLACE(@SearchCriteria, @SearchTag, '');

			BEGIN TRY
			SET @MRN = CONVERT(BIGINT, @SearchCriteria);
			SET @SearchCriteria = CAST(@MRN AS NVARCHAR(16));

			IF ISNULL(@MRN, 0) = 0
				BEGIN
				RAISERROR ('No MRN', 16, 1);
				END
			END TRY
			BEGIN CATCH
			SELECT @ResultCode = -1,
					@ResultMessage = 'Please use the correct format for your search criteria: MRN=NNNNNNNNN';
			RETURN
			END CATCH
			END

		-- Generate the string to facilitate partial searches
		INSERT INTO @string
		SELECT CONCAT ( '"*', Items ,'*"') FROM Core.fn_Split (@SearchCriteria, ' ') 
			
		SELECT @SearchCriteriaOR = COALESCE(@SearchCriteriaOR+' OR ' , '') + c1 FROM @string

		--create temp table to just store contacts that match search criteria
		EXEC Core.usp_OpenEncryptionkeys @ResultCode OUTPUT, @ResultMessage OUTPUT

		INSERT INTO #tmpContactData
		(
			[ContactID],
			[ContactTypeID],
			[ClientTypeText],
			[SearchableFields]
		)		   
		SELECT
			c.ContactID as ContactID,
			c.ContactTypeID,
			OD.Name AS ClientTypeText,
			CONCAT(':', SearchableFields, OD.Name, ':') AS SearchableFields
		FROM
			Registration.Contact c WITH(NOLOCK)
			LEFT OUTER JOIN  Reference.ClientType CT
				ON c.ClientTypeID = CT.ClientTypeID
			LEFT OUTER JOIN Core.OrganizationDetails OD
				ON CT.OrganizationDetailID = OD.DetailID
		WHERE
			c.isActive=1
			AND
			(
				(CONTAINS(SearchableFields, @SearchCriteriaOR) AND @UseSearchTagFilter = 0)
				OR (@UseSearchTagFilter = 1 AND @SearchTag = 'SSN=' AND SSN = @SearchCriteria AND @SSNLen = 4)
				OR (@UseSearchTagFilter = 1 AND @SearchTag = 'SSN=' AND @SSNLen = 9 AND Core.fn_Decrypt(SSNEncrypted) = @SearchCriteria AND SSN = @SSNLastFour)
				OR (@UseSearchTagFilter = 1 AND @SearchTag = 'DOB=' AND DOB = @DOB)
				OR (@UseSearchTagFilter = 1 AND @SearchTag = 'DL#=' AND DriverLicense LIKE '%' + @SearchCriteria + '%')
				OR (@UseSearchTagFilter = 1 AND @SearchTag = 'MRN=' AND MRN = @MRN)
			)
			AND ContactTypeID IN ( SELECT CAST(Items AS INT) FROM Core.fn_Split (@ContactType, ','))

		-- Split the search criteria string into rows in a table
		INSERT  INTO #WordsToLookUp (Item, SearchItem, SortOrder)
		SELECT	Item, '%'+Item+'%', SortOrder FROM  [Core].[fn_IterativeWordChop] (@SearchCriteria) WHERE Item IS NOT NULL AND Item <> ''

		SELECT @WordCount = COUNT(*) FROM #WordsToLookUp
		
		/******************************************* STEP 3 ***********************************************************************************/
		-- Based on the result set, determine the possible exact match 
		CREATE TABLE #Matches
		(
			ContactID BIGINT,
			ClientTypeText NVARCHAR(200),
			Color NVARCHAR(10)
		);

		WITH Match_CTE (ContactID, ClientTypeText, PartialMatch, ExactMatch)
		AS
		(
			SELECT
				ContactID,
				ClientTypeText,
				CASE
					WHEN (LEN(tCD.SearchableFields) - LEN(REPLACE(tCD.SearchableFields, WLU.Item, ''))) / LEN(WLU.Item) > 0 THEN 1
					ELSE 0 END AS PartialMatch,
				CASE
					WHEN CHARINDEX(CONCAT(':', WLU.Item, ':'), tCD.SearchableFields) > 0 AND @SearchTag <> 'SSN=' THEN 1
					WHEN CHARINDEX(CONCAT(':', WLU.Item, ':'), tCD.SearchableFields) > 0 AND @SearchTag = 'SSN=' AND @SSNLen = 4 THEN 1
					WHEN CHARINDEX(CONCAT(':', RIGHT(WLU.Item, 4), ':'), tCD.SearchableFields) > 0 AND @SearchTag = 'SSN=' AND @SSNLen = 9 THEN 1
					ELSE 0 END AS ExactMatch
			FROM
				#WordsToLookUp WLU
				INNER JOIN #tmpContactData tCD
					ON 
					(
						tCD.SearchableFields like WLU.SearchItem
						OR @SearchTag = 'SSN=' AND @SSNLen = 9 AND tcD.SearchableFields LIKE CONCAT('%', @SSNLastFour, '%')
					)
		)
		INSERT INTO #Matches
		SELECT
			ContactID,
			ClientTypeText,
			CASE
				WHEN SUM(PartialMatch) = 1 AND SUM(ExactMatch) = 0 THEN 'Red'
				WHEN SUM(ExactMatch) = @WordCount THEN 'Green'
				ELSE 'Orange' END			
		FROM
			Match_CTE
		GROUP BY
			ContactID,
			ClientTypeText

		-- Return the final result set. 
		SELECT TOP 50
			M.ContactID,
			c.ContactTypeID,
			c.[FirstName],
			c.ClientTypeID,
			ClientTypeText,
			c.Middle,
			c.LastName,
			c.DOB,
			c.SSN,
			c.DriverLicense,
			c.PreferredName,
			c.MRN,
			Color,
			Gender AS ContactGenderText,
			Suffix,
			LTRIM(STUFF((SELECT DISTINCT ',' + CAST(OSD.MappingID AS NVARCHAR(10)) FROM Registration.vw_ContactAdmissionDischarge vCAD INNER JOIN Core.vw_GetOrganizationStructureDetails OSD ON vCAD.ProgramUnitID = OSD.MappingID WHERE ContactID = M.ContactID AND vCAD.DataKey = 'ProgramUnit' FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'), 1, 1, '')) AS ProgramUnit,
			CAST(CASE WHEN MRN.MRN IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsMerged,
			MRN.MRN AS MergedMRN
		FROM 
			#Matches M
			INNER JOIN Registration.Contact c
				ON c.ContactID = M.ContactID
			LEFT OUTER JOIN Reference.Suffix sf
				ON sf.SuffixID = c.SuffixID        
			LEFT OUTER JOIN Reference.Gender g
				ON g.GenderID = c.GenderID
			LEFT OUTER JOIN Core.MergedContactsMapping MCM
				ON C.ContactID = MCM.ContactID
			LEFT OUTER JOIN Registration.ContactMRN MRN
				ON MCM.ChildID = MRN.ContactID
		ORDER BY Color asc
		
		-- Drop temp tables
		DROP TABLE #WordOccurence
	
		DROP TABLE #WordsToLookUp

		DROP TABLE #tmpContactData

		DROP TABLE #Matches
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END