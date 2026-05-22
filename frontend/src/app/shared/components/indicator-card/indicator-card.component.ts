import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../status-badge/status-badge.component';
import { EvmStatusNames } from '../../../core/constants';

@Component({
  selector: 'app-indicator-card',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent],
  templateUrl: './indicator-card.component.html',
  styles: []
})
export class IndicatorCardComponent {
  label = input.required<string>();
  value = input<number | null>(null);
  subtitle = input<string>('');
  status = input<string | null>(null);

  borderClass = computed(() => {
    const statusValue = this.status();
    return this.getBorderClass(statusValue);
  });

  formattedValue = computed(() => {
    const value = this.value();
    return value !== null ? value.toFixed(2) : '—';
  });

  private getBorderClass(status: string | null): string {
    if (status === EvmStatusNames.UnderBudget || status === EvmStatusNames.AheadOfSchedule) {
      return 'border-l-emerald-500';
    }
    if (status === EvmStatusNames.OverBudget || status === EvmStatusNames.BehindSchedule) {
      return 'border-l-red-500';
    }
    if (status === EvmStatusNames.OnBudget || status === EvmStatusNames.OnSchedule) {
      return 'border-l-amber-400';
    }
    return 'border-l-slate-300';
  }
}
