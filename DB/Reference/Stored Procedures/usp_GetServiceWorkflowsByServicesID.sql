-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_GetServiceWorkflowsByServicesID]
-- Author:		Kyle Campbell
-- Date:		01/04/2017
--
-- Purpose:		Get Module Components (Service Workflows) and Details for Service Workflows grid in Service Configuration
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/04/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE Reference.usp_GetServiceWorkflowsByServicesID
	@ServicesID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	AS
BEGIN
	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
		SELECT 
			SMC.ServicesModuleComponentID, 
			SMC.ServicesID, 
			MC.ModuleComponentID, 
			F.Name As ServiceWorkflow,
			STUFF((SELECT DISTINCT ',' + DeliveryMethod FROM Reference.DeliveryMethod DM INNER JOIN Reference.DeliveryMethodModuleComponent DMMC ON DM.DeliveryMethodID = DMMC.DeliveryMethodID WHERE DMMC.ModuleComponentID = SMC.ModuleComponentID AND DMMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As DeliveryMethod,
			STUFF((SELECT DISTINCT ',' + ServiceLocation FROM Reference.ServiceLocation SL INNER JOIN Reference.ServiceLocationModuleComponent SLMC ON SL.ServiceLocationID = SLMC.ServiceLocationID WHERE SLMC.ModuleComponentID = SMC.ModuleComponentID AND SLMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As PlaceOfService,
			STUFF((SELECT DISTINCT ',' + CodeDescription FROM Reference.RecipientCode RC INNER JOIN Reference.RecipientCodeModuleComponent RCMC ON RC.CodeID = RCMC.RecipientCodeID WHERE RCMC.ModuleComponentID = SMC.ModuleComponentID AND RCMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As Recipient,
			STUFF((SELECT DISTINCT ',' + ServiceStatus FROM Reference.ServiceStatus SS INNER JOIN Reference.ServiceStatusModuleComponent SSMC ON SS.ServiceStatusID = SSMC.ServiceStatusID WHERE SSMC.ModuleComponentID = SMC.ModuleComponentID AND SSMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As ServiceStatus,
			STUFF((SELECT DISTINCT ',' + AttendanceStatus FROM Reference.AttendanceStatus [AS] INNER JOIN Reference.AttendanceStatusModuleComponent ASMC ON [AS].AttendanceStatusID = ASMC.AttendanceStatusID WHERE ASMC.ModuleComponentID = SMC.ModuleComponentID AND ASMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As AttendanceStatus,
			STUFF((SELECT DISTINCT ',' + TrackingField FROM Reference.TrackingField TF INNER JOIN Reference.TrackingFieldModuleComponent TFMC ON TF.TrackingFieldID = TFMC.TrackingFieldID WHERE TFMC.ModuleComponentID = SMC.ModuleComponentID AND TFMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As TrackingField,
			STUFF((SELECT DISTINCT ',' + CredentialName FROM Reference.Credentials C INNER JOIN Reference.CredentialModuleComponent CMC ON C.CredentialID = CMC.CredentialID WHERE CMC.ModuleComponentID = SMC.ModuleComponentID AND CMC.ServicesID = SMC.ServicesID FOR XML PATH(''), ROOT ('List'), type).value('/List[1]','NVARCHAR(MAX)'),1,1,'') As [Credentials],
			SMC.IsActive
		FROM Reference.ServicesModuleComponent SMC 
			INNER JOIN Core.ModuleComponent MC ON SMC.ModuleComponentID = MC.ModuleComponentID
			INNER JOIN Core.Feature F ON MC.FeatureID = F.FeatureID
		WHERE SMC.ServicesID = @ServicesID
		ORDER BY SMC.IsActive DESC, F.Name ASC
	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END