import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { FormatDecimalPipe } from '../../../shared/pipes/format-decimal/format-decimal.pipe';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent, FormatDecimalPipe],
  templateUrl: './activity-table.component.html',
  styles: [`
    .table-container {
      @apply overflow-x-auto;
    }
    .table {
      @apply w-full border-collapse;
    }
    .table th {
      @apply px-4 py-3 text-left text-xs font-medium text-slate-500 uppercase tracking-widest border-b-2 border-slate-200;
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
