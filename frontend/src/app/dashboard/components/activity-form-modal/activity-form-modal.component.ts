import { Component, input, output, inject, OnInit, computed, DestroyRef, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivityService, CreateActivityRequest, UpdateActivityRequest } from '../../../core/services/activity.service';
import { ActivityConstants, ValidationMessages } from '../../../core/constants';
import type { Activity } from '../../../core/models';

@Component({
  selector: 'app-activity-form-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './activity-form-modal.component.html',
  styles: []
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
  isSaving = signal(false);

  activityForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(ActivityConstants.MaxNameLength)]],
    budgetedCost: [0, [Validators.required, Validators.min(ActivityConstants.MinBudgetedCost)]],
    plannedPercentage: [0, [Validators.required, Validators.min(ActivityConstants.MinPercentage), Validators.max(ActivityConstants.MaxPercentage)]],
    actualPercentage: [0, [Validators.required, Validators.min(ActivityConstants.MinPercentage), Validators.max(ActivityConstants.MaxPercentage)]],
    actualCost: [0, [Validators.required, Validators.min(ActivityConstants.MinActualCost)]]
  });

  isEditMode = computed(() => this.activity() !== null);
  protected readonly ValidationMessages = ValidationMessages;

  ngOnInit(): void {
    if (this.activity()) {
      this.activityForm.patchValue(this.activity()!);
    }
  }

  save(): void {
    if (this.activityForm.invalid) {
      return;
    }

    this.isSaving.set(true);
    const formValue = this.activityForm.value;

    if (this.isEditMode()) {
      this.updateActivity(formValue);
    } else {
      this.createActivity(formValue);
    }
  }

  private updateActivity(formValue: typeof this.activityForm.value): void {
    const request: UpdateActivityRequest = {
      name: formValue.name!,
      budgetedCost: formValue.budgetedCost!,
      plannedPercentage: formValue.plannedPercentage!,
      actualPercentage: formValue.actualPercentage!,
      actualCost: formValue.actualCost!
    };

    this.activityService.update(this.activity()!.id, request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => this.onSaveSuccess(),
        error: () => this.isSaving.set(false)
      });
  }

  private createActivity(formValue: typeof this.activityForm.value): void {
    const request: CreateActivityRequest = {
      projectId: this.projectId(),
      name: formValue.name!,
      budgetedCost: formValue.budgetedCost!,
      plannedPercentage: formValue.plannedPercentage!,
      actualPercentage: formValue.actualPercentage!,
      actualCost: formValue.actualCost!
    };

    this.activityService.create(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => this.onSaveSuccess(),
        error: () => this.isSaving.set(false)
      });
  }

  private onSaveSuccess(): void {
    this.isSaving.set(false);
    this.saved.emit();
  }

  cancel(): void {
    this.cancelled.emit();
  }
}
