import { Component, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ProjectService, CreateProjectRequest } from '../../../../core/services/project.service';
import { ProjectCardComponent } from '../../components/project-card/project-card.component';
import type { Project } from '../../../../core/models';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ProjectCardComponent],
  template: `
    <div class="container">
      <div class="header">
        <h1 class="title">Proyectos</h1>
        <button class="btn-primary" (click)="showCreateForm.set(true)">Nuevo Proyecto</button>
      </div>

      @if (showCreateForm()) {
        <div class="form-container">
          <h2 class="form-title">Crear Nuevo Proyecto</h2>
          <form [formGroup]="projectForm" (ngSubmit)="createProject()">
            <div class="form-group">
              <label for="name">Nombre</label>
              <input id="name" type="text" formControlName="name" placeholder="Nombre del proyecto" />
              @if (projectForm.get('name')?.invalid && projectForm.get('name')?.touched) {
                <span class="error">El nombre es requerido</span>
              }
            </div>
            <div class="form-group">
              <label for="description">Descripción</label>
              <textarea id="description" formControlName="description" placeholder="Descripción del proyecto"></textarea>
              @if (projectForm.get('description')?.invalid && projectForm.get('description')?.touched) {
                <span class="error">La descripción es requerida</span>
              }
            </div>
            <div class="form-group">
              <label for="budget">Presupuesto</label>
              <input id="budget" type="number" formControlName="budget" placeholder="0.00" step="0.01" />
              @if (projectForm.get('budget')?.invalid && projectForm.get('budget')?.touched) {
                <span class="error">El presupuesto debe ser mayor a 0</span>
              }
            </div>
            <div class="form-actions">
              <button type="button" class="btn-secondary" (click)="showCreateForm.set(false)">Cancelar</button>
              <button type="submit" class="btn-primary" [disabled]="isCreating()">Crear</button>
            </div>
          </form>
        </div>
      }

      @if (isLoading()) {
        <div class="loading">
          <div class="skeleton"></div>
          <div class="skeleton"></div>
          <div class="skeleton"></div>
        </div>
      } @else if (projects().length === 0) {
        <div class="empty-state">
          <p>No hay proyectos. Crea uno nuevo para comenzar.</p>
        </div>
      } @else {
        <div class="projects-grid">
          @for (project of projects(); track project.id) {
            <app-project-card [project]="project" (delete)="deleteProject($event)" />
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .container {
      @apply max-w-7xl mx-auto p-6;
    }
    .header {
      @apply flex justify-between items-center mb-8;
    }
    .title {
      @apply text-3xl font-bold text-slate-900;
    }
    .btn-primary {
      @apply px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors font-medium;
    }
    .btn-secondary {
      @apply px-4 py-2 bg-slate-200 text-slate-700 rounded-lg hover:bg-slate-300 transition-colors font-medium;
    }
    .form-container {
      @apply bg-white shadow-sm border border-slate-100 rounded-lg p-6 mb-8;
    }
    .form-title {
      @apply text-xl font-semibold text-slate-900 mb-4;
    }
    .form-group {
      @apply mb-4;
    }
    .form-group label {
      @apply block text-sm font-medium text-slate-700 mb-1;
    }
    .form-group input,
    .form-group textarea {
      @apply w-full px-3 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500;
    }
    .form-group textarea {
      @apply min-h-[100px] resize-y;
    }
    .error {
      @apply text-red-500 text-sm mt-1;
    }
    .form-actions {
      @apply flex justify-end gap-3 mt-6;
    }
    .loading {
      @apply grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6;
    }
    .skeleton {
      @apply bg-slate-200 rounded-lg h-48 animate-pulse;
    }
    .empty-state {
      @apply text-center py-12 text-slate-500;
    }
    .projects-grid {
      @apply grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6;
    }
  `]
})
export class ProjectListComponent {
  private readonly projectService = inject(ProjectService);
  private readonly fb = inject(FormBuilder);

  projects = signal<Project[]>([]);
  isLoading = signal(true);
  isCreating = signal(false);
  showCreateForm = signal(false);

  projectForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
    budget: [0, [Validators.required, Validators.min(0.01)]]
  });

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.isLoading.set(true);
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects.set(projects);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  createProject(): void {
    if (this.projectForm.invalid) {
      return;
    }

    this.isCreating.set(true);
    const request: CreateProjectRequest = this.projectForm.value as CreateProjectRequest;

    this.projectService.create(request).subscribe({
      next: (project) => {
        this.projects.update((projects) => [...projects, project]);
        this.projectForm.reset();
        this.showCreateForm.set(false);
        this.isCreating.set(false);
      },
      error: () => {
        this.isCreating.set(false);
      }
    });
  }

  deleteProject(id: string): void {
    if (!confirm('¿Estás seguro de eliminar este proyecto?')) {
      return;
    }

    this.projectService.delete(id).subscribe({
      next: () => {
        this.projects.update((projects) => projects.filter((p) => p.id !== id));
      },
      error: () => {
        // Handle error
      }
    });
  }
}
