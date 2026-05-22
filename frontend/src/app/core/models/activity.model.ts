import { EvmIndicators } from './evm-indicators.model';

export interface Activity {
  id: string;
  projectId: string;
  name: string;
  budgetedCost: number;
  plannedPercentage: number;
  actualPercentage: number;
  actualCost: number;
  plannedValue: number;
  earnedValue: number;
  indicators: EvmIndicators;
}
