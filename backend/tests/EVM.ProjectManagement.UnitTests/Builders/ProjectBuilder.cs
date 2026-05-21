namespace EVM.ProjectManagement.UnitTests.Builders;

using EVM.ProjectManagement.Domain.Entities;

public sealed class ProjectBuilder
{
    private string name = "Test Project";
    private string description = "Test Description";

    public ProjectBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public ProjectBuilder WithDescription(string description)
    {
        this.description = description;
        return this;
    }

    public Project Build()
    {
        return Project.Create(this.name, this.description);
    }
}
