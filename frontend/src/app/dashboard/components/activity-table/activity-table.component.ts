import { Component, input, output, computed } from '@angular/core';
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
    .btn-edit {
      @apply px-2 py-1 text-xs bg-blue-600 text-white hover:bg-blue-700 rounded transition-colors mr-1;
    }
    .btn-delete {
      @apply px-2 py-1 text-xs text-red-600 hover:text-red-700 hover:bg-red-50 rounded transition-colors;
    }
  `]
})
export class ActivityTableComponent {
  activities = input.required<Activity[]>();
  edit = output<Activity>();
  delete = output<string>();

  getRowClasses(activity: Activity): string {
    const cpi = activity.indicators.costPerformanceIndex;
    const spi = activity.indicators.schedulePerformanceIndex;
    
    if (cpi !== null && spi !== null) {
      if (cpi < 1 || spi < 1) {
        return 'row-danger';
      }
      if (cpi > 1 && spi > 1) {
        return 'row-healthy';
      }
      return 'row-warning';
    }
    return 'row-neutral';
  }

  onEdit(activity: Activity): void {
    this.edit.emit(activity);
  }

  onDelete(activityId: string): void {
    this.delete.emit(activityId);
  }
}
