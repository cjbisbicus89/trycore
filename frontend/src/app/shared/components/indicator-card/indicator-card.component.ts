import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../status-badge/status-badge.component';

@Component({
  selector: 'app-indicator-card',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent],
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
    .subtitle {
      @apply text-sm text-slate-600 mt-1;
    }
  `]
})
export class IndicatorCardComponent {
  label = input.required<string>();
  value = input<number | null>(null);
  subtitle = input<string>('');
  status = input<string | null>(null);

  cardClasses = computed(() => {
    const statusValue = this.status();
    return `card ${this.getStatusClass(statusValue)}`;
  });

  formattedValue = computed(() => {
    const value = this.value();
    return value !== null ? value.toFixed(2) : '—';
  });

  private getStatusClass(status: string | null): string {
    if (status === 'Bajo Presupuesto' || status === 'Adelantado al Cronograma') {
      return 'card-healthy';
    }
    if (status === 'Sobre Presupuesto' || status === 'Atrasado al Cronograma') {
      return 'card-danger';
    }
    if (status === 'En Presupuesto' || status === 'En Cronograma') {
      return 'card-warning';
    }
    return '';
  }
}
