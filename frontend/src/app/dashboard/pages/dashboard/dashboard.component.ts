import { Component, signal, computed, inject, OnInit, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { ActivityService } from '../../../core/services/activity.service';
import { ActivityTableComponent } from '../../components/activity-table/activity-table.component';
import { EvmSummaryComponent } from '../../components/evm-summary/evm-summary.component';
import { EvmChartComponent } from '../../components/evm-chart/evm-chart.component';
import { ActivityFormModalComponent } from '../../components/activity-form-modal/activity-form-modal.component';
import type { Project, Activity } from '../../../core/models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ActivityTableComponent, EvmSummaryComponent, EvmChartComponent, ActivityFormModalComponent],
  templateUrl: './dashboard.component.html',
  styles: [`
    .container {
      @apply max-w-7xl mx-auto p-6;
    }
    .header {
      @apply mb-8 flex justify-between items-center;
    }
    .btn-back {
      @apply mb-4 px-4 py-2 bg-slate-200 text-slate-700 rounded-lg hover:bg-slate-300 transition-colors;
    }
    .btn-primary {
      @apply px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors font-medium;
    }
    .header-content {
      @apply bg-gradient-to-r from-blue-600 via-blue-700 to-indigo-700 text-white rounded-lg p-8 shadow-lg flex-1 mr-4;
    }
    .project-name {
      @apply text-4xl font-bold mb-2 tracking-tight;
    }
    .project-description {
      @apply text-blue-100 text-lg;
    }
    .activities-section {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-6 mb-8;
    }
    .chart-section {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-6;
    }
    .section-title {
      @apply text-xl font-semibold text-slate-900 mb-4;
    }
    .loading {
      @apply space-y-6;
    }
    .skeleton-header {
      @apply h-32 bg-slate-200 rounded-lg animate-pulse;
    }
    .skeleton-cards {
      @apply grid grid-cols-2 md:grid-cols-4 gap-4;
    }
    .skeleton-cards > div {
      @apply h-24 bg-slate-200 rounded-lg animate-pulse;
    }
    .skeleton-table {
      @apply h-64 bg-slate-200 rounded-lg animate-pulse;
    }
    .error-state {
      @apply text-center py-12 text-slate-500;
    }
  `]
})
export class DashboardComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly projectService = inject(ProjectService);
  private readonly activityService = inject(ActivityService);
  private readonly destroyRef = inject(DestroyRef);

  project = signal<Project | null>(null);
  activities = signal<Activity[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  showModal = signal(false);
  selectedActivity = signal<Activity | null>(null);

  consolidatedIndicators = computed(() => this.project()?.indicators ?? null);
  totalBudgetedCost = computed(() =>
    this.activities().reduce((sum, a) => sum + a.budgetedCost, 0)
  );

  ngOnInit(): void {
    const projectId = this.route.snapshot.paramMap.get('id');
    if (projectId) {
      this.loadDashboard(projectId);
    } else {
      this.error.set('ID de proyecto no proporcionado');
      this.isLoading.set(false);
    }
  }

  loadDashboard(projectId: string): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.projectService.getById(projectId).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (project) => {
        this.project.set(project);
        this.loadActivities(projectId);
      },
      error: () => {
        this.error.set('Error al cargar el proyecto');
        this.isLoading.set(false);
      }
    });
  }

  loadActivities(projectId: string): void {
    this.activityService.getByProjectId(projectId).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (activities) => {
        this.activities.set(activities);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar las actividades');
        this.isLoading.set(false);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/projects']);
  }

  openCreateModal(): void {
    this.selectedActivity.set(null);
    this.showModal.set(true);
  }

  openEditModal(activity: Activity): void {
    this.selectedActivity.set(activity);
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.selectedActivity.set(null);
  }

  onActivitySaved(): void {
    const projectId = this.project()?.id;
    if (projectId) {
      this.loadActivities(projectId);
      this.loadDashboard(projectId);
    }
    this.closeModal();
  }

  onActivityDeleted(activityId: string): void {
    if (!confirm('¿Estás seguro de eliminar esta actividad?')) {
      return;
    }

    this.activityService.delete(activityId).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: () => {
        const projectId = this.project()?.id;
        if (projectId) {
          this.loadActivities(projectId);
          this.loadDashboard(projectId);
        }
      },
      error: () => {
        // Handle error
      }
    });
  }
}
