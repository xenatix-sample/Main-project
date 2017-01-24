----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetIncomingOutgoingReferrals]
-- Author:		Scott Martin
-- Date:		03/10/2016
--
-- Purpose:		Gets a list of referrals sorted into Incoming/Outgoing buckets
--
-- Notes:		@View | 1 = Incoming; 2 = Outgoing
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/10/2016	Scott Martin		Initial creation.
-- 03/11/2016	Scott Martin		Refactored query to include search and specify Incoming/Outgoing
-- 05/02/2016	Scott Martin		Incorporated org structure and tweaked incoming/outgoing queries
-- 05/05/2016	Scott Martin		Fix a bug where Un-forwarded referrals weren't showing on incoming view
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetIncomingOutgoingReferrals]
	@SearchCriteria NVARCHAR(1000),
	@UserID BIGINT,
	@View INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#FwdTo') IS NOT NULL
		DROP TABLE #FwdTo

	IF OBJECT_ID('tempdb..#Modified') IS NOT NULL
		DROP TABLE #Modified

	IF OBJECT_ID('tempdb..#IncomingData') IS NOT NULL
		DROP TABLE #IncomingData

	IF OBJECT_ID('tempdb..#OutgoingData') IS NOT NULL
		DROP TABLE #OutgoingData

	CREATE TABLE #FwdTo
	(
		ReferralHeaderID BIGINT,
		ReferralForwardedDetailID BIGINT
	);

	INSERT INTO #FwdTo
	SELECT
		RFD.ReferralHeaderID,
		MAX(ReferralForwardedDetailID)
	FROM
		Registration.ReferralForwardedDetails RFD
	WHERE
		(
			RFD.SendingReferralToID = @UserID
			OR 
			(
				RFD.OrganizationID IN (SELECT UODM.MappingID FROM Core.UserOrganizationDetailsMapping UODM WHERE UODM.UserID = @UserID)
				AND RFD.SendingReferralToID IS NULL
			)
		)
		AND RFD.IsActive = 1
	GROUP BY
		RFD.ReferralHeaderID
	UNION
	SELECT
		RH.ReferralHeaderID,
		NULL
	FROM
		Registration.ReferralHeader RH
		LEFT OUTER JOIN Registration.ReferralForwardedDetails RFD
			ON RH.ReferralHeaderID = RFD.ReferralHeaderID
	WHERE
		RH.CreatedBy = @UserID
		AND RFD.ReferralForwardedDetailID IS NULL;


	CREATE TABLE #Modified
	(
		ReferralHeaderID BIGINT,
		ModifiedOn DATETIME
	);

	INSERT INTO #Modified
	SELECT
		ReferralHeaderID,
		MAX(ModifiedOn)
	FROM
		(
			SELECT
				ReferralHeaderID,
				ModifiedOn
			FROM
				Registration.ReferralHeader
			WHERE
				IsActive = 1
			UNION ALL
			SELECT
				ReferralHeaderID,
				ModifiedOn
			FROM
				Registration.ReferralAdditionalDetails
			WHERE
				IsActive = 1
			UNION ALL
			SELECT
				ReferralHeaderID,
				ModifiedOn
			FROM
				Registration.ReferralDispositionDetails
			WHERE
				IsActive = 1
			UNION ALL
			SELECT
				ReferralHeaderID,
				ModifiedOn
			FROM
				Registration.ReferralForwardedDetails
			WHERE
				IsActive = 1
			UNION ALL
			SELECT
				ReferralHeaderID,
				ModifiedOn
			FROM
				Registration.ReferralReferredToDetails
			WHERE
				IsActive = 1
			UNION ALL
			SELECT
				ReferralHeaderID,
				ModifiedOn
			FROM
				Registration.ReferralOutcomeDetails
			WHERE
				IsActive = 1
		) AS GRP
	GROUP BY
		ReferralHeaderID;

	DECLARE @SearchWords TABLE (Item NVARCHAR(255));

	INSERT INTO @SearchWords
	SELECT
		Item
	FROM
		[Core].[fn_IterativeWordChop] (@SearchCriteria);

	IF NOT EXISTS (SELECT TOP 1 * FROM @SearchWords)
		BEGIN
		INSERT INTO @SearchWords VALUES ('');
		END

	IF @View = 1
		BEGIN
		SELECT
			#FwdTo.ReferralHeaderID,
			RH.ContactID AS HeaderContactID,
			CLI.ContactID,
			CLI.MRN,
			CLI.FirstName,
			CLI.LastName,
			(SELECT TOP 1 P.Number FROM Registration.ContactPhone CP INNER JOIN Core.Phone P ON CP.PhoneID = P.PhoneID LEFT OUTER JOIN Reference.PhonePermission PP ON CP.PhonePermissionID = PP.PhonePermissionID WHERE PP.PhonePermission LIKE '%Yes%' AND CP.IsActive = 1 AND CP.ContactID = CLI.ContactID ORDER BY CP.IsPrimary DESC, CP.ContactPhoneID DESC) AS Contact,
			REQ.FirstName AS RequestorFirstname,
			REQ.LastName AS RequestorLastName,
			(SELECT TOP 1 P.Number FROM Registration.ContactPhone CP INNER JOIN Core.Phone P ON CP.PhoneID = P.PhoneID LEFT OUTER JOIN Reference.PhonePermission PP ON CP.PhonePermissionID = PP.PhonePermissionID WHERE CP.IsActive = 1 AND CP.ContactID = REQ.ContactID ORDER BY CP.IsPrimary DESC, CP.ContactPhoneID DESC) AS RequestorContact,
			RH.ReferralDate AS TransferReferralDate,
			RS.ReferralStatus,
			NULL AS ForwardedTo,
			CASE WHEN U.UserID IS NOT NULL THEN CONCAT(U.FirstName, ' ', U.LastName) ELSE CONCAT(U2.FirstName, ' ', U2.LastName) END AS SubmittedBy,
			RT.ReferralType,
			OSD.Name AS ProgramUnit,
			RFD.ModifiedOn,
			CAST('' AS NVARCHAR(MAX)) AS SearchString
		INTO #IncomingData
		FROM
			#FwdTo
			INNER JOIN Registration.ReferralHeader RH
				ON #FwdTo.ReferralHeaderID = RH.ReferralHeaderID
			LEFT OUTER JOIN Registration.ReferralAdditionalDetails RAD
				ON RH.ReferralHeaderID = RAD.ReferralHeaderID
			LEFT OUTER JOIN Registration.Contact REQ
				ON RH.ContactID = REQ.ContactID
			LEFT OUTER JOIN Registration.Contact CLI
				ON RAD.ContactID = CLI.ContactID
			LEFT OUTER JOIN Reference.ReferralStatus RS
				ON RH.ReferralStatusID = RS.ReferralStatusID
			LEFT OUTER JOIN Registration.ReferralForwardedDetails RFD
				ON #FwdTo.ReferralForwardedDetailID = RFD.ReferralForwardedDetailID
			LEFT OUTER JOIN Core.Users U
				ON RFD.CreatedBy = U.UserID
			LEFT OUTER JOIN Core.Users U2
				ON RH.CreatedBy = U2.UserID
			LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD
				ON RFD.OrganizationID = OSD.MappingID
			LEFT OUTER JOIN Reference.ReferralType RT
				ON RH.ReferralTypeID = RT.ReferralTypeID
		WHERE
			ISNULL(RS.ReferralStatus, '') <> 'Closed'
			AND RH.IsActive = 1

		UPDATE #IncomingData
		SET SearchString = COALESCE(CONVERT(NVARCHAR(10), MRN)  + ':', '') + COALESCE(FirstName + ':', '') + COALESCE(LastName + ':', '') + COALESCE(Contact + ':', '') + COALESCE(ReferralType + ':', '') + COALESCE(RequestorFirstName + ':', '') + COALESCE(RequestorLastName + ':', '') + COALESCE(ProgramUnit + ':', '');

		SELECT
			ReferralHeaderID,
			HeaderContactID,
			ContactID,
			MRN,
			FirstName,
			LastName,
			Contact,
			ReferralType,
			CONCAT(RequestorFirstname, ' ', RequestorLastName) AS RequestorName,
			RequestorContact,
			ProgramUnit,
			TransferReferralDate,
			ReferralStatus,
			ForwardedTo,
			SubmittedBy
		FROM
			#IncomingData RD
			INNER JOIN @SearchWords W
				ON RD.SearchString LIKE '%' + W.Item + '%'
		ORDER BY
			ModifiedOn DESC;
		END

	IF @View = 2
		BEGIN
		SELECT DISTINCT
			RH.ReferralHeaderID,
			RH.ContactID AS HeaderContactID,
			CLI.ContactID,
			CLI.MRN,
			CLI.FirstName,
			CLI.LastName,
			(SELECT TOP 1 P.Number FROM Registration.ContactPhone CP INNER JOIN Core.Phone P ON CP.PhoneID = P.PhoneID LEFT OUTER JOIN Reference.PhonePermission PP ON CP.PhonePermissionID = PP.PhonePermissionID WHERE PP.PhonePermission LIKE '%Yes%' AND CP.IsActive = 1 AND CP.ContactID = CLI.ContactID ORDER BY CP.IsPrimary DESC, CP.ContactPhoneID DESC) AS Contact,
			REQ.FirstName AS RequestorFirstname,
			REQ.LastName AS RequestorLastName,
			(SELECT TOP 1 P.Number FROM Registration.ContactPhone CP INNER JOIN Core.Phone P ON CP.PhoneID = P.PhoneID LEFT OUTER JOIN Reference.PhonePermission PP ON CP.PhonePermissionID = PP.PhonePermissionID WHERE CP.IsActive = 1 AND CP.ContactID = REQ.ContactID ORDER BY CP.IsPrimary DESC, CP.ContactPhoneID DESC) AS RequestorContact,
			RH.ReferralDate AS TransferReferralDate,
			RS.ReferralStatus,
			CASE WHEN U.UserID IS NOT NULL THEN CONCAT(U.FirstName, ' ', U.LastName) ELSE NULL END AS ForwardedTo,
			CONCAT(U2.FirstName, ' ', U2.LastName) AS SubmittedBy,
			RT.ReferralType,
			OSD.Name AS ProgramUnit,
			M.ModifiedOn,
			CAST('' AS NVARCHAR(MAX)) AS SearchString
		INTO #OutgoingData
		FROM
			Registration.ReferralHeader RH
			LEFT OUTER JOIN Registration.ReferralAdditionalDetails RAD
				ON RH.ReferralHeaderID = RAD.ReferralHeaderID
			LEFT OUTER JOIN Registration.Contact REQ
				ON RH.ContactID = REQ.ContactID
			LEFT OUTER JOIN Registration.Contact CLI
				ON RAD.ContactID = CLI.ContactID
			LEFT OUTER JOIN Reference.ReferralStatus RS
				ON RH.ReferralStatusID = RS.ReferralStatusID
			LEFT OUTER JOIN Registration.ReferralDispositionDetails RDD
				ON RH.ReferralHeaderID = RDD.ReferralHeaderID
			INNER JOIN #Modified M
				ON RH.ReferralHeaderID = M.ReferralHeaderID
			LEFT OUTER JOIN Registration.ReferralForwardedDetails RFD
				ON RH.ReferralHeaderID = RFD.ReferralHeaderID
			LEFT OUTER JOIN Core.Users U
				ON RFD.SendingReferralToID = U.UserID
			LEFT OUTER JOIN Core.vw_GetOrganizationStructureDetails OSD
				ON RFD.OrganizationID = OSD.MappingID
			LEFT OUTER JOIN Reference.ReferralType RT
				ON RH.ReferralTypeID = RT.ReferralTypeID
			LEFT OUTER JOIN Core.Users U2
				ON RH.CreatedBy = U2.UserID
			LEFT OUTER JOIN Core.UserOrganizationDetailsMapping UODM
				ON U2.UserID = UODM.UserID
		WHERE
			RH.ReferralHeaderID NOT IN (SELECT ReferralHeaderID FROM #FwdTo)
			AND ISNULL(RS.ReferralStatus, '') <> 'Closed'
			AND RH.IsActive = 1
			AND UODM.MappingID IN (SELECT UODM.MappingID FROM Core.UserOrganizationDetailsMapping UODM WHERE UODM.UserID = @UserID);

		UPDATE #OutgoingData
		SET SearchString = COALESCE(CONVERT(NVARCHAR(10), MRN)  + ':', '') + COALESCE(FirstName + ':', '') + COALESCE(LastName + ':', '') + COALESCE(Contact + ':', '') + COALESCE(ReferralType + ':', '') + COALESCE(RequestorFirstName + ':', '') + COALESCE(RequestorLastName + ':', '') + COALESCE(ProgramUnit + ':', '');

		SELECT
			ReferralHeaderID,
			HeaderContactID,
			ContactID,
			MRN,
			FirstName,
			LastName,
			Contact,
			ReferralType,
			CONCAT(RequestorFirstname, ' ', RequestorLastName) AS RequestorName,
			RequestorContact,
			ProgramUnit,
			TransferReferralDate,
			ReferralStatus,
			ForwardedTo,
			SubmittedBy
		FROM
			#OutgoingData RD
			INNER JOIN @SearchWords W
				ON RD.SearchString LIKE '%' + W.Item + '%'
		ORDER BY
			ModifiedOn DESC;
	END
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO