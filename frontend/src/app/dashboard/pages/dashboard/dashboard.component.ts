import { Component, signal, computed, inject, OnInit, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { ActivityService } from '../../../core/services/activity.service';
import { ActivityTableComponent } from '../../components/activity-table/activity-table.component';
import type { Project, Activity } from '../../../core/models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ActivityTableComponent],
  template: `
    <div class="container">
      @if (isLoading()) {
        <div class="loading">
          <div class="skeleton-header"></div>
          <div class="skeleton-cards"></div>
          <div class="skeleton-table"></div>
        </div>
      } @else if (error()) {
        <div class="error-state">
          <p>{{ error() }}</p>
          <button class="btn-back" (click)="goBack()">Volver</button>
        </div>
      } @else if (project()) {
        <div class="header">
          <button class="btn-back" (click)="goBack()">← Volver</button>
          <div class="header-content">
            <h1 class="project-name">{{ project()!.name }}</h1>
            <p class="project-description">{{ project()!.description }}</p>
          </div>
        </div>

        <div class="indicators-grid">
          <div class="indicator-card">
            <div class="indicator-label">PV (Planned Value)</div>
            <div class="indicator-value">\${{ project()!.evmIndicators.pv?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">EV (Earned Value)</div>
            <div class="indicator-value">\${{ project()!.evmIndicators.ev?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">AC (Actual Cost)</div>
            <div class="indicator-value">\${{ project()!.evmIndicators.ac?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">CV (Cost Variance)</div>
            <div class="indicator-value">\${{ project()!.evmIndicators.cv?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">SV (Schedule Variance)</div>
            <div class="indicator-value">\${{ project()!.evmIndicators.sv?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">CPI (Cost Performance Index)</div>
            <div class="indicator-value">{{ project()!.evmIndicators.cpi?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">SPI (Schedule Performance Index)</div>
            <div class="indicator-value">{{ project()!.evmIndicators.spi?.toFixed(2) || 'N/A' }}</div>
          </div>
          <div class="indicator-card">
            <div class="indicator-label">Estado</div>
            <div class="indicator-value">{{ project()!.evmIndicators.status || 'N/A' }}</div>
          </div>
        </div>

        <div class="activities-section">
          <h2 class="section-title">Actividades</h2>
          <app-activity-table [activities]="activities()" />
        </div>
      }
    </div>
  `,
  styles: [`
    .container {
      @apply max-w-7xl mx-auto p-6;
    }
    .header {
      @apply mb-8;
    }
    .btn-back {
      @apply mb-4 px-4 py-2 bg-slate-200 text-slate-700 rounded-lg hover:bg-slate-300 transition-colors;
    }
    .header-content {
      @apply bg-gradient-to-r from-blue-600 via-blue-700 to-indigo-700 text-white rounded-lg p-8 shadow-lg;
    }
    .project-name {
      @apply text-4xl font-bold mb-2 tracking-tight;
    }
    .project-description {
      @apply text-blue-100 text-lg;
    }
    .indicators-grid {
      @apply grid grid-cols-2 md:grid-cols-4 gap-4 mb-8;
    }
    .indicator-card {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-4;
    }
    .indicator-label {
      @apply text-xs text-slate-500 uppercase tracking-widest mb-2;
    }
    .indicator-value {
      @apply text-2xl font-bold text-slate-900;
    }
    .activities-section {
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
}
