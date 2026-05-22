import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import type { Activity } from '../models';

export interface CreateActivityRequest {
  projectId: string;
  name: string;
  budgetedCost: number;
  plannedPercentage: number;
  actualPercentage: number;
  actualCost: number;
}

export interface UpdateActivityRequest {
  name: string;
  budgetedCost: number;
  plannedPercentage: number;
  actualPercentage: number;
  actualCost: number;
}

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private readonly apiUrl = `${environment.apiUrl}/activities`;

  constructor(private readonly http: HttpClient) {}

  getByProjectId(projectId: string): Observable<Activity[]> {
    return this.http.get<Activity[]>(`${this.apiUrl}/project/${projectId}`);
  }

  getById(id: string): Observable<Activity> {
    return this.http.get<Activity>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateActivityRequest): Observable<Activity> {
    return this.http.post<Activity>(this.apiUrl, request);
  }

  update(id: string, request: UpdateActivityRequest): Observable<Activity> {
    return this.http.put<Activity>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
