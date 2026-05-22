export const ValidationMessages = {
  NameRequired: 'El nombre es obligatorio.',
  NameTooLong: 'El nombre no puede superar los 200 caracteres.',
  DescriptionRequired: 'La descripción es obligatoria.',
  DescriptionTooLong: 'La descripción no puede superar los 500 caracteres.',
  BudgetedCostInvalid: 'El costo presupuestado debe ser mayor a cero.',
  PlannedPercentageRange: 'El porcentaje planificado debe estar entre 0 y 100.',
  ActualPercentageRange: 'El porcentaje real debe estar entre 0 y 100.',
  ActualCostInvalid: 'El costo real no puede ser negativo.',
  ConfirmDeleteProject: '¿Estás seguro de eliminar este proyecto?',
  ConfirmDeleteActivity: '¿Estás seguro de eliminar esta actividad?',
} as const;
