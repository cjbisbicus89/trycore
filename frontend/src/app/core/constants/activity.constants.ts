export const ActivityConstants = {
  MaxNameLength: 200,
  MaxDescriptionLength: 500,
  MaxPercentage: 100,
  MinPercentage: 0,
  MinBudgetedCost: 0.01,
  MinActualCost: 0,
} as const;

export const EvmStatusNames = {
  UnderBudget: 'Bajo Presupuesto',
  OverBudget: 'Sobre Presupuesto',
  OnBudget: 'En Presupuesto',
  AheadOfSchedule: 'Adelantado al Cronograma',
  BehindSchedule: 'Atrasado al Cronograma',
  OnSchedule: 'En Cronograma',
  NotAvailable: 'N/A',
} as const;
