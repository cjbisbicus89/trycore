namespace EVM.ProjectManagement.Domain.Entities;

public static class ActivityErrors
{
    public const string NameIsRequired = "El nombre es obligatorio";
    public const string BudgetedCostMustBePositive = "El costo presupuestado debe ser mayor que cero";
    public const string PlannedPercentageExceedsMaximum = "El porcentaje planificado debe estar entre 0 y 100";
    public const string ActualPercentageExceedsMaximum = "El porcentaje actual debe estar entre 0 y 100";
    public const string ActualCostCannotBeNegative = "El costo actual no puede ser negativo";
    public const string ActivityNotFound = "Actividad con ID {0} no encontrada";
    public const string ProjectNotFound = "Proyecto con ID {0} no encontrado";
}
