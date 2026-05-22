import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-badge.component.html',
  styles: [`
    .badge {
      @apply px-2 py-1 rounded-full text-xs font-medium;
    }
    .badge-healthy {
      @apply bg-emerald-100 text-emerald-800;
    }
    .badge-warning {
      @apply bg-amber-100 text-amber-800;
    }
    .badge-danger {
      @apply bg-red-100 text-red-800;
    }
    .badge-neutral {
      @apply bg-slate-100 text-slate-800;
    }
  `]
})
export class StatusBadgeComponent {
  status = input.required<string>();

  badgeClasses = computed(() => {
    const statusValue = this.status();
    return `badge ${this.getColorClass(statusValue)}`;
  });

  private getColorClass(status: string): string {
    if (status === 'Bajo Presupuesto' || status === 'Adelantado al Cronograma') {
      return 'badge-healthy';
    }
    if (status === 'Sobre Presupuesto' || status === 'Atrasado al Cronograma') {
      return 'badge-danger';
    }
    if (status === 'En Presupuesto' || status === 'En Cronograma') {
      return 'badge-warning';
    }
    return 'badge-neutral';
  }
}
