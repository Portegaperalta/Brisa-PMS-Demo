using System;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class HouseKeepingTask
{
    public Guid Id { get; }
    public Guid RoomId { get; init; }
    public Guid AssignedBy { get; init; }
    public Guid AssignedTo { get; private set; }
    public HouseKeepingTaskType Type { get; private set; }
    public TaskPriority Priority { get; private set; }
    public HotelTaskStatus Status { get; private set; }
    public string? Notes { get;  private set; }
    public TaskExpectedTimeInterval ExpectedTimeInterval { get; private set; }
    public TaskActualTimeInterval? ActualTimeInterval { get; private set; }
    public bool IncidentReported { get; private set; }
    public string? IncidentDescription { get; private set; }

    public HouseKeepingTask
    (
        Guid roomId,
        Guid assignedTo,
        Guid assignedBy,
        HouseKeepingTaskType type,
        TaskPriority priority,
        TaskExpectedTimeInterval  expectedTimeInterval,
        string? notes = null
    )
    {
        if (roomId == Guid.Empty)
            throw new EmptyRequiredFieldException("Room Id");
        
        if (assignedTo == Guid.Empty)
            throw new EmptyRequiredFieldException("Assigned To");
        
        if (assignedBy == Guid.Empty)
            throw new EmptyRequiredFieldException("Assigned By");
        
        if (Enum.IsDefined<HouseKeepingTaskType>(type) is false)
            throw new BusinessRuleException("Invalid houseKeeping task type");
        
        if (Enum.IsDefined<TaskPriority>(priority) is false)
            throw new  BusinessRuleException("Invalid task priority");

        Id = Guid.CreateVersion7();
        RoomId = roomId;
        AssignedBy = assignedBy;
        AssignedTo = assignedTo;
        Type = type;
        Priority = priority;
        Status = HotelTaskStatus.Pending;
        Notes = notes;
        ExpectedTimeInterval = expectedTimeInterval;
        ActualTimeInterval = null;
        IncidentReported = false;
        IncidentDescription = null;
    }

    public void ChangeAssignedTo(Guid newAssignedTo)
    {
        if (newAssignedTo == Guid.Empty)
            throw new EmptyRequiredFieldException("Assigned To");

        AssignedTo = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task can't be reassigned"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task can't be reassigned"),
            _ => newAssignedTo
        };
    }

    public void ChangeTaskType(HouseKeepingTaskType newType)
    {
        if (Enum.IsDefined<HouseKeepingTaskType>(newType) is false)
            throw new BusinessRuleException("Invalid task type");

        Type = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task type can't be modified"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task type can't be modified"),
            _ => newType
        };
    }

    public void ChangePriority(TaskPriority newPriority)
    {
        if (Enum.IsDefined<TaskPriority>(newPriority) is false)
            throw new BusinessRuleException("Invalid task priority");

        Priority = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task priority can't be modified"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task priority can't be modified"),
            _ => newPriority
        };
    }

    public void UpdatedStatus(HotelTaskStatus newHotelTaskStatus)
    {
        if (Enum.IsDefined<HotelTaskStatus>(newHotelTaskStatus) is false)
            throw new BusinessRuleException("Invalid hotel task status");

        Status = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task status can't be modified"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task status can't be modified"),
            _ => newHotelTaskStatus
        };
    }
    
    public void UpdatedNotes(string? notes) =>  Notes = notes;

    public void ChangeExpectedTimeInterval(TaskExpectedTimeInterval newExpectedTimeInterval)
    {
        ExpectedTimeInterval = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task expected time interval can't be modified"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task expected time interval can't be modified"),
            _ => newExpectedTimeInterval
        };
    }

    public void StartActualTimeInterval()
    {
        ActualTimeInterval = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task actual time interval can't be modified"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task actual time interval can't be modified"),
            _ => new TaskActualTimeInterval(actualStartAt: DateTime.UtcNow, actualEndAt:DateTime.MinValue)
        };
    }

    public void EndActualTimeInterval(TaskActualTimeInterval currentActualTimeInterval)
    {
        ActualTimeInterval = Status switch
        {
            HotelTaskStatus.Completed => throw new BusinessRuleException("Completed task actual time interval can't be modified"),
            HotelTaskStatus.Cancelled => throw new BusinessRuleException("Cancelled task actual time interval can't be modified"),
            _ => new TaskActualTimeInterval(actualStartAt: currentActualTimeInterval.ActualStartAt, actualEndAt: DateTime.UtcNow)
        };
    }

    public void ReportIncident(string incidentDescription)
    {
        if (string.IsNullOrWhiteSpace(incidentDescription))
            throw new EmptyRequiredFieldException("Incident Description");
        
        IncidentReported = true;
        IncidentDescription = incidentDescription;
    }

    public void UpdateIncidentDescription(string newIncidentDescription)
    {
        if (IncidentReported is false)
            throw new BusinessRuleException("An incident needs to be reported to modify its description");
        
        if (string.IsNullOrWhiteSpace(newIncidentDescription))
            throw new EmptyRequiredFieldException("Incident Description");

        IncidentDescription = newIncidentDescription;
    }
}