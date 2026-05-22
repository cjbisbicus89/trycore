import { EvmThresholds } from '../constants/evm.constants';
import type { EvmIndicators } from '../models';

export function getActivityHealthRowClass(indicators: EvmIndicators): string {
  const { costPerformanceIndex: cpi, schedulePerformanceIndex: spi } = indicators;
  const threshold = EvmThresholds.PerformanceOptimal;
  if (cpi !== null && spi !== null && cpi > threshold && spi > threshold)
    return 'bg-emerald-50';
  if ((cpi !== null && cpi < threshold) || (spi !== null && spi < threshold))
    return 'bg-red-50';
  return '';
}
