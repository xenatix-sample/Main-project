-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReferralSearchResults]
-- Author:		John Crossen
-- Date:		12/15/2015
--
-- Purpose:		Search Referral Information
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/15/2015   TFS#4469        Created by John Crossen
-- 12/18/2015	Gurpreet Singh	Fixed exceptions in sproc
-- 12/22/2015	Gurpreet Singh	Fixed data not showing for Referral Search
-- 12/30/2015	Satish Singh	Show Top 1 phone for a contact
-- 01/04/2015	Gurpreet Singh	Added ContactID, get only records for Referral Requestor
-- 01/08/2016   Justin Spalti   Removed the References to TransferReferralDate
-- 12/01/2016   Lokesh Singhal   Added HeaderContactID
-- 13/01/2016   Lokesh Singhal   Remove requestor from referral search when client have been registered 
--01/14/2016	Gurpreet Singh	Added contact phone Number, referral date to output
-- 04/18/2016	Scott Martin	Changed sort order to TransferReferralDate desc
---------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Registration].[usp_GetReferralSearchResults]
	@SearchCriteria nvarchar(1000),
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
			ReferralHeaderID	INT NOT NULL,
			Item		VARCHAR(255) NOT NULL ,
			Location	INT NOT NULL,
			SortOrder	INT NOT NULL
		)

		-- Holds the words to look for
		CREATE TABLE #WordsToLookUp (	Item VARCHAR(255), 
										Location INT, 
										SortOrder INT PRIMARY KEY)

		-- Retrieve the result set 
			SELECT		RH.ReferralHeaderID,
						ct.MRN AS MRN,
						NULL AS TKIDSID, 
						rdc.[FirstName] AS FirstName,  
						rdc.[LastName] AS LastName,
						(SELECT TOP 1 ph.Number FROM Core.Phone ph join Registration.ContactPhone cp ON ph.PhoneID = cp.PhoneID WHERE cp.ContactID = RD.ContactID and ph.IsActive=1
						and cp.IsActive=1) AS Contact,
						(SELECT TOP 1 ph.Number FROM Core.Phone ph join Registration.ContactPhone cp ON ph.PhoneID = cp.PhoneID WHERE cp.ContactID = c.ContactID and ph.IsActive=1
						and cp.IsActive=1) AS RequestorContact,
						rt.ReferralType,
						c.FirstName +' '+c.LastName AS RequestorName,
						OSD.Name AS OrganizationName,
						--RD.TransferredInDate AS TransferReferralDate,
						rs.ReferralStatus,
						RD.[ContactID],
						RH.ContactID as HeaderContactID,
						RH.ReferralDate AS TransferReferralDate
			INTO		#ContactData
			FROM 		Registration.ReferralHeader RH 
			JOIN 		Registration.Contact c  ON RH.ContactID=c.ContactID
			JOIN		Reference.ReferralType rt ON rt.ReferralTypeID=rh.ReferralTypeID
			JOIN		Reference.ReferralStatus rs ON RS.ReferralStatusID=RH.ReferralStatusID
			JOIN 		Registration.Contact F  ON RH.ContactID=F.ContactID
			LEFT OUTER	JOIN Registration.ReferralAdditionalDetails RD ON RH.ReferralHeaderID=RD.ReferralHeaderID
			LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD ON OSD.MappingID=RH.OrganizationID
			LEFT OUTER JOIN	Registration.ReferralContact rc ON rc.ContactID=c.ContactID
			LEFT OUTER JOIN	Registration.Contact ct ON ct.ContactID = rc.ContactID 
			LEFT OUTER JOIN	Registration.Contact rdc ON rdc.ContactID = rd.ContactID 
			WHERE RH.IsActive = 1 AND c.ContactTypeID = 7
			AND isnull(RD.[ContactID],0)  = CASE rdc.ContactTypeID WHEN 1 THEN -1 ELSE isnull(RD.[ContactID],0) END
			--WHERE rs.ReferralStatus <>'Closed' AND RH.IsActive = 1 AND c.ContactTypeID = 7
			


		
		--WHERE c.ContactTypeID IN ( SELECT * FROM Core.fn_Split (@ContactType, ','))
		

		/********************************************** STEP 1 ******************************************************************************/
		/* Map the text value of each row into the #WordOccurence table, recording the location of each word */
		INSERT INTO  #WordOccurence
		SELECT ReferralHeaderID, Item, Location, SortOrder
		FROM
		(		SELECT		ReferralHeaderID, (coalesce(c.[FirstName]+' ' ,'')+  coalesce( c.[LastName]+ ' ','') + coalesce(convert(nvarchar(10),c.MRN )+' ','')   + coalesce(CONVERT(NVARCHAR(25), c.Contact) + ' ','') +
							coalesce(c.ReferralType+' ','')+ coalesce(c.RequestorName+ ' ','') + coalesce(c.OrganizationName+ ' ','') +
							coalesce(convert(nvarchar(20),c.MRN )+' ','')) as ContactString
				FROM 		#ContactData c

		)f
		CROSS APPLY  [Core].[fn_IterativeWordChop](ContactString)

		/********************************************** STEP 2 ******************************************************************************/
		-- Split the search criteria string into rows in a table
		INSERT  INTO #WordsToLookUp (Item,Location,SortOrder)
		SELECT	'%'+Item+'%', Location, SortOrder FROM  [Core].[fn_IterativeWordChop] (@SearchCriteria)

		SELECT @WordCount = COUNT(*) FROM #WordsToLookUp

		/******************************************* STEP 3 ***********************************************************************************/
		-- Based on the result set, determine the possible exact match 

		-- Least match with just 1 word or 1 letter 
		SELECT	*,'Red' as Color
		into    #LeastMatchRed
		FROM	#ContactData
		WHERE	ReferralHeaderID  IN (	SELECT ReferralHeaderID 
									FROM (	SELECT	ReferralHeaderID, WLU.SortOrder 
											from  #WordsToLookUp WLU 
											INNER JOIN #WordOccurence CWO 
											ON		 CWO.Item like WLU.Item   
											GROUP BY ReferralHeaderID,WLU.SortOrder)AllWordMatches
									GROUP BY ReferralHeaderID having count(*) = 1
									)

		-- Partial matching 
		SELECT	*,'Orange' as Color
		into    #PotentialMatchOrange
		FROM	#ContactData
		WHERE	ReferralHeaderID  IN (	SELECT ReferralHeaderID 
									FROM (	SELECT	ReferralHeaderID, WLU.SortOrder 
											from  #WordsToLookUp WLU 
											INNER JOIN #WordOccurence CWO 
											ON		 CWO.Item like WLU.Item   
											GROUP BY ReferralHeaderID,WLU.SortOrder)AllWordMatches
									GROUP BY ReferralHeaderID  -- having count(*)= 1
									)

		-- Exact Match with all the search criteria
		SELECT	*,'Green' as Color
		into    #ExactMatchGreen
		FROM	#ContactData
		WHERE	ReferralHeaderID  IN (	SELECT ReferralHeaderID 
									FROM (	SELECT	ReferralHeaderID, WLU.SortOrder 
											from  #WordsToLookUp WLU 
											INNER JOIN #WordOccurence CWO 
											ON		 CWO.Item = replace( WLU.Item  ,'%','')
											GROUP BY ReferralHeaderID,WLU.SortOrder)AllWordMatches
									GROUP BY ReferralHeaderID  having count(*)= @WordCount
									)



		-- Return the final result set. 
		SELECT 	ReferralHeaderID, [FirstName],	[LastName],Contact, ReferralType,RequestorName,OrganizationName,ReferralStatus, ContactID,HeaderContactID, TransferReferralDate, RequestorContact
		FROM (
				SELECT o.* FROM #PotentialMatchOrange o   
				WHERE o.ReferralHeaderID NOT IN ( SELECT ReferralHeaderID FROM #LeastMatchRed g )
				AND o.ReferralHeaderID not in (Select ReferralHeaderID from #ExactMatchGreen g)
				UNION
				SELECT r.* FROM #LeastMatchRed r   
				WHERE r.ReferralHeaderID NOT IN ( SELECT ReferralHeaderID FROM #ExactMatchGreen g )
				UNION
				SELECT g.* FROM #ExactMatchGreen g    
			) Result
		ORDER BY
			TransferReferralDate DESC

		-- Drop temp tables
		IF  OBJECT_ID(N'#WordOccurence') IS NOT NULL DROP TABLE #WordOccurence
	
		IF  OBJECT_ID(N'#WordsToLookUp') IS NOT NULL DROP TABLE #WordsToLookUp
		
		IF  OBJECT_ID(N'#ContactData') IS NOT NULL DROP TABLE #ContactData
	
		IF  OBJECT_ID(N'#ExactMatchGreen') IS NOT NULL DROP TABLE #ExactMatchGreen

		IF  OBJECT_ID(N'#PotentialMatchOrange') IS NOT NULL DROP TABLE #PotentialMatchOrange

		IF  OBJECT_ID(N'#LeastMatchRed') IS NOT NULL DROP TABLE #LeastMatchRed

	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END