-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[fn_GetBenefitsAssistanceBenefitTypes]
-- Author:		Scott Martin
-- Date:		11/10/2016
--
-- Purpose:		Function to pull Benefits Assistance benefit types
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/10/2016	Scott Martin	Initial Creation
-- 11/22/2016	Scott Martin	Changed the QuestionID for prescriptions
-- 12/27/2016	Scott Martin	Merged code from 1.1.4 update script
-----------------------------------------------------------------------------------------------------------------------

CREATE FUNCTION Registration.fn_GetBenefitsAssistanceBenefitTypes ()
	RETURNS @BT TABLE
	(
		BenefitType nvarchar(100),
		QuestionID bigint,
		SortOrder int
	)
AS
BEGIN
	INSERT INTO @BT
	(
		BenefitType,
		QuestionID,
		SortOrder
	)
	VALUES
	('Benefits Screening',1801,1),
	('Fee Assessment',1812,2),
	('Benefits Research',1817,3),
	('SSI Application',1819,4),
	('SSI Appeals',1834,5),
	('SSDI Application',1836,6),
	('SSDI Appeals',1851,7),
	('Medicare Application (MAO/DAC/QMB/MQMB)',1853,8),
	('Medicare Appeals',1855,9),
	('Medicare D Enrollment',1856,10),
	('Prescription Drug Plan',1862,11),
	('Traditional Medicaid',1867,12),
	('Traditional Children''s Medicaid Application',1901,13),
	('SNAP Application',1919,14),
	('CHIP Application',1924,15),
	('JPS Connection Programs',1942,16),
	('TANF Application',1947,17),
	('Other',1952,18);
	RETURN
END