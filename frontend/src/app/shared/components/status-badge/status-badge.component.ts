import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EvmStatusNames } from '../../../core/constants';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-badge.component.html',
  styles: []
})
export class StatusBadgeComponent {
  status = input.required<string | null>();

  badgeClasses = computed(() => {
    const statusValue = this.status();
    return this.getColorClass(statusValue);
  });

  private getColorClass(status: string | null): string {
    const map: Record<string, string> = {
      [EvmStatusNames.UnderBudget]: 'bg-emerald-100 text-emerald-800',
      [EvmStatusNames.OverBudget]: 'bg-red-100 text-red-800',
      [EvmStatusNames.OnBudget]: 'bg-amber-100 text-amber-800',
      [EvmStatusNames.AheadOfSchedule]: 'bg-emerald-100 text-emerald-800',
      [EvmStatusNames.BehindSchedule]: 'bg-red-100 text-red-800',
      [EvmStatusNames.OnSchedule]: 'bg-amber-100 text-amber-800',
      [EvmStatusNames.NotAvailable]: 'bg-slate-100 text-slate-500',
    };
    return map[status ?? ''] ?? 'bg-slate-100 text-slate-500';
  }
}
