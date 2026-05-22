import { Component, input, output, computed } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../../../../shared/components/status-badge/status-badge.component';
import type { Project } from '../../../../core/models';

@Component({
  selector: 'app-project-card',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent],
  templateUrl: './project-card.component.html',
  styles: [`
    .card {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-6 cursor-pointer hover:shadow-lg hover:border-blue-200 transition-all duration-200;
    }
    .card-header {
      @apply flex justify-between items-start mb-3;
    }
    .project-name {
      @apply text-lg font-semibold text-slate-900;
    }
    .description {
      @apply text-slate-600 text-sm mb-4 line-clamp-2;
    }
    .metrics {
      @apply flex gap-4 mb-4;
    }
    .metric {
      @apply flex flex-col;
    }
    .metric-label {
      @apply text-xs text-slate-500 uppercase tracking-widest;
    }
    .metric-value {
      @apply text-sm font-medium text-slate-900;
    }
    .actions {
      @apply flex justify-end gap-2;
    }
    .btn-view {
      @apply px-3 py-1 text-sm bg-blue-600 text-white hover:bg-blue-700 rounded transition-colors;
    }
    .btn-delete {
      @apply px-3 py-1 text-sm text-red-600 hover:text-red-700 hover:bg-red-50 rounded transition-colors;
    }
  `]
})
export class ProjectCardComponent {
  project = input.required<Project>();
  delete = output<string>();

  constructor(private readonly router: Router) {}

  formattedDate = computed(() => {
    const date = new Date(this.project().createdAt);
    return date.toLocaleDateString('es-ES', { year: 'numeric', month: 'short', day: 'numeric' });
  });

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard', this.project().id]);
  }

  deleteProject(event: Event): void {
    event.stopPropagation();
    this.delete.emit(this.project().id);
  }
}
