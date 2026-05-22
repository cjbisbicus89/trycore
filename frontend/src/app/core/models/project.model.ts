import { Activity } from './activity.model';
import { EvmIndicators } from './evm-indicators.model';

export interface Project {
  id: string;
  name: string;
  description: string;
  budget: number;
  activities: Activity[];
  evmIndicators: EvmIndicators;
}
