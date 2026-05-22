import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IndicatorCardComponent } from '../../../shared/components/indicator-card/indicator-card.component';
import type { EvmIndicators } from '../../../core/models';

@Component({
  selector: 'app-evm-summary',
  standalone: true,
  imports: [CommonModule, IndicatorCardComponent],
  templateUrl: './evm-summary.component.html',
  styles: [`
    .summary-grid {
      @apply grid grid-cols-2 md:grid-cols-4 gap-4;
    }
  `]
})
export class EvmSummaryComponent {
  evmIndicators = input.required<EvmIndicators>();
  totalBudgetedCost = input.required<number>();

  getPvStatus(): string | null {
    return null;
  }

  getEvStatus(): string | null {
    return null;
  }

  getAcStatus(): string | null {
    return null;
  }

  getCvStatus(): string | null {
    const cv = this.evmIndicators().costVariance;
    return cv >= 0 ? 'Bajo Presupuesto' : 'Sobre Presupuesto';
  }

  getSvStatus(): string | null {
    const sv = this.evmIndicators().scheduleVariance;
    return sv >= 0 ? 'Adelantado al Cronograma' : 'Atrasado al Cronograma';
  }

  getCpiStatus(): string | null {
    const cpi = this.evmIndicators().costPerformanceIndex;
    if (cpi === null) return null;
    if (cpi >= 1) return 'Bajo Presupuesto';
    if (cpi >= 0.9) return 'En Presupuesto';
    return 'Sobre Presupuesto';
  }

  getSpiStatus(): string | null {
    const spi = this.evmIndicators().schedulePerformanceIndex;
    if (spi === null) return null;
    if (spi >= 1) return 'Adelantado al Cronograma';
    if (spi >= 0.9) return 'En Cronograma';
    return 'Atrasado al Cronograma';
  }

  getOverallStatus(): string | null {
    return this.evmIndicators().costStatus;
  }
}
