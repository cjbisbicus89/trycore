import { Component, input, output, inject, OnInit, computed, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivityService, CreateActivityRequest, UpdateActivityRequest } from '../../../core/services/activity.service';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-activity-form-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './activity-form-modal.component.html',
  styles: [`
    .modal-overlay {
      @apply fixed inset-0 bg-black/50 flex items-center justify-center z-50 transition-opacity;
    }
    .modal-content {
      @apply bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transition-all;
    }
    .modal-header {
      @apply flex justify-between items-center p-6 border-b border-slate-200;
    }
    .modal-title {
      @apply text-xl font-semibold text-slate-900;
    }
    .btn-close {
      @apply text-slate-400 hover:text-slate-600 text-2xl leading-none;
    }
    .form-group {
      @apply p-6 pt-4;
    }
    .form-group label {
      @apply block text-sm font-medium text-slate-700 mb-1;
    }
    .form-group input {
      @apply w-full px-3 py-2 border border-slate-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500;
    }
    .error {
      @apply text-red-500 text-sm mt-1;
    }
    .form-actions {
      @apply flex justify-end gap-3 p-6 border-t border-slate-200;
    }
    .btn-primary {
      @apply px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50;
    }
    .btn-secondary {
      @apply px-4 py-2 bg-slate-200 text-slate-700 rounded-lg hover:bg-slate-300 transition-colors disabled:opacity-50;
    }
  `]
})
export class ActivityFormModalComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly activityService = inject(ActivityService);
  private readonly destroyRef = inject(DestroyRef);

  projectId = input.required<string>();
  activity = input<Activity | null>(null);
  saved = output<void>();
  cancelled = output<void>();

  isOpen = input(true);
  isSaving = false;

  activityForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    budgetedCost: [0, [Validators.required, Validators.min(0.01)]],
    plannedPercentage: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    actualPercentage: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    actualCost: [0, [Validators.required, Validators.min(0)]]
  });

  isEditMode = computed(() => this.activity() !== null);

  ngOnInit(): void {
    if (this.activity()) {
      this.activityForm.patchValue(this.activity()!);
    }
  }

  save(): void {
    if (this.activityForm.invalid) {
      return;
    }

    this.isSaving = true;
    const formValue = this.activityForm.value;

    if (this.isEditMode()) {
      const request: UpdateActivityRequest = {
        name: formValue.name!,
        budgetedCost: formValue.budgetedCost!,
        plannedPercentage: formValue.plannedPercentage!,
        actualPercentage: formValue.actualPercentage!,
        actualCost: formValue.actualCost!
      };

      this.activityService.update(this.activity()!.id, request).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
        next: () => {
          this.isSaving = false;
          this.saved.emit();
        },
        error: () => {
          this.isSaving = false;
        }
      });
    } else {
      const request: CreateActivityRequest = {
        projectId: this.projectId(),
        name: formValue.name!,
        budgetedCost: formValue.budgetedCost!,
        plannedPercentage: formValue.plannedPercentage!,
        actualPercentage: formValue.actualPercentage!,
        actualCost: formValue.actualCost!
      };

      this.activityService.create(request).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
        next: () => {
          this.isSaving = false;
          this.saved.emit();
        },
        error: () => {
          this.isSaving = false;
        }
      });
    }
  }

  cancel(): void {
    this.cancelled.emit();
  }
}
