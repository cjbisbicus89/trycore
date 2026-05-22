import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'projects', pathMatch: 'full' },
  {
    path: 'projects',
    loadComponent: () =>
      import('./features/projects/pages/project-list/project-list.component')
        .then(m => m.ProjectListComponent)
  },
  {
    path: 'dashboard/:id',
    loadComponent: () =>
      import('./dashboard/pages/dashboard/dashboard.component')
        .then(m => m.DashboardComponent)
  },
  { path: '**', redirectTo: 'projects' }
];
