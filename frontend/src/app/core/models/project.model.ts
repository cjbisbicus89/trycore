import { Activity } from './activity.model';
import { EvmIndicators } from './evm-indicators.model';

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  indicators: EvmIndicators | null;
  activities: Activity[];
}
