-----------------------------------------------------------------------------------------------------------------------
-- View:		[vw_GetAppointmentDetails]
-- Author:		Sumana Sangapu
-- Date:		04/11/2016
--
-- Purpose:		Get all appoinments both single and group for all resources and resource types. Returns Group details too for appointments
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/11/2016	Sumana Sangapu	Initial Creation
-- 04/14/2016	Sumana Sangapu	Removed the filter on IsCancelled
-- 04/29/2016   Justin Spalti - Changed the join type for the appointmentresources table and added IsActive to the where clause
-----------------------------------------------------------------------------------------------------------------------

CREATE VIEW [Scheduling].[vw_GetAppointmentDetails]
AS

				SELECT			--AppointmentResource
								AR.AppointmentResourceID as AppointmentResourceID,
								AR.ResourceID as ResourceID,
								AR.ResourceTypeID as ResourceTypeID,
								AR.ParentID as ParentID,

								--Appointment
								A.AppointmentID as AppointmentID,
								A.ProgramID as ProgramID,
								A.FacilityID as FacilityID,
								A.AppointmentTypeID as AppointmentTypeID,
								A.ServicesID as ServicesID,
								S.ServiceName as ServiceName,
								A.ServiceStatusID as ServiceStatusID,
								A.AppointmentDate as AppointmentDate,
								A.AppointmentStartTime as AppointmentStartTime,
								A.AppointmentLength as AppointmentLength,
								A.SupervisionVisit as SupervisionVisit,
								A.ReferredBy as ReferredBy,
								A.ReasonForVisit as ReasonForVisit,
								A.RecurrenceID as RecurrenceID,
								A.IsCancelled as IsCancelled,
								A.CancelReasonID as CancelReasonID,
								A.CancelComment as CancelComment,
								A.Comments as Comments,
								A.IsInterpreterRequired as IsInterpreterRequired,
								A.NonMHMRAppointment as NonMHMRAppointment,
								A.IsActive as IsActive,
								A.ModifiedBy as ModifiedBy,
								A.ModifiedOn as ModifiedOn,

								A.IsGroupAppointment as IsGroupAppointment,

								--AppointmentType
								AT.IsBlocked as IsBlocked,
								AT.AppointmentType as AppointmentType,

								-- Group Details
								gd.GroupDetailID as GroupDetailID,
								gh.GroupHeaderID as GroupHeaderID,
								gh.Comments as GroupComments,
								gd.GroupName as GroupName,
								gd.GroupTypeID as GroupTypeID,
								gt.GroupType as GroupType,

								-- AppointmentStatus
								ApptStatus.AppointmentStatusID,
								ast.AppointmentStatus as AppointmentStatus,

								-- Facility and Program
								P.Name as ProgramName,
								F.FacilityName as FacilityName

					FROM	Scheduling.Appointment a
					INNER JOIN Scheduling.AppointmentResource ar
					ON		ar.AppointmentID = a.AppointmentID  
					LEFT JOIN [Scheduling].[GroupSchedulingHeader] gh
					ON		ar.GroupHeaderID = gh.GroupHeaderID 
					LEFT JOIN [Scheduling].[GroupDetails] gd
					ON		gh.GroupDetailID = gd.GroupDetailID 
					LEFT JOIN [Core].[vw_GetOrganizationStructureDetails] P
					ON		a.ProgramID = P.MappingID 
					LEFT JOIN Reference.Facility f
					ON		f.FacilityID = a.FacilityID
					LEFT JOIN Scheduling.AppointmentType at
					ON		a.AppointmentTypeID = at.AppointmentTypeID
					LEFT JOIN Reference.[Services] s
					ON		a.ServicesID = s.ServicesID
					LEFT JOIN Reference.GroupType gt
					ON		gd.GroupTypeID = gt.GroupTypeID
					OUTER APPLY (SELECT TOP 1 S.AppointmentStatusID 
								 FROM Scheduling.AppointmentStatusDetails S
								 WHERE S.AppointmentResourceID = AR.AppointmentResourceID 
								 AND S.IsActive = 1 ORDER BY S.ModifiedOn DESC) AS ApptStatus
					LEFT JOIN Reference.AppointmentStatus ast
					ON		ApptStatus.AppointmentStatusID = ast.AppointmentStatusID
					WHERE	a.IsActive = 1
						AND ar.IsActive = 1
				--	AND		a.IsCancelled = 0 -- NotCancelled 

GO

