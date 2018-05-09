import { Injectable } from '@angular/core';
import { Assignment } from './assignment.service';
import { environment } from '../../../environments/environment';
import { Http } from '@angular/http';
import { UserStorageService } from '../user-storage.service';

export class Progress {
  progressId: number;
  date: string;
  assignmentId: number;
  assignment: Assignment;

  constructor() {
    this.progressId = 0;
  }
}

@Injectable()
export class ProgressService {
  private baseUrl = `${environment.apiUrl}/progresses`;

  constructor(private http: Http, private userStorageService: UserStorageService) { }

  getProgresses(): Promise<Progress[]> {
    return this.http
      .get(`${this.baseUrl}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Progress[])
      .toPromise();
  }

  getProgressesByAssignment(assignmentId: number): Promise<Progress[]> {
    return this.http
      .get(`${this.baseUrl}?assignmentId=${assignmentId}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Progress[])
      .toPromise();
  }

  getProgress(id: number): Promise<Progress> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Progress)
      .toPromise();
  }

  createProgress(progress: Partial<Progress>): Promise<Progress> {
    return this.http
      .post(`${this.baseUrl}`, progress, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Progress)
      .toPromise();
  }

  editProgress(progress: Progress): Promise<boolean> {
    return this.http
    .put(`${this.baseUrl}/${progress.progressId}`, Progress, this.userStorageService.getAuthorizationRequestOptions())
    .map(response => response.ok)
    .toPromise();
  }

  deleteProgress(progressId: number): Promise<boolean> {
    return this.http
    .delete(`${this.baseUrl}/${progressId}`, this.userStorageService.getAuthorizationRequestOptions())
    .map(response => response.ok)
    .toPromise();
  }
}
