-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetReferralQueue]
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
-- 12/15/2015    TFS#4469        Created by John Crossen
-- 12/16/2015	 Gurpreet Singh	 Added IsActive check
-- 12/22/2015	 Gurpreet Singh	 Fixed data not showing for Referral Search
-- 12/30/2015	 Satish Singh	 Show Top 1 phone for a contact
-- 01/04/2015	Gurpreet Singh	Added ContactID, get only records for Referral Requestor
-- 12/01/2016   Lokesh Singhal   Added HeaderContactID
-- 13/01/2016   Lokesh Singhal   Remove requestor from referral search when client have been registered 
--01/14/2016	Gurpreet Singh	Added contact phone Number, referral date to output
---------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_GetReferralQueue]
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN	
	BEGIN TRY
		SELECT
			@ResultCode = 0,
			@ResultMessage = 'Executed successfully'

			SELECT	RH.ReferralHeaderID,
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
						P.ProgramName AS Program,
						--RD.TransferredInDate AS TransferReferralDate,
						rs.ReferralStatus,
						RD.[ContactID],
						RH.ContactID as HeaderContactID,
						RH.ReferralDate AS TransferReferralDate
			FROM 		Registration.ReferralHeader RH 
			JOIN 		Registration.Contact c  ON RH.ContactID=c.ContactID
			JOIN		Reference.ReferralType rt ON rt.ReferralTypeID=rh.ReferralTypeID
			JOIN		Reference.ReferralStatus rs ON RS.ReferralStatusID=RH.ReferralStatusID
			LEFT OUTER	JOIN Registration.ReferralAdditionalDetails RD ON RH.ReferralHeaderID=RD.ReferralHeaderID
			LEFT OUTER JOIN Reference.Program P ON P.ProgramID=RH.ProgramID
			LEFT OUTER JOIN	Registration.ReferralContact rc ON rc.ContactID=c.ContactID
			LEFT OUTER JOIN	Registration.Contact ct ON ct.ContactID = rc.ContactID
			LEFT OUTER JOIN	Registration.Contact rdc ON rdc.ContactID = rd.ContactID 
			WHERE rs.ReferralStatus <>'Closed' AND RH.IsActive = 1 AND c.ContactTypeID = 7
			AND isnull(RD.[ContactID],0)  = CASE rdc.ContactTypeID WHEN 1 THEN -1 ELSE isnull(RD.[ContactID],0) END
			
		END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END