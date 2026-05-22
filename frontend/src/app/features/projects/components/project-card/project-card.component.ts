import { Component, input, output, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StatusBadgeComponent } from '../../../../shared/components/status-badge/status-badge.component';
import type { Project } from '../../../../core/models';

@Component({
  selector: 'app-project-card',
  standalone: true,
  imports: [CommonModule, StatusBadgeComponent],
  templateUrl: './project-card.component.html',
  styles: []
})
export class ProjectCardComponent {
  private readonly router = inject(Router);
  project = input.required<Project>();
  delete = output<string>();

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
