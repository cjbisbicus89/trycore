import { Component, signal, computed, inject, OnInit, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { ActivityService } from '../../../core/services/activity.service';
import { ValidationMessages } from '../../../core/constants';
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
  styles: []
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
  protected readonly ValidationMessages = ValidationMessages;

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

    this.projectService.getById(projectId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (project) => this.onDashboardLoaded(project, projectId),
        error: () => this.onDashboardError()
      });
  }

  private onDashboardLoaded(project: Project, projectId: string): void {
    this.project.set(project);
    this.loadActivities(projectId);
  }

  private onDashboardError(): void {
    this.error.set('Error al cargar el proyecto');
    this.isLoading.set(false);
  }

  loadActivities(projectId: string): void {
    this.activityService.getByProjectId(projectId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (activities) => this.onActivitiesLoaded(activities),
        error: () => this.onActivitiesError()
      });
  }

  private onActivitiesLoaded(activities: Activity[]): void {
    this.activities.set(activities);
    this.isLoading.set(false);
  }

  private onActivitiesError(): void {
    this.error.set('Error al cargar las actividades');
    this.isLoading.set(false);
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
    if (!confirm(ValidationMessages.ConfirmDeleteActivity)) {
      return;
    }

    this.activityService.delete(activityId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => this.onActivityDeletedSuccess(),
        error: () => { /* El error es manejado por el interceptor */ }
      });
  }

  private onActivityDeletedSuccess(): void {
    const projectId = this.project()?.id;
    if (projectId) {
      this.loadActivities(projectId);
      this.loadDashboard(projectId);
    }
  }
}
