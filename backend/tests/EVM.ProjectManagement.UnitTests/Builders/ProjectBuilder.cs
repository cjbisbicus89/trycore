using EVM.ProjectManagement.Domain.Entities;

namespace EVM.ProjectManagement.UnitTests.Builders;

public sealed class ProjectBuilder
{
    private string _name = "Test Project";
    private string _description = "Test Description";

    public ProjectBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public Project Build()
    {
        return Project.Create(_name, _description);
    }
}
