import { EvmIndicators } from './evm-indicators.model';

export interface Activity {
  id: string;
  name: string;
  budgetedCost: number;
  plannedPercentage: number;
  actualPercentage: number;
  actualCost: number;
  evmIndicators: EvmIndicators;
}
