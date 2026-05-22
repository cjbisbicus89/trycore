import { Component, input, computed } from '@angular/core';
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

  getPvStatus(): 'healthy' | 'warning' | 'danger' | null {
    return null;
  }

  getEvStatus(): 'healthy' | 'warning' | 'danger' | null {
    return null;
  }

  getAcStatus(): 'healthy' | 'warning' | 'danger' | null {
    return null;
  }

  getCvStatus(): 'healthy' | 'warning' | 'danger' | null {
    const cv = this.evmIndicators().cv;
    if (cv === null || cv === undefined) return null;
    return cv >= 0 ? 'healthy' : 'danger';
  }

  getSvStatus(): 'healthy' | 'warning' | 'danger' | null {
    const sv = this.evmIndicators().sv;
    if (sv === null || sv === undefined) return null;
    return sv >= 0 ? 'healthy' : 'danger';
  }

  getCpiStatus(): 'healthy' | 'warning' | 'danger' | null {
    const cpi = this.evmIndicators().cpi;
    if (cpi === null || cpi === undefined) return null;
    if (cpi >= 1) return 'healthy';
    if (cpi >= 0.9) return 'warning';
    return 'danger';
  }

  getSpiStatus(): 'healthy' | 'warning' | 'danger' | null {
    const spi = this.evmIndicators().spi;
    if (spi === null || spi === undefined) return null;
    if (spi >= 1) return 'healthy';
    if (spi >= 0.9) return 'warning';
    return 'danger';
  }

  getOverallStatus(): 'healthy' | 'warning' | 'danger' | null {
    const status = this.evmIndicators().status?.toLowerCase();
    if (status === 'healthy' || status === 'on track') return 'healthy';
    if (status === 'warning' || status === 'at risk') return 'warning';
    if (status === 'danger' || status === 'off track') return 'danger';
    return null;
  }
}
