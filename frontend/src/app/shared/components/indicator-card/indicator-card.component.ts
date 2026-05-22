import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-indicator-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './indicator-card.component.html',
  styles: [`
    .card {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-4;
    }
    .card-healthy {
      @apply border-l-4 border-emerald-500;
    }
    .card-warning {
      @apply border-l-4 border-amber-400;
    }
    .card-danger {
      @apply border-l-4 border-red-500;
    }
    .label {
      @apply text-xs text-slate-500 uppercase tracking-widest;
    }
    .value {
      @apply text-3xl font-bold text-slate-900 tracking-tight;
    }
  `]
})
export class IndicatorCardComponent {
  label = input.required<string>();
  value = input<number | null>(null);
  status = input<'healthy' | 'warning' | 'danger' | null>(null);

  cardClasses = computed(() => {
    const statusValue = this.status();
    return `card ${this.getStatusClass(statusValue)}`;
  });

  formattedValue = computed(() => {
    const value = this.value();
    return value !== null ? value.toFixed(2) : 'N/A';
  });

  private getStatusClass(status: 'healthy' | 'warning' | 'danger' | null): string {
    if (status === 'healthy') {
      return 'card-healthy';
    }
    if (status === 'warning') {
      return 'card-warning';
    }
    if (status === 'danger') {
      return 'card-danger';
    }
    return '';
  }
}
