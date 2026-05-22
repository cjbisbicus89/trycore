import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { FormatDecimalPipe } from '../../../shared/pipes/format-decimal/format-decimal.pipe';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent, FormatDecimalPipe],
  template: `
    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>Actividad</th>
            <th>Costo Presupuestado</th>
            <th>% Planificado</th>
            <th>% Actual</th>
            <th>Costo Real</th>
            <th>CPI</th>
            <th>SPI</th>
            <th>Estado</th>
          </tr>
        </thead>
        <tbody>
          @for (activity of activities(); track activity.id) {
            <tr [class]="getRowClasses(activity)">
              <td class="activity-name">{{ activity.name }}</td>
              <td>\${{ activity.budgetedCost | formatDecimal }}</td>
              <td>{{ activity.plannedPercentage }}%</td>
              <td>{{ activity.actualPercentage }}%</td>
              <td>\${{ activity.actualCost | formatDecimal }}</td>
              <td>{{ activity.evmIndicators.cpi | formatDecimal }}</td>
              <td>{{ activity.evmIndicators.spi | formatDecimal }}</td>
              <td>
                <app-status-badge [status]="activity.evmIndicators.status || 'N/A'" />
              </td>
            </tr>
          }
        </tbody>
      </table>
    </div>
  `,
  styles: [`
    .table-container {
      @apply overflow-x-auto;
    }
    .table {
      @apply w-full border-collapse;
    }
    .table th {
      @apply px-4 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-widest border-b border-slate-200;
    }
    .table td {
      @apply px-4 py-3 text-sm text-slate-900 border-b border-slate-100;
    }
    .activity-name {
      @apply font-medium;
    }
    .row-healthy {
      @apply bg-emerald-50 hover:bg-emerald-100;
    }
    .row-warning {
      @apply bg-amber-50 hover:bg-amber-100;
    }
    .row-danger {
      @apply bg-red-50 hover:bg-red-100;
    }
    .row-neutral {
      @apply hover:bg-slate-50;
    }
  `]
})
export class ActivityTableComponent {
  activities = input.required<Activity[]>();

  getRowClasses(activity: Activity): string {
    const status = activity.evmIndicators.status?.toLowerCase();
    if (status === 'healthy' || status === 'on track') {
      return 'row-healthy';
    }
    if (status === 'warning' || status === 'at risk') {
      return 'row-warning';
    }
    if (status === 'danger' || status === 'off track') {
      return 'row-danger';
    }
    return 'row-neutral';
  }
}
