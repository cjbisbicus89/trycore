import { Component, signal, inject, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ProjectService, CreateProjectRequest } from '../../../../core/services/project.service';
import { ProjectCardComponent } from '../../components/project-card/project-card.component';
import { ActivityConstants, ValidationMessages } from '../../../../core/constants';
import type { Project } from '../../../../core/models';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ProjectCardComponent],
  templateUrl: './project-list.component.html',
  styles: []
})
export class ProjectListComponent implements OnInit {
  private readonly projectService = inject(ProjectService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  projects = signal<Project[]>([]);
  isLoading = signal(true);
  isCreating = signal(false);
  showCreateForm = signal(false);

  projectForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(ActivityConstants.MaxNameLength)]],
    description: ['', [Validators.required, Validators.maxLength(ActivityConstants.MaxDescriptionLength)]]
  });

  protected readonly ValidationMessages = ValidationMessages;

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.isLoading.set(true);
    this.projectService.getAll()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (projects) => this.onProjectsLoaded(projects),
        error: () => this.isLoading.set(false)
      });
  }

  private onProjectsLoaded(projects: Project[]): void {
    this.projects.set(projects);
    this.isLoading.set(false);
  }

  createProject(): void {
    if (this.projectForm.invalid) {
      return;
    }

    this.isCreating.set(true);
    const request: CreateProjectRequest = this.projectForm.value as CreateProjectRequest;

    this.projectService.create(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (project) => this.onProjectCreated(project),
        error: () => this.isCreating.set(false)
      });
  }

  private onProjectCreated(project: Project): void {
    this.projects.update((projects) => [...projects, project]);
    this.projectForm.reset();
    this.showCreateForm.set(false);
    this.isCreating.set(false);
  }

  deleteProject(id: string): void {
    if (!confirm(ValidationMessages.ConfirmDeleteProject)) {
      return;
    }

    this.projectService.delete(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => this.onProjectDeleted(id),
        error: () => { /* El error es manejado por el interceptor */ }
      });
  }

  private onProjectDeleted(id: string): void {
    this.projects.update((projects) => projects.filter((p) => p.id !== id));
  }
}
