export interface EvmIndicators {
  plannedValue: number;
  earnedValue: number;
  actualCost: number;
  costVariance: number;
  scheduleVariance: number;
  costPerformanceIndex: number | null;
  schedulePerformanceIndex: number | null;
  estimateAtCompletion: number | null;
  varianceAtCompletion: number | null;
  costStatus: string;
  scheduleStatus: string;
}
