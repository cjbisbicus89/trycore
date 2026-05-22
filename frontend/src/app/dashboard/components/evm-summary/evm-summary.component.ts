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

  getCvStatus(): string {
    const cv = this.evmIndicators().costVariance;
    if (cv > 0) return 'Bajo Presupuesto';
    if (cv < 0) return 'Sobre Presupuesto';
    return 'En Presupuesto';
  }

  getSvStatus(): string {
    const sv = this.evmIndicators().scheduleVariance;
    if (sv > 0) return 'Adelantado al Cronograma';
    if (sv < 0) return 'Atrasado al Cronograma';
    return 'En Cronograma';
  }

  getCpiStatus(): string {
    return this.evmIndicators().costStatus;
  }

  getSpiStatus(): string {
    return this.evmIndicators().scheduleStatus;
  }
}
