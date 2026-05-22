import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { FormatDecimalPipe } from '../../../shared/pipes/format-decimal/format-decimal.pipe';
import { getActivityHealthRowClass } from '../../../core/utils/evm-health.utils';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent, FormatDecimalPipe],
  templateUrl: './activity-table.component.html',
  styles: []
})
export class ActivityTableComponent {
  activities = input.required<Activity[]>();
  edit = output<Activity>();
  delete = output<string>();

  getRowClass(activity: Activity): string {
    return getActivityHealthRowClass(activity.indicators);
  }

  onEdit(activity: Activity): void {
    this.edit.emit(activity);
  }

  onDelete(activityId: string): void {
    this.delete.emit(activityId);
  }
}
