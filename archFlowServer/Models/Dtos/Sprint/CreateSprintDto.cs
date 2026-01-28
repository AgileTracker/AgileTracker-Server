namespace archFlowServer.Models.Dtos.Sprint;

public class CreateSprintDto
{
    public string Name { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public string? ExecutionPlan {  get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CapacityHours { get; set; } = 0;
}